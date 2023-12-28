namespace Engine.Render;

public class Polygon
{
    public Vertex V1 { get; }
    public Vertex V2 { get; }
    public Vertex V3 { get; }
    
    public Polygon(Vertex v1, Vertex v2, Vertex v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }
}
