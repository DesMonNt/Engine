using Engine.MathExtensions;

namespace Engine.Primitives;

public class Basis
{
    public Vector3 Center { get; private set; }
    public Vector3 XAxis { get; private set; }
    public Vector3 YAxis { get; private set; }
    public Vector3 ZAxis { get; private set; }

    private Matrix3X3 LocalCoordsMatrix => new(
        XAxis.X, YAxis.X, ZAxis.X,
        XAxis.Y, YAxis.Y, ZAxis.Y,
        XAxis.Z, YAxis.Z, ZAxis.Z
    );

    private Matrix3X3 GlobalCoordsMatrix => new(
        XAxis.X , XAxis.Y , XAxis.Z,
        YAxis.X , YAxis.Y , YAxis.Z,
        ZAxis.X , ZAxis.Y , ZAxis.Z
    );

    public Basis(Vector3 center, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
    {
        Center = center;
        XAxis = xAxis;
        YAxis = yAxis;
        ZAxis = zAxis;
    }

    public Vector3 ToLocalBasis(Vector3 global) => LocalCoordsMatrix * (global - Center);

    public Vector3 ToGlobalBasis(Vector3 local) => GlobalCoordsMatrix * local + Center;

    public void Move(Vector3 v) => Center += v;

    public void Rotate(float angle, Axis axis)
    {
        XAxis = XAxis.Rotate(angle, axis);
        YAxis = YAxis.Rotate(angle, axis);
        ZAxis = ZAxis.Rotate(angle, axis);
    }
}