using SFML.System;
using SFML.Window;

namespace MyEngine.MyInput;

public static class MouseInput
{
    private static WindowBase _window;
    
    public static Vector2f MousePositionOnWindow;
    public static Vector2f MousePositionFromWindowCenter;

    public static void AddListenerTo(WindowBase window)
    {
        window.MouseMoved += UpdateInput;
        
        _window = window;
    }
    
    private static void UpdateInput(object sender, MouseMoveEventArgs e)
    {
        MousePositionOnWindow = (Vector2f)Mouse.GetPosition(_window);

        MousePositionFromWindowCenter = MousePositionOnWindow - ((Vector2f)_window.Size * 0.5f);
    }
}