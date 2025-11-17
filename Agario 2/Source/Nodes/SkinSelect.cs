using MyEngine.MyInput;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.Nodes.UI;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class SkinSelect : Node
{
    private Action<Color> _startGameAction;

    private SkinPreview _skinPreview;
    private int _currentSkinIndex;
    
    private static readonly Vector2f SkinPreviewAnchor = new(0.5f, 0.5f);
    
    private static readonly Vector2f SelectButtonAnchor = new(0.5f, 1);
    private static readonly Vector2i SelectButtonOffset = new(-64, -74);

    private static readonly Vector2i NextSkinButtonOffset = new(-128, 0);
    private static readonly Vector2f NextSkinButtonAnchor = new(1, 0.5f);

    private static readonly Vector2i PreviousSkinButtonOffset = new(64, 0);
    private static readonly Vector2f PreviousSkinButtonAnchor = new(0, 0.5f);
    
    private int CurrentSkinIndex
    {
        get => _currentSkinIndex;
        set
        {
            if (value < 0)
                value = Colors.Length + (value % Colors.Length);
            
            if (value >= Colors.Length)
                value %= Colors.Length;
            
            _skinPreview.Color = Colors[value];
            _currentSkinIndex = value;
        }
    }
    
    private Color[] Colors
        => EatableCircle.Colors;
    
    private SkinSelect()
    { }
    
    public static SkinSelect Create(RenderTarget target, InputSystem input, Action<Color> startGame)
    {
        SkinSelect result = new();

        UICanvas canvas = UICanvas.CreateCanvas(target, input);

        result.AdoptChild(canvas);
        
        result.InitializeSkinPreview(canvas);
        result.InitializeSelectButton(canvas, startGame);
        result.InitializeDirectionButtons(canvas);
        
        return result;
    }

    private void InitializeSkinPreview(UICanvas canvas)
    {
        _skinPreview = AdoptChild(SkinPreview.CreatePreview(canvas, Colors[_currentSkinIndex]));
        
        _skinPreview.AnchorOnTarget = SkinPreviewAnchor;
        _skinPreview.RenderLayer = RenderLayer.UILayer;
    }

    private void InitializeSelectButton(UICanvas canvas, Action<Color> startGame)
    {
        Button selectButton = canvas.AddButton("select skin button");
        
        selectButton.AnchorOnTarget = SelectButtonAnchor;
        selectButton.Offset = SelectButtonOffset;
        
        _startGameAction = startGame;
        selectButton.OnPressed += StartGame;
    }

    private void InitializeDirectionButtons(UICanvas canvas)
    {
        Button previousSkinButton = canvas.AddButton("previous skin button");
        Button nextSkinButton = canvas.AddButton("next skin button");
        
        previousSkinButton.AnchorOnTarget = PreviousSkinButtonAnchor;
        nextSkinButton.AnchorOnTarget = NextSkinButtonAnchor;
        
        previousSkinButton.Offset = PreviousSkinButtonOffset;
        nextSkinButton.Offset = NextSkinButtonOffset;

        previousSkinButton.OnPressed += Previous;
        nextSkinButton.OnPressed += Next;
    }

    private void StartGame()
        => _startGameAction(Colors[_currentSkinIndex]);

    private void Next()
    {
        CurrentSkinIndex++;
    }

    private void Previous()
    {
        CurrentSkinIndex--;
    }
}