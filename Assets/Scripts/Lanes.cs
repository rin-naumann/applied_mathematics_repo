public static class Lanes
{
    public const int Count = 3;
    public static float spacing = 2.5f;
    public static float groundY = -3f;

    // lane 0, 1, 2  ->  -spacing, 0, +spacing
    public static float GetX(int lane)
    {
        return (lane - (Count - 1) * 0.5f) * spacing;
    }
}