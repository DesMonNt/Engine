using Engine.MathExtensions;

namespace Engine.Render; 

public struct Vertex
{
    public Vector3 Position { get; }
    public Vector2 TextureCoordinates { get; }
    public Vector3 Normal { get; }

    public Vertex(Vector3 position, Vector2 textureCoordinates, Vector3 normal)
    {
        Position = position;
        TextureCoordinates = textureCoordinates;
        Normal = normal;
    }

    public override int GetHashCode() => Position.GetHashCode();
}