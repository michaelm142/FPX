using System;
using LodeObj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX
{
    public class BillboardSprite : Component, IGraphicsObject
    {
        public bool Visible { get; set; } = true;
        public int DrawOrder { get; set; }

        public int PrimitiveCount { get { return 2; } }

        private int[] indicies = new int[] { 0, 1, 2, 3 };
        public int[] Indicies { get { return indicies; } }

        public VertexPositionNormalTextureBinormal[] Vertecies { get; private set; }

        public PrimitiveType PrimitiveType => PrimitiveType.TriangleStrip;

        public void Start()
        {
            Vertecies = new VertexPositionNormalTextureBinormal[QuadRenderer.QuadVertecies.Length];
            for (int i = 0; i < Vertecies.Length; i++)
            {
                var quadVertex = QuadRenderer.QuadVertecies[i];
                Vertecies[i].Position = quadVertex.Position.ToVector4(1.0f);
                Vertecies[i].TextureCoordinate = quadVertex.TextureCoordinate;
                Vertecies[i].Tangent = Vector3.Right;
                Vertecies[i].Normal = Vector3.Forward;
                Vertecies[i].BiTangent = Vector3.Up;
            }
        }

        public void Update(GameTime gameTime)
        {
            Vector3 L = (Camera.Active.position - position).Normalized();
            transform.forward = L;
        }

        public void Draw()
        {
        }
    }
}