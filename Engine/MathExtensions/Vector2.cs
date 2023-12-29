namespace Engine.MathExtensions;

public class Vector2 : IEquatable<Vector2>
{
    public float X { get; }
    public float Y { get; }
    
    public static readonly Vector2 Zero = new Vector2(0, 0);

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 left, Vector2 right)
        => new(left.X + right.X, left.Y + right.Y);
    
    public static Vector2 operator -(Vector2 vec)
        => new (-vec.X, -vec.Y);

    public static Vector2 operator -(Vector2 left, Vector2 right)
        => left + -right;

    public static Vector2 operator *(Vector2 left, float right)
        => new(left.X * right, left.Y * right);

    public static Vector2 operator *(float left, Vector2 right)
        => right * left;

    public static Vector2 operator /(Vector2 left, float right)
        => new(left.X / right, left.Y / right);

    public bool Equals(Vector2 other)
        => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj)
        => obj is Vector2 other && Equals(other);
}