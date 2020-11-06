using System;

namespace FPX
{
    public enum GamePadButton : uint
    {
        None,
        DPadUp = 0x01,
        DPadDown = 0x02,
        DPadLeft = 0x04,
        DPadRight = 0x08,
        Start = 0x10,
        Back = 0x20,
        LeftStick = 0x40,
        RightStick = 0x80,
        LeftShoulder = 0x100,
        RightShoulder = 0x200,
        A = 0x1000,
        B = 0x2000,
        X = 0x4000,
        Y = 0x8000,
    }
}