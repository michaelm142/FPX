using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LodeObj;

namespace ComponentModel
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

        public void Initialize()
        {
            instance = this;
            renderer = new DeferredRenderer();
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
                GameCore.graphicsDevice.Clear(Camera.Active.ClearColor);
                foreach (var obj in Scene.Active.Objects)
                {
                    if (obj.Visible)
                        obj.Draw(gameTime);
                }
            }
            else if (Mode == "Deferred")
            {
                renderer.Draw(gameTime);
            }
            else if (Mode == "DeferredDebug")
            {
                renderer.BeginRenderGBuffers();
                foreach (var obj in Component.g_collection.FindAll(c => c is MeshRenderer).Cast<MeshRenderer>())
                    renderer.RenderObject(obj);
                renderer.EndRenderGBuffers();

                renderer._debug_renderGBufferResults();
            }

            GameCore.spriteBatch.Begin();
            {
                Scene.Active.BroadcastMessage("DrawUI", GameCore.spriteBatch);
            }
            GameCore.spriteBatch.End();

            GameCore.graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Dispose()
        {
            renderer = null;
        }
    }
}
