using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public class ShapeSprite<T> : RenderedNode where T : Shape
{ 
    public readonly T UnderlyingShape;

    private ShapeSprite(T shape) : base(1)
        => UnderlyingShape = shape;

    public Vector2f Position
    {
        get => UnderlyingShape.Position;
        set => UnderlyingShape.Position = value;
    }
    public Color FillColor
    {
        get => UnderlyingShape.FillColor;
        set => UnderlyingShape.FillColor = value;
    }

    public static ShapeSprite<CircleShape> CreateCircleSprite(float radius = 0)
        => new(new(radius));

    public static ShapeSprite<RectangleShape> CreateRectangleSprite(Vector2f size)
        => new(new(size));
    
    public override void Draw(RenderTarget target, RenderStates states)
        => UnderlyingShape.Draw(target, states);
}