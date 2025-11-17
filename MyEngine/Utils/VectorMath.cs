using SFML.Graphics;
using SFML.System;

namespace MyEngine.Utils;

public static class VectorMath
{
    public static Vector2f Scale(this Vector2f a, Vector2f b)
        => new(a.X * b.X, a.Y * b.Y);
    public static Vector2f Scale(this Vector2f a, Vector2u b)
        => a.Scale((Vector2f)b);
    public static Vector2f Scale(this Vector2f a, Vector2i b)
        => a.Scale((Vector2f)b);
    
    public static float LengthSquared(this Vector2f vector)
        => vector.SquaredDistanceTo(new Vector2f(0, 0));
    
    public static float Length(this Vector2f vector)
        => float.Sqrt(vector.LengthSquared());
    
    public static float SquaredDistanceTo(this Vector2f from, Vector2f to)
    {
        Vector2f diff = from - to;
        return (diff.X * diff.X) + (diff.Y * diff.Y);
    }

    public static Vector2f Lerp(this Vector2f from, Vector2f to, float interpolation)
        => from + ((to - from) * interpolation);

    public static Vector2f RandomPositionInside(this FloatRect bounds)
    {
        float x = Random.Shared.NextSingle() * bounds.Width + bounds.Left;
        float y = Random.Shared.NextSingle() * bounds.Height + bounds.Top;
        
        return new Vector2f(x, y);
    }
}