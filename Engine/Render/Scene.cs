using Object = Engine.Primitives.Object;

namespace Engine.Render;

public class Scene
{
    public List<Object> Objects { get; }

    public Scene(List<Object> objects) => Objects = objects;
}