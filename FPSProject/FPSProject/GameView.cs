using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace FPSProject
{
    public class GameView : Panel
    {
        private const int WM_MOUSEMOVE = 0x200;

        private Game1 simulation;

        [DllImport("user32.dll")]
        static extern void ValidateRect(IntPtr hwnd, IntPtr rect);

        public GameView()
        {
            Paint += GameView_Paint;
            DoubleBuffered = true;
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            if (DesignMode || simulation == null)
                return;

            simulation.GraphicsDevice.Present();
        }

        private void GameView_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode || simulation == null)
                return;

            simulation.GraphicsDevice.Present(null, null, IntPtr.Zero);
        }

        public void LoadSim(string sceneName)
        {
            if (!DesignMode)
            {
                simulation = GameCore.CreateGameInstance(sceneName, Handle) as Game1;
                Debug.Log("Loaded engine with scene name {0}", sceneName);
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
            if (DesignMode || simulation == null)
                return;
            Debug.Log(m.Msg);
            simulation.Tick();
                Invalidate();
        }
    }
}
