using MyEngine.MyInput;
using MyEngine.Nodes.Graphics;
using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.UI;

public sealed class UICanvas : Node
{
    private Camera _camera;
    public InputSystem Input { get; init; }

    public Camera Camera
    {
        get => _camera;
        private set
        {
            AdoptChild(value);
            _camera = value;
        }
    }

    private RenderTarget RenderTarget
        => _camera.Target;

    public Vector2f Size
        => _camera.Size;

    private UICanvas(InputSystem input)
    {
        Input = input;
    }

    public static UICanvas CreateCanvas(RenderTarget target, InputSystem input)
    {
        UICanvas canvas = new(input);

        canvas.Camera = Camera.CreateUICamera(target);
        
        return canvas;
    }

    public Button AddButton()
        => Button.CreateButton(this);
    public Button AddButton(string textureName)
        => Button.CreateButton(this, textureName);
}