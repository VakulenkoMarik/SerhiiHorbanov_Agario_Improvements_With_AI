using SFML.Graphics;

namespace MyEngine.Nodes.Graphics;

public readonly record struct Subtexture(Texture Texture, IntRect TextureRect)
{
    public void ApplyTo(Sprite sprite)
    {
        sprite.Texture = Texture;
        sprite.TextureRect = TextureRect;
    }
}