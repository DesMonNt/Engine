﻿using Engine.MathExtensions;

namespace Engine.Primitives;

public class Triangle
{
    public readonly int[] VerticesIndexes;
    public Vector3 Normal { get; set; }

    public Triangle(int vertexIndex1, int vertexIndex2, int vertexIndex3) 
        => VerticesIndexes = new[] { vertexIndex1, vertexIndex2, vertexIndex3 };
}