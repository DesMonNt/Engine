using Engine.MathExtensions;

namespace Engine.Primitives;

public class Cube: Object
{
    public float BottomWidth { get; protected set; }
    public float BottomHeight { get; protected set; }
    public float Height { get; protected set; }
    public Cube(Vector3 center, float bottomWidth, float bottomHeight, float height)
    {
        Basis = new Basis(center, new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        BottomWidth = bottomWidth;
        BottomHeight = bottomHeight;
        Height = height;
        
        InitializeVertices();
        InitializeTriangles();
    }

    public Cube(Vector3 center, float side) : this(center, side, side, side) { }

    private void InitializeVertices()
    {
        var bottomWidth = BottomWidth / 2.0f;
        var bottomHeight = BottomHeight / 2.0f;
        var height = Height / 2.0f;
        
        GlobalVertices = new[]
        {
            Basis.ToGlobalBasis(new Vector3(bottomWidth, height, bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(bottomWidth,-height, bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(-bottomWidth, height, bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(-bottomWidth, -height, bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(bottomWidth, height, -bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(bottomWidth, -height, -bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(-bottomWidth, height, -bottomHeight)),
            Basis.ToGlobalBasis(new Vector3(-bottomWidth, -height, -bottomHeight)),
        };

        LocalVertices = GlobalVertices.Select(v => Basis.ToLocalBasis(v)).ToArray();
    }
    
    private void InitializeTriangles() => Triangles = 
        new []
        {
            new Triangle(0, 1, 2),
            new Triangle(1, 3, 2),
            new Triangle(4, 5, 6),
            new Triangle(5, 7, 6),
            new Triangle(0, 2, 4),
            new Triangle(2, 6, 4),
            new Triangle(1, 5, 3),
            new Triangle(5, 7, 3),
            new Triangle(0, 4, 1),
            new Triangle(4, 5, 1),
            new Triangle(2, 3, 6),
            new Triangle(3, 7, 6)
        };
}