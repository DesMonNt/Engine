﻿using System.Drawing;
using Engine.MathExtensions;

namespace Engine.Primitives
{
    public class Pyramid : Object
    {
        public float BottomWidth { get; protected set; }
        public float BottomHeight { get; protected set; }
        public float Height { get; protected set; }

        public Pyramid(Vector3 center, float bottomWidth, float bottomHeight, float height, Color color)
        {
            Basis = new Basis(center, new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
            BottomWidth = bottomWidth;
            BottomHeight = bottomHeight;
            Height = height;
            Color = color;
            
            InitializeVertices();
            InitializeTriangles();
            
            foreach (var triangle in Triangles)
                CalculateNormal(triangle);
        }

        private void InitializeVertices()
        {
            var bottomWidth = BottomWidth / 2.0f;
            var bottomHeight = BottomHeight / 2.0f;
            var height = Height / 2.0f;

            GlobalVertices = new[]
            {
                Basis.ToGlobalBasis(new Vector3(0, height, 0)),
                Basis.ToGlobalBasis(new Vector3(bottomWidth, -height, bottomHeight)),
                Basis.ToGlobalBasis(new Vector3(bottomWidth, -height, -bottomHeight)),
                Basis.ToGlobalBasis(new Vector3(-bottomWidth, -height, -bottomHeight)),
                Basis.ToGlobalBasis(new Vector3(-bottomWidth, -height, bottomHeight)),
            };

            LocalVertices = GlobalVertices.Select(v => Basis.ToLocalBasis(v)).ToArray();
        }

        private void InitializeTriangles() =>
            Triangles = new[]
            {
                new Triangle(0, 1, 2),
                new Triangle(0, 2, 3),
                new Triangle(0, 3, 4),
                new Triangle(0, 4, 1),
                new Triangle(1, 2, 3),
                new Triangle(1, 3, 4),
            };
    }
}