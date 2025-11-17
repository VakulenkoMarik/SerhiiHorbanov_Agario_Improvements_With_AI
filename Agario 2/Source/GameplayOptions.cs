using SFML.Graphics;

namespace Agario_2;

public record GameplayOptions(Color PlayerSkin)
{
    public GameplayOptions() : this(Color.White)
    { }
}