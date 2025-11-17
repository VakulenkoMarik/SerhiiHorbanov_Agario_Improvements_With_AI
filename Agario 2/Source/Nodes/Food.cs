using MyEngine.Nodes;
using SFML.System;

namespace Agario_2.Nodes;

public class Food : Node
{
    private EatableCircle _eatable;
    private const float Radius = 10;

    public Action OnEaten
    {
        get => _eatable.OnEaten;
        set => _eatable.OnEaten = value;
    }

    public Vector2f Position
    {
        get => _eatable.Position;
        set => _eatable.Position = value;
    }

    // just after being created food DOES NOT have any OnEaten action. it has to be added from outside
    public static Food CreateFood(Vector2f position)
    {
        Food result = new();

        result._eatable = EatableCircle.CreateEatableCircle(Radius, position);
        result.AdoptChild(result._eatable);
        
        return result;
    }
}