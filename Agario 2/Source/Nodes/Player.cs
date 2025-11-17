using Agario_2.Configs;
using MyEngine;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.SoundSystem;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class Player : Node, IUpdatable
{
    public UpdateLayer UpdateLayer 
        => UpdateLayer.Normal;

    private EatableCircle _body;
    public Vector2f WishedDelta;
    private float _maxSpeed;
    private float _maxSpeedSquared;
    
    private int _dashingFramesLeft;
    
    private EatableCircle Body
    {
        get => _body;
        set
        {
            if (_body != null)
            {
                DetachChild(_body);
                _body.OnEaten -= Kill;
            }

            AdoptChild(value);
            value.OnEaten += Kill;
            _body = value;
        }
    }
    
    private Camera _draggedCamera;
    private Vector2f? _baseCameraSize;
    public Camera DraggedCamera
    {
        get => _draggedCamera;
        set
        {
            _draggedCamera = value;
            if (value != null)
                _baseCameraSize = value.Size; // remember initial camera size to scale from
        }
    }

    public float MaxSpeed
    {
        get => _maxSpeed;
        private set
        {
            _maxSpeed = value;
            _maxSpeedSquared = value * value;
        }
    }

    private float Radius
    {
        get => _body.Radius;
        set => _body.Radius = value;
    }
    
    public Vector2f Position
    {
        get => _body.Position;
        set => _body.Position = value;
    }

    private float CurrentDashSpeedMultiplier
        => _dashingFramesLeft > 0 ? PlayerConfigs.DashSpeedMultiplier : 1;
    
    private Player()
    {
        _dashingFramesLeft = 0;
        WishedDelta = new(0, 0);
        _maxSpeed = PlayerConfigs.StartingMaxSpeed;
        _maxSpeedSquared = PlayerConfigs.StartingMaxSpeed * PlayerConfigs.StartingMaxSpeed;
    }

    public static Player CreatePlayerWithNoController(Vector2f position)
    {
        Player result = new();
        
        result.Body = EatableCircle.CreateEatableCircle(PlayerConfigs.StartingRadius, position);
        
        return result;
    }
    
    private Vector2f CalculateCappedDelta()
    {
        float wishedDeltaLengthSquared = WishedDelta.LengthSquared();

        if (wishedDeltaLengthSquared < _maxSpeedSquared) 
            return WishedDelta;
        
        float wishedDeltaLength = float.Sqrt(wishedDeltaLengthSquared);
        return WishedDelta / wishedDeltaLength * _maxSpeed;
    }
    
    public void Update(in UpdateInfo updateInfo)
    {
        CheckForEatingInNode(updateInfo.Root);
        
        DoMovement(updateInfo.Time);

        ProcessDash();
        TryDragCamera(updateInfo.Time);
    }

    private void ProcessDash()
    {
        if (_dashingFramesLeft != 0)
        {
            _dashingFramesLeft--;
        }
    }

    public void Dash()
    {
        _dashingFramesLeft = PlayerConfigs.DashSpanFrames;
        SoundManager.CreateSound("dash").WithRandomizedPitch(min: 0.7f, max: 1.3f).Play();
    }

    public void SwapBodies()
    {
        Node root = GetRootNode();

        List<Player> players = root.GetChildrenOfType<Player>();
        Player randomPlayer = players.GetRandomElement();

        (randomPlayer.Body, Body) = (Body, randomPlayer.Body);
    }
    
    private const float MaxZoomOutMultiplier = 4f; // limit how far we can zoom out automatically
    private const float ZoomLerpSpeed = 3f;        // how fast camera zoom adapts

    private void TryDragCamera(FrameTiming time)
    {
        if (DraggedCamera == null)
            return;

        float interpolation = time.DeltaSeconds * 5;
        DraggedCamera.Position = DraggedCamera.Position.Lerp(Position, interpolation);

        // Adjust camera zoom based on player size so big player doesn't fill the whole screen
        if (_baseCameraSize.HasValue)
        {
            float radiusRatio = Radius / PlayerConfigs.StartingRadius;
            float targetMultiplier = float.Clamp(radiusRatio, 1f, MaxZoomOutMultiplier);

            Vector2f targetSize = _baseCameraSize.Value * targetMultiplier;
            float zoomLerp = time.DeltaSeconds * ZoomLerpSpeed;
            DraggedCamera.Size = DraggedCamera.Size.Lerp(targetSize, zoomLerp);
        }
    }
    
    private void DoMovement(FrameTiming time)
    {
        Vector2f delta = CalculateCappedDelta() * time.DeltaSeconds * CurrentDashSpeedMultiplier;
        Position += delta;
    }

    private void CheckForEatingInNode(Node root)
    {
        foreach (Node child in root)
        {
            if (child is EatableCircle eatableCircle)
                TryEat(eatableCircle);
            else
                CheckForEatingInNode(child);
        }
    }

    private void TryEat(EatableCircle eatable)
    {
        if (eatable.Radius >= Radius)
            return;

        if (!_body.Encloses(eatable))
            return;

        Radius += eatable.Eat() * (1 / float.Log2(Radius));
        UpdateMaxSpeed();
    }

    private void UpdateMaxSpeed()
        => MaxSpeed = PlayerConfigs.StartingMaxSpeed / float.Max(1, float.Log10(Radius - PlayerConfigs.StartingRadius));

    public void SetSkin(Color skin)
    {
        Body.Color = skin;
    }
}