namespace Engine.MathExtensions;

public enum Axis
{
    X, Y, Z
}
public static class Vector3Functions
{
    public static double Length(Vector3 vector)
        => Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
    
    public static Vector3 Normalize(Vector3 vector)
    {
        var len = Length(vector);

        return len == 0
            ? new Vector3(0, 0, 0)
            : new Vector3(vector.X / len, vector.Y / len, vector.Z / len);
    }
    public static double Dot(Vector3 a, Vector3 b)
        => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    
    public static Vector3 Cross(Vector3 a, Vector3 b)
        => new(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - b.Z * a.X, a.X * b.Y - b.X * a.Y);
    
    public static double TripleProduct(Vector3 a, Vector3 b, Vector3 c)
        => Dot(a, Cross(b, c));
    
    public static Vector3 ProjectOnto(Vector3 vector, Vector3 ontoVector)
    {
        var projection = Dot(vector,ontoVector) / Length(ontoVector);

        return Normalize(ontoVector) * projection;
    }
    
    public static double DistanceTo(Vector3 a, Vector3 b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        var dz = a.Z - b.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static double Angle(Vector3 a, Vector3 b)
        => Math.Acos(Dot(a, b) / (Length(a) * Length(b)));
    
    public static Vector3 Reflect(Vector3 a, Vector3 b)
    {
        var dot = Dot(a, b);
        var reflection = a - 2 * dot * b;
        
        return reflection;
    }

    public static Vector3 Rotate(Vector3 vector, double angle, Axis axis)
    {
        var cosTheta = Math.Cos(angle);
        var sinTheta = Math.Sin(angle);

        var rotationMatrix = axis switch
        {
            Axis.X => new Matrix3X3(
                new[,] { { 1, 0, 0 }, { 0, cosTheta, -sinTheta }, { 0, sinTheta, cosTheta } }),
            Axis.Y => new Matrix3X3(
                new[,] { { cosTheta, 0, sinTheta }, { 0, 1, 0 }, { -sinTheta, 0, cosTheta } }),
            Axis.Z => new Matrix3X3(
                new[,] { { cosTheta, -sinTheta, 0 }, { sinTheta, cosTheta, 0 }, { 0, 0, 1 } }),
            
            _ => throw new ArgumentException("Invalid axis specified.")
        };

        return rotationMatrix * vector;
    }
}