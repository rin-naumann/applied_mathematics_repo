using UnityEngine;

public static class Easing
{
    // Fast start, slow finish. Used for the ghost HP bar and the coin counter.
    public static float OutCubic(float t)
    {
        float u = 1f - t;
        return 1f - u * u * u;
    }

    // Slow start, fast finish. Used for the coin flying to the UI.
    public static float InQuad(float t) => t * t;
}