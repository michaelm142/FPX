using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LodeObj
{
    public struct VertexPositionNormalTextureBinormal : IVertexType
    {
        public Vector4 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector3 Tangent;
        public Vector3 BiTangent;

        public VertexPositionNormalTextureBinormal(Vector3 Position, Vector3 Normal, Vector2 TextureCoordinate, Vector3 Tangent, Vector3 BiTangent)
        {
            this.Position = new Vector4(Position, 1.0f);
            this.Normal = Normal;
            this.Tangent = Tangent;
            this.BiTangent = BiTangent;
            this.TextureCoordinate = TextureCoordinate;
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public static VertexDeclaration vertexDeclaration
        {
            get
            {
                VertexElement[] elements = new VertexElement[5];

                elements[0].Offset = 0;
                elements[0].VertexElementFormat = VertexElementFormat.Vector4;
                elements[0].VertexElementUsage = VertexElementUsage.Position;

                elements[1].Offset = 16;
                elements[1].VertexElementFormat = VertexElementFormat.Vector3;
                elements[1].VertexElementUsage = VertexElementUsage.Normal;

                elements[2].Offset = 28;
                elements[2].VertexElementFormat = VertexElementFormat.Vector2;
                elements[2].VertexElementUsage = VertexElementUsage.TextureCoordinate;

                elements[3].Offset = 36;
                elements[3].VertexElementFormat = VertexElementFormat.Vector3;
                elements[3].VertexElementUsage = VertexElementUsage.Binormal;

                elements[4].Offset = 48;
                elements[4].VertexElementFormat = VertexElementFormat.Vector3;
                elements[4].VertexElementUsage = VertexElementUsage.Binormal;
                elements[4].UsageIndex = 1;

                return new VertexDeclaration(elements);
            }
        }
    }
}