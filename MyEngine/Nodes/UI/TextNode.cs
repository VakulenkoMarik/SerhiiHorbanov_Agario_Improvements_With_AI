using MyEngine.Nodes.Graphics;
using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.UI;

public sealed class TextNode : RenderedNode
{
    public readonly Text MyText;

    public static Font DefaultFont = new("C:/Windows/Fonts/arial.ttf");

    public string Line
    {
        get => MyText.DisplayedString;
        set => MyText.DisplayedString = value;
    }
    
    public Vector2f Position
    {
        get => MyText.Position;
        set => MyText.Position = value;
    }

    public Vector2f Scale
    {
        get => MyText.Scale;
        set => MyText.Scale = value;
    }

    public Vector2f Origin
    {
        get => MyText.Origin;
        set => MyText.Origin = value;
    }
    
    private TextNode(RenderLayer layer) : base(layer)
    {
        MyText = new();
    }

    public static TextNode CreateTextNode()
    {
        TextNode result = new(RenderLayer.UILayer);

        result.MyText.Font = DefaultFont;
        
        return result;
    }

    public void UpdateLine(string newLine)
        => Line = newLine;
    
    public override void Draw(RenderTarget target, RenderStates states)
        => MyText.Draw(target, states);
}