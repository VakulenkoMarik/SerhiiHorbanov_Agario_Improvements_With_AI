using MyEngine.ResourceLibraries;
using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public class SpriteNode : RenderedNode
{
    public Sprite Sprite;

    public Texture Texture
    {
        get => Sprite.Texture;
        set => Sprite.Texture = value;
    }

    public Vector2f Position
    {
        get => Sprite.Position;
        set => Sprite.Position = value;
    }

    public Vector2f Scale
    {
        get => Sprite.Scale;
        set => Sprite.Scale = value;
    }

    public Vector2f Origin
    {
        get => Sprite.Origin;
        set => Sprite.Origin = value;
    }
    
    private SpriteNode(RenderLayer layer, Sprite sprite) : base(layer)
    {
        Sprite = sprite;
    }

    public static SpriteNode CreateSprite(RenderLayer layer, string textureName)
        => new(layer, new(TextureLibrary.GetTexture(textureName)));
    
    public static SpriteNode CreateSprite(RenderLayer layer, Sprite sprite)
        => new(layer, sprite);
    
    public static SpriteNode CreateSprite(RenderLayer layer)
        => CreateSprite(layer, new Sprite());

    public override void Draw(RenderTarget target, RenderStates states)
        => Sprite.Draw(target, states);
}