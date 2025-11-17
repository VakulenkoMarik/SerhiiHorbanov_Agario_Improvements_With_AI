using MyEngine.Nodes.Graphics;
using MyEngine.Nodes.UI;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class SkinPreview : UINode
{
    private ShapeSprite<CircleShape> _bodyVisuals;
    
    private const float DefaultRadius = 200;

    public Color Color
    {
        get => _bodyVisuals.FillColor; 
        set => _bodyVisuals.FillColor = value; 
    }

    public RenderLayer RenderLayer
    {
        get => _bodyVisuals.Layer; 
        set => _bodyVisuals.Layer = value;
    }

    private ShapeSprite<CircleShape> BodyVisuals
    {
        get => _bodyVisuals;
        set
        {
            if (_bodyVisuals != null)
            {
                DetachChild(_bodyVisuals);
            }
            
            AdoptChild(value);
            _bodyVisuals = value;
        }
    }

    private float Radius
    {
        get => _bodyVisuals.UnderlyingShape.Radius;
        set
        {
            _bodyVisuals.UnderlyingShape.Origin = new(value, value);
            _bodyVisuals.UnderlyingShape.Radius = value;
        }
    }

    private SkinPreview(UICanvas canvas) : base(canvas)
    { }
    
    public static SkinPreview CreatePreview(UICanvas canvas, Color color)
    {
        SkinPreview result = new(canvas);
        
        result.BodyVisuals = ShapeSprite<CircleShape>.CreateCircleSprite();
        result.Radius = DefaultRadius;
        result.Color = color;
        
        return result;
    }
    
    protected override void OnPositionSet()
    {
        _bodyVisuals.Position = (Vector2f)Position + Canvas.Camera.LeftTop;
    }
}