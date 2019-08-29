using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Point = System.Drawing.Point;

namespace FPSProject
{
    public class GameView : Panel
    {
        private const int WM_MOUSEMOVE = 0x200;

        private Game1 simulation;

        private GameObject sceneCamera;

        private MouseEventArgs mouseArgsPrev;
        private Point mousePointPrev
        {
            get { return mouseArgsPrev.Location; }
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        public GameView()
        {
            DoubleBuffered = true;
            sceneCamera = new GameObject();
            Camera.Active = sceneCamera.AddComponent<Camera>();
            Resize += GameView_Resize;
            MouseMove += GameView_MouseMove;

        }

        public void InputUpdate()
        {
            if (DesignMode || simulation == null)
                return;

            float forward = 0.0f;
            float up = 0.0f;
            float right = 0.0f;
            if (GetAsyncKeyState((int)Keys.W) != 0)
                forward += 1.0f;
            if (GetAsyncKeyState((int)Keys.S) != 0)
                forward -= 1.0f;
            if (GetAsyncKeyState((int)Keys.D) != 0)
                right += 1.0f;
            if (GetAsyncKeyState((int)Keys.A) != 0)
                right -= 1.0f;
            if (GetAsyncKeyState((int)Keys.E) != 0)
                up += 1.0f;
            if (GetAsyncKeyState((int)Keys.Q) != 0)
                up -= 1.0f;

            sceneCamera.position += sceneCamera.transform.forward * forward
                + sceneCamera.transform.right * right
                + sceneCamera.transform.up * up;

            simulation.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).ToList().Find(method => method.Name == "Draw").Invoke(simulation, new object[] { new GameTime(TimeSpan.Zero, TimeSpan.Zero, false) });
            simulation.GraphicsDevice.Present();
        }

        private void GameView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && mouseArgsPrev.Button == MouseButtons.Right)
            {
                int delta_x = mousePointPrev.X - e.X;
                int delta_y = mousePointPrev.Y - e.Y;

                Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(delta_x)) * Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(delta_y));
                sceneCamera.rotation *= rotation;
            }

            mouseArgsPrev = e;
        }

        private void GameView_Resize(object sender, EventArgs e)
        {
            if (simulation == null || DesignMode)
                return;

            PresentationParameters prams = simulation.GraphicsDevice.PresentationParameters;

            prams.BackBufferWidth = Width;
            prams.BackBufferHeight = Height;

            simulation.GraphicsDevice.Reset(prams);
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            if (DesignMode || simulation == null)
                return;

            simulation.GraphicsDevice.Present();
        }

        public void LoadSim(string sceneName)
        {
            if (!DesignMode)
            {
                simulation = GameCore.CreateGameInstance(sceneName, Handle) as Game1;
                var scene = simulation.Components.ToList().Find(c => c.GetType() == typeof(Scene)) as Scene;
                scene.Load();
                simulation.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).ToList().Find(method => method.Name == "Initialize").Invoke(simulation, new object[] { });
                GameCore.spriteBatch = simulation.GetType().GetProperties().ToList().Find(prop => prop.Name.ToLower() == "spritebatch").GetGetMethod().Invoke(simulation, new object[] { }) as SpriteBatch;
                Debug.Log("Loaded engine with scene name {0}", sceneName);
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
            Debug.Log(m.Msg);
        }
    }
}
