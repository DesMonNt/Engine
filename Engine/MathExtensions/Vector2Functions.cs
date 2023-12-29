namespace Engine.MathExtensions;

public static class Vector2Functions
{
    public static double DistanceTo(Vector2 a, Vector2 b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }
}