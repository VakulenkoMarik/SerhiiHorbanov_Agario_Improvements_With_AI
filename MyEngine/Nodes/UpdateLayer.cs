namespace MyEngine.Nodes;

public record struct UpdateLayer(uint Value)
{
    public static readonly UpdateLayer NeverUpdatingLayer = 0;
    public static readonly UpdateLayer Normal = 1;
    public static readonly UpdateLayer UI = 2;
    public static readonly UpdateLayer Animations = 3;
    
    public bool IsNeverUpdatingLayer
        => Value == NeverUpdatingLayer;
    
    public static implicit operator UpdateLayer(uint layer)
        => new(layer);
}