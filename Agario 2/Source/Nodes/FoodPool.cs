using MyEngine.Nodes;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;

namespace Agario_2.Nodes;

public class FoodPool : Node
{
    private FloatRect mapBounds;
    
    private int _targetTargetFoodAmount;

    public FoodPool(FloatRect mapBounds)
    {
        this.mapBounds = mapBounds;
    }

    public int TargetFoodAmount
    {
        get => _targetTargetFoodAmount;
        set
        {
            if (_targetTargetFoodAmount < value)
                AddFood(value - _targetTargetFoodAmount);
            else
                _targetTargetFoodAmount = value;
        }
    }

    public static FoodPool CreateFoodPool(int foodAmount, FloatRect mapBounds)
    {
        FoodPool result = new(mapBounds);

        result.TargetFoodAmount = foodAmount;

        return result;
    }
    
    private void AddFood(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddFood();
    }
    
    private void AddFood()
    {
        Vector2f position = GetRandomFoodPosition();
        Food food = Food.CreateFood(position);
        food.OnEaten += delegate { RegenerateFood(food); };
        
        AdoptChild(food);
    }

    private Vector2f GetRandomFoodPosition()
        => mapBounds.RandomPositionInside();

    private void RegenerateFood(Food food)
        => food.Position = GetRandomFoodPosition();
}