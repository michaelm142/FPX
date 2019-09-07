using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LodeObj;

using XnaModel = Microsoft.Xna.Framework.Graphics.Model;

namespace ComponentModel
{
    public class DeferredRenderer
    {
        public int ScreenWidth
        {
            get { return GameCore.graphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return GameCore.graphicsDevice.Viewport.Height; }
        }

        public ContentManager Content;

        public GraphicsDevice Device
        {
            get { return GameCore.graphicsDevice; }
        }

        public SamplerState anisoSampler;

        RenderTarget2D diffuseMap;
        RenderTarget2D normalMap;
        RenderTarget2D specularMap;
        RenderTarget2D depthMap;

        RenderTargetBinding[] rt_bindings;

        Effect GBufferShader;
        Effect DirectionalLightShader;
        Effect AmbientLightShader;
        BasicEffect basicEffect;

        VertexBuffer TestQuad;
        VertexBuffer ScreenQuad;
        VertexBuffer vBuffer;

        Texture2D testRed;
        Texture2D testGreen;
        Texture2D testBlue;
        Texture2D testYellow;

        public DeferredRenderer()
        {
            Content = GameCore.content;
            DeviceUpdate(GameCore.graphicsDevice.Viewport.Width, GameCore.graphicsDevice.Viewport.Height);

            rt_bindings = new RenderTargetBinding[4]
            {
                diffuseMap,
                normalMap,
                specularMap,
                depthMap
            };
            // GBufferShader = Content.Load<Effect>("Shaders\\DeferredGBuffers");
            // DirectionalLightShader = Content.Load<Effect>("Shaders\\Lights\\DirectionalLight");
            // AmbientLightShader = Content.Load<Effect>("Shaders\\Lights\\AmbientLight");
            basicEffect = new BasicEffect(Device);
            basicEffect.TextureEnabled = true;

            VertexPositionTexture[] testQuadVertecies = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0.0f),new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.0f),new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0.0f),new Vector2(1.0f,1.0f)),
            };

            VertexPositionTexture[] screenQuadVertecies = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1, -1, 0.0f), new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1, 1, 0.0f),new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(1, -1, 0.0f),new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(1, 1, 0.0f),new Vector2(1.0f,1.0f)),
            };

            TestQuad = new VertexBuffer(Device, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.None);
            TestQuad.SetData<VertexPositionTexture>(testQuadVertecies);

            ScreenQuad = new VertexBuffer(Device, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.None);
            ScreenQuad.SetData<VertexPositionTexture>(screenQuadVertecies);

            vBuffer = new VertexBuffer(Device, typeof(VertexPositionNormalTextureBinormal), 1, BufferUsage.None);

            anisoSampler = SamplerState.AnisotropicWrap;
            anisoSampler = new SamplerState();
            anisoSampler.AddressU = TextureAddressMode.Wrap;
            anisoSampler.AddressV = TextureAddressMode.Wrap;
            anisoSampler.AddressW = TextureAddressMode.Wrap;

            anisoSampler.Filter = TextureFilter.Anisotropic;
            anisoSampler.MaxAnisotropy = 16;
            anisoSampler.MaxMipLevel = 4;

            Device.SamplerStates[0] = Device.SamplerStates[1] = Device.SamplerStates[2] = Device.SamplerStates[3] = anisoSampler;

            Debug.Log("Sampler State: " + Device.SamplerStates[3].Filter.ToString());
        }

        private void DeviceUpdate(int ScreenWidth, int ScreenHeight)
        {
            diffuseMap = new RenderTarget2D(Device, ScreenWidth, ScreenHeight, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            diffuseMap.Name = "Diffuse Map";
            normalMap = new RenderTarget2D(Device, ScreenWidth, ScreenHeight, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            normalMap.Name = "Normal Map";
            specularMap = new RenderTarget2D(Device, ScreenWidth, ScreenHeight, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            normalMap.Name = "Specular Map";
            depthMap = new RenderTarget2D(Device, ScreenWidth, ScreenHeight, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents);
            normalMap.Name = "Depth Map";
        }


        public void Draw(GameTime gameTime)
        {
            BeginRenderGBuffers(Camera.Active);

            foreach (MeshRenderer meshRender in Component.g_collection.FindAll(c => c is MeshRenderer))
                RenderObject(meshRender);

            EndRenderGBuffers();

            RenderLights();
        }

        public void RenderObject(MeshRenderer renderer)
        {
            GBufferShader.Parameters["World"].SetValue(renderer.transform.localToWorldMatrix);
            GBufferShader.Parameters["DiffuseColor"].SetValue(renderer.GetComponent<Material>().DiffuseColor.ToVector4());
            GBufferShader.Parameters["SpecularColor"].SetValue(renderer.GetComponent<Material>().SpecularColor.ToVector4());
            //GBufferShader.Parameters["Roughness"].SetValue(_object.GetComponet<Material>().Roughness);
            //GBufferShader.Parameters["SpecularPower"].SetValue(_object.GetComponet<Material>().SpecularPower / 10.0f);
            //GBufferShader.Parameters["SpecularIntensity"].SetValue(_object.GetComponet<Material>().SpecularIntensity / 10.0f);
            GBufferShader.CurrentTechnique.Passes[0].Apply();
            Device.Textures[0] = renderer.GetComponent<Material>().DiffuseMap;
            Device.Textures[1] = renderer.GetComponent<Material>().NormalMap;
            Device.Textures[2] = renderer.GetComponent<Material>().SpecularMap;

            // Device.DrawPrimitives(PrimitiveType.TriangleList, renderer.startIndex, renderer.primitiveCount);
            Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, renderer.Vertecies, 0, renderer.primitiveCount);
        }

        public void BeginRenderGBuffers(Camera camera = null)
        {
            if (camera == null)
                camera = Camera.Active;

            Device.SetRenderTargets(rt_bindings);

            GBufferShader.Parameters["ViewProjection"].SetValue(camera.ViewMatrix * camera.ProjectionMatrix);
            Vector3 cameraForward = camera.transform.forward;
            cameraForward.Normalize();
            GBufferShader.Parameters["CameraForward"].SetValue(cameraForward);

            GBufferShader.CurrentTechnique.Passes[0].Apply();

            Device.Clear(Color.Transparent);
            // Device.SetVertexBuffer(vBuffer);
            Device.BlendState = BlendState.Opaque;
        }

        public void EndRenderGBuffers()
        {
            Device.SetRenderTargets(null);
        }

        public void RenderLights()
        {

            Device.Clear(Camera.Active.ClearColor);
            Device.SetVertexBuffer(ScreenQuad);
            Device.BlendState = BlendState.Additive;
            Device.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Device.SamplerStates[1] = SamplerState.AnisotropicWrap;
            Device.SamplerStates[2] = SamplerState.AnisotropicWrap;
            Device.SamplerStates[3] = SamplerState.AnisotropicWrap;


            //foreach (ILightSource light in root.GetNodesInChildren<DirectionalLight>())
            //{
            //    DirectionalLight dirLight = light as DirectionalLight;
            //    DirectionalLightShader.Parameters["DiffuseColor"].SetValue(light.DiffuseColor.ToVector4());
            //    DirectionalLightShader.Parameters["SpecularColor"].SetValue(light.SpecularColor.ToVector4());
            //    DirectionalLightShader.Parameters["gCameraPos"].SetValue(camera.GetNodeInChildren<Transform>().position);
            //    DirectionalLightShader.Parameters["gInvViewProj"].SetValue(Matrix.Transpose(camera.View * camera.Perspective));
            //    DirectionalLightShader.Parameters["LightDirection"].SetValue(dirLight.direction);
            //    DirectionalLightShader.CurrentTechnique.Passes[0].Apply();
            //    Device.Textures[0] = diffuseMap;
            //    Device.Textures[1] = normalMap;
            //    Device.Textures[2] = specularMap;
            //    Device.Textures[3] = depthMap;

            //    Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            //}

            foreach (AmbientLight light in Component.g_collection.FindAll(c => c is AmbientLight))
            {
                AmbientLightShader.Parameters["DiffuseColor"].SetValue(light.DiffuseColor.ToVector4());
                AmbientLightShader.Parameters["SpecularColor"].SetValue(light.SpecularColor.ToVector4());
                AmbientLightShader.Parameters["gCameraPos"].SetValue(Camera.Active.transform.position);
                AmbientLightShader.CurrentTechnique.Passes[0].Apply();
                Device.Textures[0] = diffuseMap;

                Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            for (int i = 0; i < 4; i++)
                Device.Textures[i] = null;
        }

        public void AppendVertecies(VertexPositionNormalTextureBinormal[] vertecies, out int startIndex)
        {
            startIndex = vBuffer.VertexCount;
            VertexPositionNormalTextureBinormal[] gpu_vertecies = new VertexPositionNormalTextureBinormal[vBuffer.VertexCount];
            vBuffer.GetData(gpu_vertecies);
            VertexPositionNormalTextureBinormal[] newVBuffer = new VertexPositionNormalTextureBinormal[gpu_vertecies.Length + vertecies.Length];

            for (int i = 0; i < newVBuffer.Length; i++)
            {
                if (i < gpu_vertecies.Length)
                    newVBuffer[i] = gpu_vertecies[i];
                else
                    newVBuffer[i] = vertecies[i - gpu_vertecies.Length];
            }

            vBuffer = new VertexBuffer(Device, typeof(VertexPositionNormalTextureBinormal), newVBuffer.Length, BufferUsage.None);
            vBuffer.SetData(newVBuffer);
            startIndex = 0;
        }

        public void SetVBufferXml(XmlElement geometryNode)
        {
            var positionNode = geometryNode.ChildNodes.Cast<XmlNode>().ToList().Find(n => n.Name == "Positions");
            var normalNode = geometryNode.ChildNodes.Cast<XmlNode>().ToList().Find(n => n.Name == "Normals");
            var uvsNode = geometryNode.ChildNodes.Cast<XmlNode>().ToList().Find(n => n.Name == "Uvs");
            var binormalsNode = geometryNode.ChildNodes.Cast<XmlNode>().ToList().Find(n => n.Name == "Binormals");

            string positionString = positionNode.InnerText;
            string normalString = normalNode.InnerText;
            string uvsString = uvsNode.InnerText;
            string binormalsString = binormalsNode.InnerText;

            string[] positionStringData = positionString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] normalStringData = normalString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] uvsStringData = uvsString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] binormalsStringData = binormalsString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            VertexPositionNormalTextureBinormal[] gpu_vertecies = new VertexPositionNormalTextureBinormal[positionStringData.Length / 3];
            for (int i = 0; i < gpu_vertecies.Length; i++)
            {
                int float3Index = i * 3;
                int float2Index = i * 2;

                gpu_vertecies[i].Position = new Vector4(float.Parse(positionStringData[float3Index]), float.Parse(positionStringData[float3Index + 1]), float.Parse(positionStringData[float3Index + 2]), 0.0f);
                gpu_vertecies[i].Normal = new Vector3(float.Parse(normalStringData[float3Index]), float.Parse(normalStringData[float3Index + 1]), float.Parse(normalStringData[float3Index + 2]));
                gpu_vertecies[i].Binormal = new Vector3(float.Parse(binormalsStringData[float3Index]), float.Parse(binormalsStringData[float3Index + 1]), float.Parse(binormalsStringData[float3Index + 2]));
                gpu_vertecies[i].TextureCoordinate = new Vector2(float.Parse(binormalsStringData[float2Index]), float.Parse(binormalsStringData[float2Index + 1]));

            }

            vBuffer = new VertexBuffer(Device, typeof(VertexPositionNormalTextureBinormal), gpu_vertecies.Length, BufferUsage.None);
            vBuffer.SetData(gpu_vertecies);
        }

        public void GetVBufferXml(XmlNode parent)
        {
            VertexPositionNormalTextureBinormal[] gpu_vertecies = new VertexPositionNormalTextureBinormal[vBuffer.VertexCount];
            vBuffer.GetData(gpu_vertecies);

            string positionString = "",
                uvString = "",
                normalString = "",
                binormalString = "";

            for (int i = 0; i < gpu_vertecies.Length; i++)
            {
                positionString += string.Format("{0}, {1}, {2},", gpu_vertecies[i].Position.X, gpu_vertecies[i].Position.Y, gpu_vertecies[i].Position.Z);
                uvString += string.Format("{0}, {1},", gpu_vertecies[i].TextureCoordinate.X, gpu_vertecies[i].TextureCoordinate.Y);
                normalString += string.Format("{0}, {1}, {2},", gpu_vertecies[i].Normal.X, gpu_vertecies[i].Normal.Y, gpu_vertecies[i].Normal.Z);
                binormalString += string.Format("{0}, {1}, {2},", gpu_vertecies[i].Binormal.X, gpu_vertecies[i].Binormal.Y, gpu_vertecies[i].Binormal.Z);
            }

            var geometryNode = parent.AppendChild(parent.OwnerDocument.CreateElement("Geometry"));
            var positionsNode = geometryNode.AppendChild(parent.OwnerDocument.CreateElement("Positions"));
            var normalsNode = geometryNode.AppendChild(parent.OwnerDocument.CreateElement("Normals"));
            var uvsNode = geometryNode.AppendChild(parent.OwnerDocument.CreateElement("Uvs"));
            var binormalsNode = geometryNode.AppendChild(parent.OwnerDocument.CreateElement("Binormals"));

            positionsNode.InnerText = positionString;
            normalsNode.InnerText = normalString;
            uvsNode.InnerText = uvString;
            binormalsNode.InnerText = binormalString;
        }

        public void _debug_renderGBufferResults()
        {
            Device.BlendState = BlendState.AlphaBlend;
            Device.SetVertexBuffer(TestQuad);

            basicEffect.Projection = Matrix.Identity;
            basicEffect.View = Matrix.Identity;

            basicEffect.World = Matrix.CreateTranslation((Vector3.Up + Vector3.Left) * 0.5f);
            basicEffect.Texture = diffuseMap;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            basicEffect.World = Matrix.CreateTranslation((Vector3.Up + Vector3.Right) * 0.5f);
            basicEffect.Texture = normalMap;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            basicEffect.World = Matrix.CreateTranslation((Vector3.Down + Vector3.Left) * 0.5f);
            basicEffect.Texture = specularMap;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            basicEffect.World = Matrix.CreateTranslation((Vector3.Down + Vector3.Right) * 0.5f);
            basicEffect.Texture = depthMap;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        public void _debug_OutuptGBuffers()
        {
            diffuseMap.SaveAsJpeg(new FileStream(Environment.CurrentDirectory + "\\diffuseMapOutput.jpg", FileMode.Create), ScreenWidth, ScreenHeight);
            normalMap.SaveAsJpeg(new FileStream(Environment.CurrentDirectory + "\\normalMapOutput.jpg", FileMode.Create), ScreenWidth, ScreenHeight);
            specularMap.SaveAsJpeg(new FileStream(Environment.CurrentDirectory + "\\specularMapOutput.jpg", FileMode.Create), ScreenWidth, ScreenHeight);
            depthMap.SaveAsJpeg(new FileStream(Environment.CurrentDirectory + "\\depthMapOutput.jpg", FileMode.Create), ScreenWidth, ScreenHeight);
        }
    }



    public static class GraphcisDeviceExtentions
    {
        public static void OutputActiveTextures(this GraphicsDevice Device, string fileNamePreface = "DeviceTexture")
        {
            for (int i = 0; i < 16; i++)
            {
                try
                {
                    Texture2D tex = Device.Textures[i] as Texture2D;
                    string filename = string.Format(Environment.CurrentDirectory + "\\{0}{1}.jpg", fileNamePreface, i.ToString());
                    tex.SaveAsJpeg(new FileStream(filename, FileMode.Create), GameCore.graphicsDevice.Viewport.Width, GameCore.graphicsDevice.Viewport.Height);
                    Console.WriteLine("Wrote device texture{0} to {1}", i, filename);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Device does not have texuture{0} set", i);
                    continue;
                }
            }
        }
    }
}
