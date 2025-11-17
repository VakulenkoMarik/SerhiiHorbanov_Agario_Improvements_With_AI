using SFML.Graphics;
using SFML.Window;

namespace MyEngine.MyInput;

public static class MouseWheel
{
    public static float DeltaSinceStart;
    public static float LastDelta;
    
    public static void AddListenerTo(RenderWindow window)
        => window.MouseWheelScrolled += UpdateDeltas;

    private static void UpdateDeltas(object o, MouseWheelScrollEventArgs args)
    {
        if (args.Wheel != Mouse.Wheel.VerticalWheel)
            return;

        DeltaSinceStart += args.Delta;
        LastDelta = args.Delta;
    }
}