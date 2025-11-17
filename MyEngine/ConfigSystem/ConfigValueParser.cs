using MyEngine.Utils;
using SFML.System;

namespace MyEngine.ConfigSystem;

public class ConfigValueParser
{
    public virtual object ParseFieldValue(Type parseTo, string value)
    {
        if (parseTo == typeof(int))
            return int.Parse(value);
        if (parseTo == typeof(float))
            return float.Parse(value);
        if (parseTo == typeof(double))
            return double.Parse(value);
        if (parseTo == typeof(bool))
            return bool.Parse(value);
        if (parseTo == typeof(string))
            return value.TrimFirstLast('"');
        if (parseTo == typeof(Vector2f))
            return ParseVector2F(value);
        if (parseTo == typeof(Vector2i))
            return ParseVector2I(value);
        if (parseTo == typeof(Vector2u))
            return ParseVector2U(value);
        
        throw new Exception($"can't parse \"{value}\" to {parseTo.FullName}");
    }

    protected Vector2f ParseVector2F(string value)
    {
        value = value.TrimBrackets();
        
        float x = float.Parse(value.CutOffBefore(','));
        float y = float.Parse(value.CutOffAfter(',')); 
        
        return new(x, y);
    }
    
    protected Vector2i ParseVector2I(string value)
    {
        value = value.TrimBrackets();
        
        int x = int.Parse(value.CutOffBefore(','));
        int y = int.Parse(value.CutOffAfter(',')); 
        
        return new(x, y);
    }
    
    protected Vector2u ParseVector2U(string value)
    {
        value = value.TrimBrackets();
        
        uint x = uint.Parse(value.CutOffBefore(','));
        uint y = uint.Parse(value.CutOffAfter(','));
        
        return new(x, y);
    }
}