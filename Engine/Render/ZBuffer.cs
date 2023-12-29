using Engine.MathExtensions;

namespace Engine.Render;

public class ZBuffer
{
    private Screen Screen { get; set; }
    private Vector3[,] Buffer { get; set; }

    public ZBuffer(Screen screen)
    {
        Screen = screen;
        Buffer = new Vector3[Screen.Width, Screen.Height];
    }

    public void Add(Vector3 vector)
    {
        var x = (int)vector.X;
        var y = (int)vector.Y;
        
        if(!Screen.IsOnScreen(vector))
            return;
        
        if (Buffer[x, y] is null)
            Buffer[x, y] = vector;
        
        if (Buffer[x, y].Z < vector.Z)
            return;

        Buffer[x, y] = vector;
    }

    public Vector3? Get(int x, int y) 
        => !Screen.IsOnScreen(new Vector3(x, y, 0)) 
            ? null 
            : Buffer[x, y];

    public void Clear() => Buffer = new Vector3[Screen.Width, Screen.Height];
}