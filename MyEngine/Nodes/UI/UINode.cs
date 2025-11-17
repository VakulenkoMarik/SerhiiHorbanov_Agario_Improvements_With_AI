using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.System;

namespace MyEngine.Nodes.UI;

public abstract class UINode : Node
{
    protected readonly UICanvas Canvas;

    private Vector2f _anchorOnTarget;
    private Vector2i _offset;
    
    private Vector2i AnchorOffset 
        => (Vector2i)_anchorOnTarget.Scale(Canvas.Size);

    public Vector2f AnchorOnTarget
    {
        get => _anchorOnTarget;
        set
        {
            _anchorOnTarget = value;
            OnPositionSet();
        }
    }

    public Vector2i Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            OnPositionSet();
        }
    }
    
    public Vector2i Position
    {
        get => AnchorOffset + _offset;
        set
        {
            _offset = value - AnchorOffset;
            OnPositionSet();
        }
    }

    
    protected UINode(UICanvas canvas)
    {
        Canvas = canvas;
    }

    protected Vector2i CalculatePositionOnView(Vector2f positionOnTarget)
        => Canvas.Camera.CalculatePositionOnView((Vector2i)positionOnTarget);
    
    protected abstract void OnPositionSet();
}