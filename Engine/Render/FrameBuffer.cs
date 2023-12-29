using System.Drawing;

namespace Engine.Render;

public class FrameBuffer
{
    public Color[,] Buffer { get; private set; }
    private Screen Screen { get; set; }

    public FrameBuffer(Screen screen)
    {
        Screen = screen;
        Buffer = new Color[Screen.Width, Screen.Height];
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < Screen.Width && y >= 0 && y < Screen.Height)
            Buffer[x, y] = color;
    }

    public void Clear() => Buffer = new Color[Screen.Width, Screen.Height];
}
