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
        unsafe struct GamepadState
        {
            public int axis0;
            public int axis1;
            public int axis2;
            public int axis3;
            public int axis4;
            public int axis5;
            public fixed int rglSlider[2];
            public fixed short rgdwPOV[4];
            public fixed byte rgbButtons[128];
            public int axis6;
            public int axis7;
            public int axis8;
            public int axis9;
            public int axis10;
            public int axis11;
            public fixed int rglVSlider[2];
            public int axis12;
            public int axis13;
            public int axis14;
            public int axis15;
            public int axis16;
            public int axis17;
            public fixed int rglASlider[2];
            public int axis18;
            public int axis19;
            public int axis20;
            public int axis21;
            public int axis22;
            public int axis23;
            public fixed int rglFSlider[2];
        }
        unsafe struct MouseState
        {
            public int lX;
            public int lY;
            public int lZ;
            public fixed byte rgbButtons[4];
        }

        public enum InputPlatform
        {
            Keyboard,
            Mouse,
            GamePad,
        }

        private class InputAxis
        {
            public string Name { get; set; }
            public string Axis { get; set; }
            public string PositiveButton { get; set; }
            public string NegitiveButton { get; set; }

            public InputPlatform Platform { get; set; }

            public float Sensitivity { get; set; }
            public float DeadZone { get; set; }
            public float Gravity { get; set; }
            public float Value { get; set; }
            /// <summary>
            /// Positive Button Converted to T
            /// </summary>
            public T PositiveButtonAs<T>() where T : Enum
            { 
                return (T)Enum.Parse(typeof(T), PositiveButton);
            }
            /// <summary>
            /// Negitive Button Converted to T
            /// </summary>
            public T NegitiveButtonAs<T>() where T : Enum
            {
                return (T)Enum.Parse(typeof(T), NegitiveButton);
            }

            public InputAxis(string Name, string Axis, InputPlatform Platform, float Sensitivity, float DeadZone, float Gravity, string PositiveButton, string NegitiveButton)
            {
                this.Name = Name;
                this.Axis = Axis;
                this.Platform = Platform;
                this.Sensitivity = Sensitivity;
                this.DeadZone = DeadZone;
                this.PositiveButton = PositiveButton;
                this.NegitiveButton = NegitiveButton;
                this.Gravity = Gravity;

                var data = this.Axis.Split(new char[] { ' ' });
                Value = 0.0f;
            }
        }
    }
}