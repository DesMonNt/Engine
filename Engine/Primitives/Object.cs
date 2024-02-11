using System.Drawing;
using Engine.MathExtensions;

namespace Engine.Primitives;

public abstract class Object
{
    public Basis Basis { get; protected init; }
    public Vector3[] LocalVertices { get; protected set; }
    public Vector3[] GlobalVertices { get; protected set; }
    public Triangle[] Triangles { get; protected set; }
    public Color Color { get; protected set; }

    public void Move(Vector3 v)
    {
        Basis.Move(v);

        for (var i = 0; i < LocalVertices.Length; i++)
            GlobalVertices[i] += v;
    }

    public void Rotate(float angle, Axis axis)
    {
        Basis.Rotate(angle, axis);

        for (var i = 0; i < LocalVertices.Length; i++)
            GlobalVertices[i] = Basis.ToGlobalBasis(LocalVertices[i]);
    }

    public void Scale(float k)
    {
        for (var i = 0; i < LocalVertices.Length; i++)
            LocalVertices[i] *= k;

        for (var i = 0; i < LocalVertices.Length; i++)
            GlobalVertices[i] = Basis.ToGlobalBasis(LocalVertices[i]);
    }

    protected void CalculateNormal(Triangle triangle)
    {
        var v1 = GlobalVertices[triangle.VerticesIndexes[0]];
        var v2 = GlobalVertices[triangle.VerticesIndexes[1]];
        var v3 = GlobalVertices[triangle.VerticesIndexes[2]];
        
        var normal = Vector3Functions.Cross(v2 - v1, v3 - v1);
        
        if (Vector3Functions.Dot(v1 - Basis.Center, normal) < 0)
            normal = -normal;

        triangle.Normal = Basis.ToLocalBasis(normal.Normalize());
    }
}