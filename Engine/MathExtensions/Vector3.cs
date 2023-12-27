namespace Engine.MathExtensions;

public class Vector3 : IEquatable<Vector3>
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public static readonly Vector3 Zero = new Vector3(0, 0, 0);
    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
        => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    
    public static Vector3 operator -(Vector3 vec)
        => new (-vec.X, -vec.Y, -vec.Z);

    public static Vector3 operator -(Vector3 left, Vector3 right)
        => left + -right;

    public static Vector3 operator *(Vector3 left, double right)
        => new(left.X * right, left.Y * right, left.Z * right);

    public static Vector3 operator *(double left, Vector3 right)
        => right * left;

    public static Vector3 operator *(Matrix3X3 left, Vector3 right)
    {
        var x = left[0, 0] * right.X + left[0, 1] * right.Y + left[0, 2] * right.Z;
        var y = left[1, 0] * right.X + left[1, 1] * right.Y + left[1, 2] * right.Z;
        var z = left[2, 0] * right.X + left[2, 1] * right.Y + left[2, 2] * right.Z;

        return new Vector3(x, y, z);
    }
    
    public static Vector3 operator /(Vector3 left, double right)
        => new(left.X / right, left.Y / right, left.Z / right);

    public bool Equals(Vector3 other)
        => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

    public override bool Equals(object? obj)
        => obj is Vector3 other && Equals(other);

    public double Length() => Vector3Functions.Length(this);

    public Vector3 Normalize() => Vector3Functions.Normalize(this);

    public Vector3 Rotate(double angle, Axis axis) => Vector3Functions.Rotate(this, angle, axis);
}