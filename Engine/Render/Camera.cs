using Engine.MathExtensions;
using Engine.Primitives;

namespace Engine.Render;

public class Camera
{
    public Basis Basis { get;}
    public Screen Screen { get; }
    public double DistanceToScreen { get;} 
    public double Fov { get;}
    private double Scale => Screen.Width / (2 * DistanceToScreen * Math.Tan(Fov / 2));

    public Camera(Vector3 center, Screen screen, double distanceToScreen, double fov)
    {
        Basis = new Basis(center, new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        Screen = screen;
        DistanceToScreen = distanceToScreen;
        Fov = fov;
    }
    
    public void Move(Vector3 vector) => Basis.Move(vector);
    
    public void Rotate(float angle, Axis axis) => Basis.Rotate(angle, axis);
    
    public Vector3 ScreenProjection(Vector3 vector)
    {
        var vectorInCameraBasis = Basis.ToLocalCoords(vector);
        
        if (vectorInCameraBasis.Z <= DistanceToScreen)
            return new Vector3(float.NaN, float.NaN, 0);
        
        var projection = GetProjection(vectorInCameraBasis);
        var vectorInScreenBasis = new Vector3(projection.X + Screen.Width / 2, Screen.Height / 2 - projection.Y, 0);

        return Screen.IsOnScreen(vectorInScreenBasis) 
            ? vectorInScreenBasis 
            : new Vector3(float.NaN, float.NaN, 0);
    }

    private Vector3 GetProjection(Vector3 vector)
    {
        var delta = DistanceToScreen / vector.Z * Scale;
        var projectionOnScreen = new Vector3(vector.X, vector.Y, 0) * delta;

        return projectionOnScreen;
    }
}