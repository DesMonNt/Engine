using Engine.MathExtensions;

namespace Engine.Primitives;

public class Cube: Object
{
    public Cube(Vector3 center, float sideLen)
    {
        Basis = new Basis(center, new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        var delta = new[] { -sideLen / 2, sideLen / 2 };
        GlobalVertices = delta.SelectMany(n => delta.SelectMany(n1 => delta.Select(n2 => center + new Vector3(n, n1, n2)))).ToArray();
        LocalVertices = GlobalVertices.Select(v => Basis.ToLocalCoords(v)).ToArray();
        
        Indexes = new[]
        {
            1,3,2 , 
            1,0,2 , 
            5,7,6 ,
            5,4,6 ,
            1,0,4 ,
            1,5,4 ,
            3,2,6 ,
            3,7,6 ,
            1,3,7 ,
            1,5,7 ,
            0,2,6 ,
            0,4,6
        };
    }
}