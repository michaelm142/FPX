using System;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LodeObj;

namespace FPX
{
    [Editor(typeof(MeshRendererEditor))]
    public class MeshRenderer : Component, IDrawable
    {
        public Model model;

        public Material material
        {
            get { return GetComponent<Material>(); }
        }

        public int startIndex { get; private set; }
        public int primitiveCount { get; private set; }

        public VertexPositionNormalTextureBinormal[] Vertecies { get; private set; }

        public bool Visible { get; set; } = true;

        public int DrawOrder { get; set; }

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public void Draw(GameTime gametime)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    AmbientLight ambient = (g_collection.Find(c => c is AmbientLight) as AmbientLight);
                    effect.AmbientLightColor = (ambient != null ? ambient.DiffuseColor : AmbientLight.DefaultColor).ToVector3();
                    effect.DiffuseColor = material.DiffuseColor.ToVector3();
                    effect.SpecularColor = material.SpecularColor.ToVector3();
                    effect.PreferPerPixelLighting = true;
                    if (material.DiffuseMap != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = material.DiffuseMap;
                    }

                    effect.DirectionalLight0.DiffuseColor = (Color.White * 0.1F).ToVector3();
                    effect.DirectionalLight0.Direction = (Vector3.Down + Vector3.Left + Vector3.Forward);
                    effect.DirectionalLight0.Direction.Normalize();
                    effect.DirectionalLight0.Enabled = true;
                    effect.LightingEnabled = true;

                    effect.FogEnabled = true;
                    effect.FogStart = 1.5F;
                    effect.FogEnd = 100.0F;
                    effect.FogColor = Color.CornflowerBlue.ToVector3();

                    effect.World = GetComponent<Transform>().worldPose;
                    effect.View = Camera.Active.ViewMatrix;
                    effect.Projection = Camera.Active.ProjectionMatrix;
                    effect.CurrentTechnique.Passes[0].Apply();

                    foreach (var meshPart in mesh.MeshParts)
                    {
                        GameCore.graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                        GameCore.graphicsDevice.Indices = meshPart.IndexBuffer;
                        GameCore.graphicsDevice.DrawIndexedPrimitives(Graphics.instance.fillMode, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }
        }

        public override void LoadXml(XmlElement node)
        {
            VertexDeclaration[] decl = new VertexDeclaration[]
            {
                VertexPositionColor.VertexDeclaration,
                VertexPositionColorTexture.VertexDeclaration,
                VertexPositionNormalTexture.VertexDeclaration,
                VertexPositionNormalTextureBinormal.vertexDeclaration,
                VertexPositionTexture.VertexDeclaration,
            };

            string modelName = node.SelectSingleNode("Model").Attributes["Name"].Value;
            try
            {
                model = GameCore.content.Load<Model>(modelName);
                foreach (var mesh in model.Meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        VertexPositionNormalTexture[] vertecies = new VertexPositionNormalTexture[part.VertexBuffer.VertexCount];
                        part.VertexBuffer.GetData(vertecies);

                        VertexPositionNormalTextureBinormal[] verts = new VertexPositionNormalTextureBinormal[vertecies.Length];
                        for (int i = 0; i < vertecies.Length; i++)
                        {
                            verts[i].Position = vertecies[i].Position.ToVector4();
                            verts[i].Normal = vertecies[i].Normal;
                            verts[i].TextureCoordinate = vertecies[i].TextureCoordinate;
                        }
                        Vertecies = verts;

                        int index = 0;
                        // Graphics.instance.renderer.AppendVertecies(verts, out index);
                        startIndex = index;
                        primitiveCount = verts.Length / 2;
                    }
                }
                model.Tag = modelName;
            }
            catch (ContentLoadException)
            {
                Debug.LogError("Models {0} could not be found in content", modelName);
                return;
            }
        }

        public void SaveXml(XmlElement node)
        {
            var modelNode = node.OwnerDocument.CreateElement("Model");
            var filenameAttr = node.OwnerDocument.CreateAttribute("Name");
            filenameAttr.Value = model.Tag.ToString();
            modelNode.Attributes.Append(filenameAttr);
            node.AppendChild(modelNode);
        }
    }

}
