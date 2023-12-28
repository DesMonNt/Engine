using Engine.MathExtensions;

namespace Engine.Render;

public class ZBuffer
{
    public Vector3[,] Buffer { get; private set; }
    public uint Width { get; }
    public uint Height { get; }

    public ZBuffer(uint width, uint height)
    {
        Width = width;
        Height = height;
        Buffer = new Vector3[Width, Height];
    }

    public void Add(Vector3 vector, Vector2 projection)
    {
        var x = (int)Math.Round(projection.X);
        var y = (int)Math.Round(projection.Y);
        
        if (Buffer[x, y] is null)
            Buffer[x, y] = vector;
        
        if (Buffer[x, y].Z < vector.Z)
            return;

        Buffer[x, y] = vector;
    }
    
    public void Clear() => Buffer = new Vector3[Width, Height];
}