using UnityEngine;

// Helper class to get the screen bounds in world units. Used to clamp player and upgrade spawn positions within the screen.
public static class ScreenBounds
{
    public static float HalfHeight => Camera.main.orthographicSize;
    public static float HalfWidth => HalfHeight * Screen.width / Screen.height;
}
