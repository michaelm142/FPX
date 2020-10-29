﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPX
{
    public static class Screen
    {
        public static int Width
        {
            get { return GameCore.viewport.Width; }
        }

        public static int Height
        {
            get { return GameCore.viewport.Height; }
        }

        public static IntPtr handle
        {
            get { return GameCore.gameInstance.Window.Handle; }
        }
    }
}
