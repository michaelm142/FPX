using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentModel
{
    public class SkyboxRenderer : Component, IDrawable
    {
        public bool Visible { get; set; }

        public int DrawOrder { get { return -1000; } }

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        private TextureCube SkyCube;
        private Texture2D testTexture;

        private Effect SkyCubeShader;

        private Model model;

        public void Start()
        {
            SkyCube = GameCore.content.Load<TextureCube>("Textures\\CubeMap");

            model = GameCore.content.Load<Model>("Models\\Cube");
            SkyCubeShader = GameCore.content.Load<Effect>("Shaders\\CubeMapRender");
        }

        public void Draw(GameTime gameTime)
        {
            var device = GameCore.graphicsDevice;
            device.RasterizerState = RasterizerState.CullNone;
            device.DepthStencilState = DepthStencilState.None;
            device.Textures[4] = SkyCube;
            foreach (var mesh in model.Meshes)
            {
                SkyCubeShader.Parameters["World"].SetValue(Matrix.Identity);
                SkyCubeShader.Parameters["View"].SetValue(Matrix.Invert(Matrix.CreateFromQuaternion(Camera.Active.rotation)));
                SkyCubeShader.Parameters["Projection"].SetValue(Camera.Active.ProjectionMatrix);
                device.SamplerStates[4] = SamplerState.LinearClamp;
                SkyCubeShader.CurrentTechnique.Passes[0].Apply();
                device.SetVertexBuffer(mesh.MeshParts[0].VertexBuffer);
                device.Indices = mesh.MeshParts[0].IndexBuffer;

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mesh.MeshParts[0].NumVertices, 0, mesh.MeshParts[0].PrimitiveCount);
            }
            device.RasterizerState = RasterizerState.CullCounterClockwise;

            device.DepthStencilState = DepthStencilState.Default;

            Graphics.ClearDepth();
        }
    }
}
