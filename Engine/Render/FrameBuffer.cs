using System.Drawing;

namespace Engine.Render;

public class FrameBuffer
{
    public Color[,] Buffer { get; private set; }

    public int Width { get; }
    public int Height { get; }

    public FrameBuffer(Screen screen)
    {
        Width = (int)screen.Width;
        Height = (int)screen.Height;
        Buffer = new Color[Width, Height];
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            Buffer[x, y] = color;
    }

    public void Clear() => Buffer = new Color[Width, Height];
}
