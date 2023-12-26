using Engine.MathExtensions;

namespace Engine.Primitives;

public abstract class Object
{
    public Basis Basis { get; protected init; }
    public Vector3[] LocalVertices { get; protected init; }
    public Vector3[] GlobalVertices { get; protected init; }
    public int[] Indexes { get; protected set; }

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
}