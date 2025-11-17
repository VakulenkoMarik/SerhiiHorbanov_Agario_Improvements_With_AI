using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public class Camera : Node
{
    private View _view;
    public RenderTarget Target;
    public RenderLayer RenderedLayer;
    
    public Vector2f Position
    {
        get => _view.Center;
        set => _view.Center = value;
    }

    public Vector2f LeftTop
    {
        get => _view.Center - HalfSize;
        set => _view.Center = value + HalfSize;
    }
    
    public Vector2f Size
    {
        get => _view.Size;
        set => _view.Size = value;
    }

    private Vector2f HalfSize
        => _view.Size / 2;
    
    private Camera(RenderLayer renderedLayer)
    {
        RenderedLayer = renderedLayer;
        _view = new();
    }

    public static Camera CreateCamera(RenderTarget target, RenderLayer layer)
    {
        Camera result = new(layer);

        result.Target = target;
        
        return result;
    }
    
    public static Camera CreateCamera(RenderTarget target)
        => CreateCamera(target, RenderLayer.NormalLayer);
    public static Camera CreateUICamera(RenderTarget target)
        => CreateCamera(target, RenderLayer.UILayer);
    
    public void Render(List<RenderedNode> renderedNodes)
    {
        ApplyView();
        
        foreach (RenderedNode renderedNode in renderedNodes)
            renderedNode.Draw(Target, RenderStates.Default);
    }
    
    private void AddToRenderQueue(Node node, Queue<RenderedNode> queue)
    {
        if (node is RenderedNode rendered)
            if (rendered.Layer == RenderedLayer)
                queue.Enqueue(rendered);
        
        foreach (Node child in node)
            AddToRenderQueue(child, queue);
    }

    // DOES NOT support rotated views
    public Vector2i CalculatePositionOnView(Vector2i positionOnTarget)
    {
        Vector2f inverseSize = new(1f / Target.Size.X, 1f / Target.Size.Y);

        Vector2f proportionalPositionOnTarget = ((Vector2f)positionOnTarget).Scale(inverseSize);
        
        return (Vector2i)proportionalPositionOnTarget.Scale(Size);
    }
    
    private void ApplyView()
        => Target.SetView(_view);
}