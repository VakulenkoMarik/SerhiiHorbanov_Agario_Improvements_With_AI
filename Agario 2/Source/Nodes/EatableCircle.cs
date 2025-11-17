using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class EatableCircle : Node
{
    private ShapeSprite<CircleShape> _sprite; 
    public Action OnEaten;

    public static readonly Color[] Colors = { Color.Blue, Color.Red, Color.Green, Color.Cyan, Color.Magenta, Color.Yellow };
    
    public Vector2f Position
    {
        get => _sprite.Position;
        set => _sprite.Position = value;
    }
    public float Radius
    {
        get => _sprite.UnderlyingShape.Radius;
        set
        {
            _sprite.UnderlyingShape.Radius = value;   
            _sprite.UnderlyingShape.Origin = new(value, value);
        }
    }

    public Color Color
    {
        get => _sprite.UnderlyingShape.FillColor;
        set => _sprite.UnderlyingShape.FillColor = value;
    }
    
    public static EatableCircle CreateEatableCircle(float radius, Vector2f position)
    {
        EatableCircle result = new();

        result._sprite = ShapeSprite<CircleShape>.CreateCircleSprite(radius);
        result.Radius = radius;
        result.AdoptChild(result._sprite);
        result.Position = position;
        result._sprite.FillColor = GetRandomColor();
        
        return result;
    }

    public bool Encloses(EatableCircle other)
    {
        float radiusSquared = Radius * Radius;
        float otherRadiusSquared = other.Radius * other.Radius;

        float distanceSquared = Position.SquaredDistanceTo(other.Position);
        
        return distanceSquared + otherRadiusSquared < radiusSquared;
    }
    
    public float Eat()
    {
        OnEaten();
        return Radius;
    }

    private static Color GetRandomColor()
        => Colors.GetRandomElement();
}