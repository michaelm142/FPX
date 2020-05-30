using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LodeObj;

namespace FPX
{
    public class Graphics : IGameComponent, IDrawable, IDisposable
    {
        public bool Visible { get { return true; } }

        public int DrawOrder { get { return int.MaxValue; } }

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public static Graphics instance { get; private set; }

        public DeferredRenderer renderer { get; private set; }

        public static string Mode = "Default";

        private Effect clearDepthShader;

        private Texture2D transparentTexture;

        public PrimitiveType fillMode = PrimitiveType.TriangleList;


        public void Initialize()
        {
            instance = this;
            renderer = new DeferredRenderer();
            clearDepthShader = GameCore.content.Load<Effect>("Shaders\\ClearDepth");
            transparentTexture = new Texture2D(GameCore.graphicsDevice, 1, 1);
            transparentTexture.SetData(new Color[] { Color.Transparent });

            string fillModeName = Settings.GetSetting<string>("FillMode");
            if (!string.IsNullOrEmpty(fillModeName))
                fillMode = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), fillModeName);

            GameCore.gameInstance.Components.Add(new QuadRenderer());
            GameCore.graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
        }

        public static void ClearDepth()
        {
            var device = GameCore.graphicsDevice;
            var blendState = device.BlendState;

            device.BlendState = BlendState.AlphaBlend;
            QuadRenderer.Instance.RenderQuad(instance.transparentTexture, new Rectangle(0, 0, GameCore.viewport.Width, GameCore.viewport.Height), instance.clearDepthShader);
            device.BlendState = blendState;
        }

        static int SortRenderables(IDrawable a, IDrawable b)
        {
            return a.DrawOrder.CompareTo(b.DrawOrder);
        }

        public void Draw(GameTime gameTime)
        {
            if (Camera.Active == null)
                return;
            if (!GameCore.currentLevel.IsLoaded)
            {
                GameCore.graphicsDevice.Clear(Color.Magenta);
                return;
            }

            if (Mode == "Default")
            {
                var drawables = Component.g_collection.ToList().FindAll(c => c is IDrawable && c.gameObject.Visible).Cast<IDrawable>().ToList();
                drawables.Sort(SortRenderables);

                var postProcessor = Camera.Active.GetComponent<PostProcessor>();
                if (postProcessor != null)
                    postProcessor.Begin();

                GameCore.graphicsDevice.Clear(Camera.Active.ClearColor);

                foreach (var drawable in drawables)
                    drawable.Draw(gameTime);

                if (postProcessor != null)
                    postProcessor.End();
            }
            else if (Mode == "Deferred")
            {
                renderer.Draw(gameTime);
            }
            else if (Mode == "DeferredDebug")
            {
                renderer.BeginRenderGBuffers();
                foreach (var obj in Component.g_collection.FindAll(c => c is MeshRenderer).Cast<MeshRenderer>())
                {
                    if (!obj.gameObject.Visible)
                        continue;
                    renderer.RenderObject(obj);
                }
                renderer.EndRenderGBuffers();

                renderer._debug_renderGBufferResults();
            }

            //GameCore.spriteBatch.Begin();
            //{
            //    Scene.Active.BroadcastMessage("DrawUI", GameCore.spriteBatch);
            //}
            //GameCore.spriteBatch.End();

            GameCore.graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Dispose()
        {
            renderer = null;
        }
    }
}
