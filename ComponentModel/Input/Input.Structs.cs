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
            public ushort wButtons;
            public byte LeftTrigger;
            public byte RightTrigger;
            public short LeftThumbStickX;
            public short LeftThumbStickY;
            public short RightThumbStickX;
            public short RightThumbStickY;
        }

        unsafe struct MouseState
        {
            public int lX;
            public int lY;
            public int lZ;
            public fixed byte rgbButtons[4];
        }

        public enum InputPlatform : uint
        {
            Keyboard = 0x13,
            Mouse = 0x12,
            GamePad = 0x18,
        }

        private class InputAxis
        {
            public string Name { get; set; }
            public string Axis { get; set; }
            public string PositiveButton { get; set; }
            public string NegitiveButton { get; set; }

            public int DeviceIndex { get; set; }

            public bool Invert { get; set; }

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

            public InputAxis(string Name, string Axis, InputPlatform Platform, float Sensitivity, float DeadZone, 
                float Gravity, string PositiveButton, string NegitiveButton, bool Invert, int DeviceIndex)
            {
                this.Name = Name;
                this.Axis = Axis;
                this.Platform = Platform;
                this.Sensitivity = Sensitivity;
                this.DeadZone = DeadZone;
                this.PositiveButton = PositiveButton;
                this.NegitiveButton = NegitiveButton;
                this.Gravity = Gravity;
                this.Invert = Invert;
                this.DeviceIndex = DeviceIndex;

                Value = 0.0f;
            }
        }
    }
}