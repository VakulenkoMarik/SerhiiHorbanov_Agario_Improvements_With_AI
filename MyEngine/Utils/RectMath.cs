using SFML.Graphics;
using SFML.System;

namespace MyEngine.Utils;

public static class RectMath
{
    public static void SetCenter(this IntRect rect, Vector2i center)
    {
        rect.Left = (int)(center.X - rect.Width / 2);
        rect.Top = (int)(center.Y - rect.Height / 2);
    }
}