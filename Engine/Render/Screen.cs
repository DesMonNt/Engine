using Engine.MathExtensions;

namespace Engine.Render;

public class Screen
{
    public uint Width { get; private set; }
    public uint Height { get; private set; }

    public Screen(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

    public void UpdateResolution(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

    public bool IsOnScreen(Vector3 vector)
        => vector.X >= 0 && vector.X < Width && vector.Y >= 0 && vector.Y < Height;
}