using System.Drawing;
using Object = Engine.Primitives.Object;

namespace Engine.Render;

public class Scene
{
    public Camera Camera { get; }
    public List<Object> Objects { get; }
    private readonly Rasterizer _rasterizer;

    public Scene(Camera camera, List<Object> objects)
    {
        Camera = camera;
        Objects = objects;
        _rasterizer = new Rasterizer(camera);
    }

    public Color[,] GetRenderedFrame()
    {
        foreach (var obj in Objects)
            RenderObject(obj);

        var frame = _rasterizer.FrameBuffer.Buffer;
        _rasterizer.FrameBuffer.Clear();
        _rasterizer.ZBuffer.Clear();
        
        return frame;
    }

    private void RenderObject(Object obj)
    {
        var triangles = obj.Triangles;
        
        foreach (var triangle in triangles)
        {
            var v1 = obj.GlobalVertices[triangle.VerticesIndexes[0]];
            var v2 = obj.GlobalVertices[triangle.VerticesIndexes[1]];
            var v3 = obj.GlobalVertices[triangle.VerticesIndexes[2]];
            
            var p1 = Camera.ScreenProjection(v1);
            var p2 = Camera.ScreenProjection(v2);
            var p3 = Camera.ScreenProjection(v3);
            
            if (p1 is null || p2 is null || p3 is null)
                continue;
            
            _rasterizer.ComputePolygon(p1, p2, p3);
        }
    }
}