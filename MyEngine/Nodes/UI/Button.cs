using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes.Graphics;
using MyEngine.ResourceLibraries;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MyEngine.Nodes.UI;

public class Button : UINode, IDisposable
{
    private readonly InputListener _inputListener;
    private SpriteNode _sprite;

    private const Mouse.Button PressButton = Mouse.Button.Left;

    public Action OnPressed;
    public Action OnReleased;
    
    private bool IsPressed { get; set; }

    private SpriteNode Sprite
    {
        get => _sprite;
        set => _sprite = AdoptChild(value);
    }
    
    private FloatRect PressableArea 
        => _sprite.Sprite.GetGlobalBounds();

    private Button(UICanvas canvas) : base(canvas)
    {
        _inputListener = new();
    }

    protected override void OnPositionSet()
    {
        Sprite.Position = (Vector2f)Position;
    }

    private void OnMouseClicked()
    {
        Camera camera = Canvas.Camera;
        Vector2i mousePositionOnView = camera.CalculatePositionOnView((Vector2i)MouseInput.MousePositionOnWindow);
        Vector2i position = mousePositionOnView + (Vector2i)camera.LeftTop;
        
        if (PressableArea.Contains(position))
        {
            IsPressed = true;
            OnPressed?.Invoke();
        }
    }

    private void OnMouseReleased()
    {
        if (IsPressed)
            OnReleased?.Invoke();
        
        IsPressed = false;
    }
    
    public static Button CreateButton(UICanvas canvas)
    {
        Button button = new(canvas);
        canvas.AdoptChild(button);

        canvas.Input.AddListener(button._inputListener);
        
        ClickBind bind = button._inputListener.AddAction(new ClickBind("button press", PressButton));
        bind.AddOnStartedCallback(button.OnMouseClicked);
        bind.AddOnEndedCallback(button.OnMouseReleased);
        
        button.Sprite = SpriteNode.CreateSprite(RenderLayer.UILayer);

        return button;
    }

    public static Button CreateButton(UICanvas canvas, string textureName)
    {
        Button result = CreateButton(canvas);
        
        result.Sprite.Texture = TextureLibrary.GetTexture(textureName);
        
        return result;
    }

    public void Dispose()
    {
        ClickBind bind = _inputListener.GetAction<ClickBind>("button press");
        bind.ResetCallbacks();
        
        _inputListener.Dispose();
    }
}