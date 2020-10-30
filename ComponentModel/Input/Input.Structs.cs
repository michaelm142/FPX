using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace FPX
{
    public partial class Input
    {
        unsafe struct DIJOYSTATE2
        {
            public int lX;
            public int lY;
            public int lZ;
            public int lRx;
            public int lRy;
            public int lRz;
            public fixed int rglSlider[2];
            public fixed short rgdwPOV[4];
            public fixed byte rgbButtons[128];
            public int lVX;
            public int lVY;
            public int lVZ;
            public int lVRx;
            public int lVRy;
            public int lVRz;
            public fixed int rglVSlider[2];
            public int lAX;
            public int lAY;
            public int lAZ;
            public int lARx;
            public int lARy;
            public int lARz;
            public fixed int rglASlider[2];
            public int lFX;
            public int lFY;
            public int lFZ;
            public int lFRx;
            public int lFRy;
            public int lFRz;
            public fixed int rglFSlider[2];
        }

        struct mousedeltas
        {
            public int lX;
            public int lY;
            public int lZ;
        }

        public enum InputPlatform
        {
            Keyboard,
            Mouse,
            GamePad,
        }

        private struct InputAxis
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public InputPlatform Platform { get; set; }

            public InputAxis(string Name, string Type, InputPlatform Platform)
            {
                this.Name = Name;
                this.Type = Type;
                this.Platform = Platform;

                var data = this.Type.Split(new char[] { ' ' });
            }
        }
    }
}