using SFML.Graphics;

namespace MyEngine.Nodes.Graphics;

public abstract class RenderedNode : Node, Drawable
{
    public RenderLayer Layer;

    protected RenderedNode(RenderLayer layer)
        => Layer = layer;
    
    public abstract void Draw(RenderTarget target, RenderStates states);
}