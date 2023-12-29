using Engine.MathExtensions;
using Engine.Primitives;
using Object = Engine.Primitives.Object;

namespace Engine.Render;

public class Camera: Object
{
    public Screen Screen { get; }
    public double ScreenDistance { get;} 
    public double RenderDistance { get; }
    public double Fov { get;}
    private double FovScale => Screen.Width / (2 * ScreenDistance * Math.Tan(Fov / 2));

    public Camera(Vector3 center, Screen screen, double screenDistance, double renderDistance, double fov)
    {
        Basis = new Basis(center, new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        Screen = screen;
        ScreenDistance = screenDistance;
        RenderDistance = renderDistance;
        Fov = fov;
    }
    
    public new void Move(Vector3 vector) => Basis.Move(vector);
    
    public new void Rotate(float angle, Axis axis) => Basis.Rotate(angle, axis);
    
    public Vector3? ScreenProjection(Vector3 vector)
    {
        var vectorInCameraBasis = Basis.ToLocalBasis(vector);
        
        if (vectorInCameraBasis.Z <= ScreenDistance || vectorInCameraBasis.Z >= RenderDistance)
            return null;
        
        var projection = GetProjection(vectorInCameraBasis);
        var vectorInScreenBasis = new Vector3(projection.X + Screen.Width / 2, Screen.Height / 2 - projection.Y, vectorInCameraBasis.Z);

        return Screen.IsOnScreen(vectorInScreenBasis) 
            ? vectorInScreenBasis 
            : null;
    }

    private Vector2 GetProjection(Vector3 vector)
    {
        var delta = (float)(ScreenDistance / vector.Z * FovScale);
        var projectionOnScreen = new Vector2(vector.X, vector.Y) * delta;

        return projectionOnScreen;
    }
}