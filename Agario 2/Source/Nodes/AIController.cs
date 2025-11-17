using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using MyEngine.Utils;
using SFML.System;

namespace Agario_2.Nodes;

public class AiController : Controller<Player>
{
    private Vector2f _currentWayPoint;

    private const float MaxDistanceToWayPoint = 500;
    private const float DistanceSquaredForNewWayPoint = 100;

    private Vector2f Position
        => Controlled?.Position ?? _currentWayPoint;
    
    private AiController()
    { }
    
    protected override void SetControlled(Player newControlled)
    {
        base.SetControlled(newControlled);
        
        _currentWayPoint = newControlled.Position;
        SetNewWayPoint();
    }

    public static AiController CreateAiController(Player player)
    {
        AiController result = new();
        
        result.Controlled = player;
        result.SetNewWayPoint();

        return result;
    }

    private void SetNewWayPoint()
    {
        float x = MyRandom.GetFloatInDistance(Position.X, MaxDistanceToWayPoint);
        float y = MyRandom.GetFloatInDistance(Position.Y, MaxDistanceToWayPoint);
        
        _currentWayPoint = new(x, y);
    }
    
    public override void Update(in UpdateInfo info)
    {
        if (Controlled == null)
            return;
        
        UpdateDelta();
        
        if (TooCloseToWayPoint())
            SetNewWayPoint();
    }

    private bool TooCloseToWayPoint()
        => Controlled.Position.SquaredDistanceTo(_currentWayPoint) < DistanceSquaredForNewWayPoint;
    
    private void UpdateDelta()
    {
        Vector2f delta = _currentWayPoint - Controlled.Position;
        delta /= delta.Length();
        delta *= Controlled.MaxSpeed;
        
        Controlled.WishedDelta = delta;
    }
}