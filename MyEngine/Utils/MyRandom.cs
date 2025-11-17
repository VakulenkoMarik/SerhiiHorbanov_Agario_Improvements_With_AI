namespace MyEngine.Utils;

public static class MyRandom
{
    public static float NextFloat(float multiplier)
        => Random.Shared.NextSingle() * multiplier;
    
    public static T GetRandomElement<T>(this List<T> list)
        => list[Random.Shared.Next(list.Count)];

    public static T GetRandomElement<T>(this T[] list)
        => list[Random.Shared.Next(list.Length)];
    
    public static float GetFloatInRange(float min, float max)
        => Random.Shared.NextSingle() * (max - min) + min;

    public static float GetFloatInDistance(float avg, float maxDist)
        => GetFloatInRange(avg - maxDist, avg + maxDist);
}