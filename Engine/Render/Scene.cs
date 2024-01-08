using System.Drawing;
using Engine.MathExtensions;
using Engine.Primitives;
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

    public Color[,] GetRenderedFrame(bool isShowPolygonEdges)
    {
        foreach (var obj in Objects)
            RenderObject(obj, isShowPolygonEdges);

        var frame = _rasterizer.FrameBuffer.Buffer;
        
        _rasterizer.FrameBuffer.Clear();
        _rasterizer.ZBuffer.Clear();
        
        return frame;
    }

    private void RenderObject(Object obj, bool isShowPolygonEdges)
    {
        var triangles = obj.Triangles;
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        
        Parallel.ForEach(triangles, parallelOptions, triangle =>
        {
            var v1 = obj.GlobalVertices[triangle.VerticesIndexes[0]];
            var v2 = obj.GlobalVertices[triangle.VerticesIndexes[1]];
            var v3 = obj.GlobalVertices[triangle.VerticesIndexes[2]];
        
            if (!IsVisible(v1, triangle.Normal))
                return;
        
            var p1 = Camera.ScreenProjection(v1);
            var p2 = Camera.ScreenProjection(v2);
            var p3 = Camera.ScreenProjection(v3);
        
            if (p1 is null || p2 is null || p3 is null)
                return;

            
            _rasterizer.ComputePolygon(p1, p2, p3, obj.Color, isShowPolygonEdges);
        });
    }

    private bool IsVisible(Vector3 v1, Vector3 n) => Vector3Functions.Dot(v1 - Camera.Basis.Center, n) < 0;
}