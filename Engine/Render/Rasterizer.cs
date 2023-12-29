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
        ZBuffer = new ZBuffer(camera.Screen);
        FrameBuffer = new FrameBuffer(camera.Screen);
    }

    public void ComputePolygon(Vector3 p1 , Vector3 p2, Vector3 p3, Color color, bool isShowPolygonEdges)
    {
        RasterizeTriangle(p1, p2, p3, color);
        
        if (!isShowPolygonEdges)
            return;
        
        var c = Color.Gray;
        
        RasterizeLine(p1, p2, c);
        RasterizeLine(p2, p3, c);
        RasterizeLine(p3, p1, c);
    }

    private void RasterizeLine(Vector3 p1 , Vector3 p2, Color color)
    {
        var (x1, y1) = RoundCoordinates(p1);
        var (x2, y2) = RoundCoordinates(p2);

        var dx = Math.Abs(x2 - x1);
        var dy = Math.Abs(y2 - y1);
        var sx = x1 < x2 ? 1 : -1;
        var sy = y1 < y2 ? 1 : -1;
        
        var err = dx - dy;

        while (x1 != x2 || y1 != y2)
        { 
            var z1 = InterpolateZ(p1, p2, new Vector2(x1, y1));
            var pixel = new Vector3(x1, y1, z1);
            
            FillPixel(pixel, color);

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
        
        for (var x = leftX; x < rightX; x++)
        {
            for (var y = bottomY; y < topY; y++)
            {
                var point = new Vector2(x, y);

                if (!IsInTriangle(p1, p2, p3, point))
                    continue;
                
                var z = InterpolateZ(p1, p2, p3, point);
                var pixel = new Vector3(x, y, z);
            
                FillPixel(pixel, color);
            }
        }
    }

    private static (Vector2 bottomLeft, Vector2 topRight) FindTriangleBoundingRectangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var leftX = (int)Math.Round(Math.Min(p1.X, Math.Min(p2.X, p3.X)));
        var rightX = (int)Math.Round(Math.Max(p1.X, Math.Max(p2.X, p3.X)));
        var bottomY = (int)Math.Round(Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
        var topY = (int)Math.Round(Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

        return (new Vector2(leftX, bottomY), new Vector2(rightX, topY));
    }

    private static bool IsInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 point)
    {
        var alpha = LineEquation(p1, p2, point);
        var beta = LineEquation(p2, p3, point);
        var gamma = LineEquation(p3, p1, point);

        return (alpha < 0 && beta < 0 && gamma < 0)
            || (alpha > 0 && beta > 0 && gamma > 0);
    }

    private static double LineEquation(Vector3 p1, Vector3 p2, Vector2 point)
    {
        var (x1, y1) = (p1.X, p1.Y);
        var (x2, y2) = (p2.X, p2.Y); 
        var (x, y) = (point.X, point.Y); 

        return x * (y1 - y2) + y * (x2 - x1) + (x1 * y2 - x2 * y1);
    }

    private static (int x, int y) RoundCoordinates(Vector3 vector)
    {
        var x = (int)Math.Round(vector.X);
        var y = (int)Math.Round(vector.Y);
        
        return (x, y);
    }
    
    private static float InterpolateZ(Vector3 p1, Vector3 p2, Vector2 point)
    {
        var t = Vector2Functions.DistanceTo(point, new Vector2(p1.X, p1.Y)) 
                / Vector2Functions.DistanceTo(new Vector2(p2.X, p2.Y), new Vector2(p1.X, p1.Y));
        var z = p1.Z + t * (p2.Z - p1.Z);
        
        return (float)z;
    }
    private static float InterpolateZ(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 point)
    {
        var detT = (p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y);
        var u = ((p2.Y - p3.Y) * (point.X - p3.X) + (p3.X - p2.X) * (point.Y - p3.Y)) / detT;
        var v = ((p3.Y - p1.Y) * (point.X - p3.X) + (p1.X - p3.X) * (point.Y - p3.Y)) / detT;
        var w = 1 - u - v;

        var barycentricCoord = new Vector3(u, v, w);
        var z = barycentricCoord.X * p1.Z + barycentricCoord.Y * p2.Z + barycentricCoord.Z * p3.Z;
        
        return z;
    }
    private void FillPixel(Vector3 vector, Color color)
    {
        var (x, y) = RoundCoordinates(vector);

        ZBuffer.Add(vector);

        var buffer = ZBuffer.Get(x, y);
        
        if (buffer is null)
            return;
        
        if (buffer.Equals(vector))
            FrameBuffer.SetPixel(x, y, color);
    }
}