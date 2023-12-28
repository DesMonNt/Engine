using System.Drawing;
using Engine.MathExtensions;

namespace Engine.Render;

public class Rasterizer
{
    public Camera Camera { get; }
    public ZBuffer ZBuffer { get; }
    public FrameBuffer FrameBuffer { get; }

    public Rasterizer(Camera camera)
    {
        Camera = camera;
        ZBuffer = new ZBuffer(camera.Screen.Width, camera.Screen.Height);
        FrameBuffer = new FrameBuffer(camera.Screen);
    }

    public void ComputePolygon(Vector3 p1 , Vector3 p2, Vector3 p3)
    {
        var c1 = Color.Gray;
        var c2 = Color.Gray;
        var c3 = Color.Gray;
        
        RasterizeTriangle(p1, p2, p3, Color.White);
        RasterizeLine(p1, p2, c1, c2);
        RasterizeLine(p2, p3, c2, c3);
        RasterizeLine(p3, p1, c3, c1);
    }

    private void RasterizeLine(Vector3 p1 , Vector3 p2, Color c1, Color c2)
    {
        var (x1, y1) = RoundCoordinates(p1);
        var (x2, y2) = RoundCoordinates(p2);

        var dx = Math.Abs(x2 - x1);
        var dy = Math.Abs(y2 - y1);
        
        var t = (float)(x1 - Math.Round(p1.X) + y1 - Math.Round(p1.Y)) / Math.Abs(dx + dy);
        
        var sx = x1 < x2 ? 1 : -1;
        var sy = y1 < y2 ? 1 : -1;

        var err = dx - dy;

        while (x1 != x2 || y1 != y2)
        {
            var color = InterpolateColor(c1, c2, t);
            var z = InterpolateZ((float)p1.Z, (float)p2.Z, t);
            
            ZBuffer.Add(new Vector3(x1, y1, z), new Vector2(x1, y1));
            
            if (Math.Abs(ZBuffer.Buffer[x1, y1].Z - z) < 1e-9)
                FrameBuffer.SetPixel(x1, y1, color);

            var err2 = 2 * err;

            if (err2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }

            if (err2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }

    private void RasterizeTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color color)
    {
        var (bottomLeft, topRight) = FindTriangleBoundingRectangle(p1, p2, p3);
        var leftX = (int)bottomLeft.X;
        var rightX = (int)topRight.X;
        var bottomY = (int)bottomLeft.Y;
        var topY = (int)topRight.Y;
        
        var (x1, y1) = RoundCoordinates(p1);
        var (x2, y2) = RoundCoordinates(p2);
        
        var dx = Math.Abs(x2 - x1);
        var dy = Math.Abs(y2 - y1);
        
        var t = (float)(x1 - Math.Round(p1.X) + y1 - Math.Round(p1.Y)) / Math.Abs(dx + dy);

        for (var x = leftX; x < rightX; x++)
        {
            for (var y = bottomY; y < topY; y++)
            {
                var point = new Vector2(x, y);

                if (!IsInTriangle(p1, p2, p3, point))
                    continue;
                
                var z1 = InterpolateZ((float)p1.Z, (float)p2.Z, t);
                var z2 = InterpolateZ((float)p2.Z, (float)p3.Z, t);
                var z3 = InterpolateZ((float)p3.Z, (float)p1.Z, t);
                
                var z = Math.Min(z1, Math.Min(z2, z3));
            
                ZBuffer.Add(new Vector3(x, y, z), new Vector2(x, y));
            
                if (ZBuffer.Buffer[x, y].Z == z)
                    FrameBuffer.SetPixel(x, y, color);
            }
        }
    }

    private (Vector2 bottomLeft, Vector2 topRight) FindTriangleBoundingRectangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var leftX = (int)Math.Round(Math.Min(p1.X, Math.Min(p2.X, p3.X)));
        var rightX = (int)Math.Round(Math.Max(p1.X, Math.Max(p2.X, p3.X)));
        var bottomY = (int)Math.Round(Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
        var topY = (int)Math.Round(Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

        return (new Vector2(leftX, bottomY), new Vector2(rightX, topY));
    }

    private bool IsInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 point)
    {
        var alpha = LineEquation(p1, p2, point);
        var beta = LineEquation(p2, p3, point);
        var gamma = LineEquation(p3, p1, point);

        return (alpha < 0 && beta < 0 && gamma < 0)
            || (alpha > 0 && beta > 0 && gamma > 0);
    }

    private double LineEquation(Vector3 p1, Vector3 p2, Vector2 point)
    {
        var (x1, y1) = (p1.X, p1.Y);
        var (x2, y2) = (p2.X, p2.Y); 
        var (x, y) = (point.X, point.Y); 

        return x * (y1 - y2) + y * (x2 - x1) + (x1 * y2 - x2 * y1);
    }

    private (int x, int y) RoundCoordinates(Vector3 vector)
    {
        var x = (int)Math.Round(vector.X);
        var y = (int)Math.Round(vector.Y);
        
        return (x, y);
    }
    
    private Color InterpolateColor(Color color1, Color color2, float t)
    {
        var r = (byte)(color1.R + (color2.R - color1.R) * t);
        var g = (byte)(color1.G + (color2.G - color1.G) * t);
        var b = (byte)(color1.B + (color2.B - color1.B) * t);

        return Color.FromArgb(r, g, b);
    }
    
    private float InterpolateZ(float z1, float z2, float t) => z1 + t * (z2 - z1);
}