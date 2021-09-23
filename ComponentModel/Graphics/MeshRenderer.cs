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
        /// <summary>
        /// Model/Mesh/MeshPart data for this object
        /// </summary>
        private object content;
        public Model model
        {
            get { return content as Model; }

            set { content = value; }
        }

        public Material material
        {
            get { return GetComponent<Material>(); }
        }

        public int startIndex { get; private set; }
        public int primitiveCount { get; private set; }
        public int indexCount { get; private set; }

        public VertexPositionNormalTextureBinormal[] Vertecies { get; private set; }

        public int[] Indicies { get; private set; }

        public bool Visible { get; set; } = true;

        public int DrawOrder { get; set; }

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public void Awake()
        {
            if (content is Model)
            {
                if (model.Bones.Count == 1)
                    CreateFromMeshPart(model.Meshes[0].MeshParts[0]);
                else
                {
                    AnylizeHeirarchy(model);
                    Destroy(this);
                }
            }
            else if (content is ModelMeshPart)
                CreateFromMeshPart(content as ModelMeshPart);
        }

        public void Start()
        {
            if (GetComponent<Material>() == null)
                Debug.LogError("Mesh Renderer does not have a material assosiated with it");
        }

        public void Draw(GameTime gametime)
        {
            if (model == null || model.Tag != null)
                return;
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Light ambient = (g_collection.Find(c => c is Light && (c as Light).LightType == LightType.Ambient) as Light);
                    effect.AmbientLightColor = (ambient != null ? ambient.DiffuseColor : Light.DefaultColor).ToVector3();
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
                if (Graphics.Mode == "Default")
                    CreateFromMeshPart(model.Meshes[0].MeshParts[0]);
                model.Tag = modelName;
            }
            catch (ContentLoadException)
            {
                Debug.LogError("Models {0} could not be found in content", modelName);
                return;
            }
        }

        void AnylizeHeirarchy(object input, ModelBone leaf = null)
        {
            if (leaf == null)
            {
                Model model = input as Model;
                if (model == null)
                {
                    Debug.LogError("Invalid call to AnylizeHeirarchy");
                    return;
                }

                var bone = model.Bones[0];
                if (bone.Meshes.Count == 1)
                {
                    var mesh = bone.Meshes[0];
                    if (mesh.MeshParts.Count == 1)
                    {
                        var meshPart = mesh.MeshParts[0];
                        CreateFromMeshPart(meshPart);
                    }
                    else
                    {
                        Debug.LogError("Invalid Mesh Part");
                        return;
                    }
                }

                foreach (var child in bone.Children)
                    AnylizeHeirarchy(transform, child);
            }
            else
            {
                var obj = Instanciate(new GameObject(leaf.Name));
                obj.position = leaf.Transform.Translation;
                obj.rotation = leaf.Transform.Rotation;
                obj.transform.parent = input as Transform;
                if (leaf.Meshes.Count == 1)
                {
                    var renderer = obj.AddComponent<MeshRenderer>();
                    obj.AddComponent(material.Clone());
                    renderer.content = leaf.Meshes[0].MeshParts[0];
                }

                foreach (var child in leaf.Children)
                    AnylizeHeirarchy(transform, child);
            }
        }

        public void CreateFromMeshPart(ModelMeshPart part)
        {
            VertexPositionNormalTexture[] vertecies = new VertexPositionNormalTexture[part.VertexBuffer.VertexCount];
            UInt16[] indicies = new UInt16[part.IndexBuffer.IndexCount];
            part.VertexBuffer.GetData(vertecies);
            part.IndexBuffer.GetData<UInt16>(indicies);

            VertexPositionNormalTextureBinormal[] verts = new VertexPositionNormalTextureBinormal[vertecies.Length];
            for (int i = 0; i < vertecies.Length; i++)
            {
                verts[i].Position = vertecies[i].Position.ToVector4(1.0f);
                verts[i].Normal = vertecies[i].Normal;
                verts[i].TextureCoordinate = vertecies[i].TextureCoordinate;
            }
            Vertecies = verts;
            Indicies = new int[indicies.Length];
            for (int i = 0; i < Indicies.Length; i++)
                Indicies[i] = (int)indicies[i];
            for (int i = 0; i < indicies.Length; i += 3)
            {
                ushort index1 = indicies[i];
                ushort index2 = indicies[i + 1];
                ushort index3 = indicies[i + 2];

                var vert1 = Vertecies[index1];
                var vert2 = Vertecies[index2];
                var vert3 = Vertecies[index3];

                Vector3 edge1 = vert2.Position.ToVector3() - vert1.Position.ToVector3();
                Vector3 edge2 = vert3.Position.ToVector3() - vert1.Position.ToVector3();
                Vector3 deltaUV1 = vert2.TextureCoordinate.ToVector3() - vert1.TextureCoordinate.ToVector3();
                Vector3 deltaUV2 = vert2.TextureCoordinate.ToVector3() - vert1.TextureCoordinate.ToVector3();

                float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

                Vector3 tangent = Vector3.Zero;
                Vector3 bitangent = Vector3.Zero;

                tangent.X = f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X);
                tangent.Y = f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y);
                tangent.Z = f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z);

                bitangent.X = f * (-deltaUV2.X * edge1.X + deltaUV1.X * edge2.X);
                bitangent.Y = f * (-deltaUV2.X * edge1.Y + deltaUV1.X * edge2.Y);
                bitangent.Z = f * (-deltaUV2.X * edge1.Z + deltaUV1.X * edge2.Z);

                Vertecies[index1] = vert1;
                Vertecies[index2] = vert2;
                Vertecies[index3] = vert3;
            }

            startIndex = 0;
            primitiveCount = indicies.Length / 3;
            indexCount = indicies.Length;
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
