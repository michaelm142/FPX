using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace FPX
{
    public partial class Input : IGameComponent, IUpdateable, IDisposable, IDrawable
    {
        #region External
        [DllImport("InputModule.dll", EntryPoint = "InitializeInputModule", CharSet = CharSet.Unicode)]
        private static extern int InitializeInputModule(int hwnd);
        [DllImport("InputModule.dll", EntryPoint = "InputUpdate", CharSet = CharSet.Unicode)]
        private static extern void InputUpdate();
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        public static extern bool IsKeyDown(KeyCode keyCode);
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        private static extern void GetMouseState(ref MouseState state);
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        private static extern void GetMousePosition(IntPtr ptr);
        [DllImport("user32.dll")]
        private static extern void GetPhysicalCursorPos(ref Point point);
        [DllImport("user32.dll")]
        private static extern void PhysicalToLogicalPoint(IntPtr ptr, ref Point point);
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        private static extern void GetGamepadState(ref GamepadState state, uint index);
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        private static extern void Close();
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        private static extern bool IsDeviceConnected(uint type, int index);
        #endregion

        private Vector2 mousePos;
        public static Vector2 mousePosition { get { return Instance.mousePos; } }

        private int mouseWheelDelta;
        public static int MouseWheelDelta { get { return Instance.mouseWheelDelta; } }

        public bool Enabled { get { return true; } }

        public int UpdateOrder { get; set; } = -1;
        const int MaxGamePads = 8;
        const int AnyControler = int.MaxValue;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        private List<InputAxis> InputAxisList = new List<InputAxis>();

        private List<GamepadState> gamepads = new List<GamepadState>();
        private MouseState mousestate;

        internal static Input Instance { get; set; }

        public int DrawOrder { get { return 1000; } }

        public bool Visible => true;

        public static float GetAxis(string name)
        {
            var axis = Instance.FindAxis(name);
            if (axis == null)
            {
                Debug.LogError("Input Axis {0} does not exit", name);
                return 0.0f;
            }

            return axis.Value;
        }

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException("More than one instance of Input Manager!");
            Instance = this;
            var window = GameCore.gameInstance.Window;
            int windowHandle = (int)window.Handle;
            int hresult = InitializeInputModule(windowHandle);
            if (hresult != 0)
                Debug.LogError("Failed to inilitize input module");

            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory + "\\Settings.xml");

            var rootNode = doc.SelectSingleNode("Settings") as XmlElement;
            var inputNode = rootNode.SelectSingleNode("Input") as XmlElement;
            if (inputNode == null)
                Debug.LogError("No input Axis list loaded in project settings");
            else
            {
                foreach (XmlElement node in inputNode.SelectNodes("Axis"))
                {
                    var nameAttr = node.GetAttribute("Name");
                    var platformAttr = node.GetAttribute("Platform");
                    var axisAttr = node.GetAttribute("Axis");
                    var sensitivityAttr = node.GetAttribute("Sensitivity");
                    var deadZoneAttr = node.GetAttribute("DeadZone");
                    var positiveButtonAttr = node.GetAttribute("PositiveButton");
                    var negitiveButtonAttr = node.GetAttribute("NegitiveButton");
                    var gravityAttr = node.GetAttribute("Gravity");
                    var invertAttr = node.GetAttribute("Invert");
                    var deviceIndexAttr = node.GetAttribute("DeviceIndex");

                    float sensitivity = 1.0f;
                    if (!string.IsNullOrEmpty(sensitivityAttr))
                        sensitivity = float.Parse(sensitivityAttr);
                    float deadZone = 0.0f;
                    if (!string.IsNullOrEmpty(deadZoneAttr))
                        deadZone = float.Parse(deadZoneAttr);
                    float gravity = 0.0f;
                    if (!string.IsNullOrEmpty(gravityAttr))
                        gravity = float.Parse(gravityAttr);
                    bool invert = false;
                    if (!string.IsNullOrEmpty(invertAttr))
                        invert = bool.Parse(invertAttr);
                    int deviceIndex = AnyControler;
                    if (!string.IsNullOrEmpty(deviceIndexAttr))
                    {
                        if (deviceIndexAttr.ToLower() == "any")
                            deviceIndex = AnyControler;
                        else
                            deviceIndex = int.Parse(deviceIndexAttr);
                    }

                    InputPlatform platform = (InputPlatform)Enum.Parse(typeof(InputPlatform), platformAttr);
                    InputAxisList.Add(new InputAxis(nameAttr, axisAttr, platform, sensitivity, deadZone, gravity, positiveButtonAttr, negitiveButtonAttr, invert, deviceIndex));
                }
            }

            Debug.Log("Input Axis List:");
            InputAxisList.ForEach(a => Debug.Log("\tName:{0}\tPlatform:{1}\tType:{2}", a.Name, a.Platform, a.Axis));

            mousePos = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            for (int i = 0; i < MaxGamePads; i++)
                gamepads.Add(new GamepadState());

            mousestate = new MouseState();
        }

        public void Update(GameTime gameTime)
        {
            InputUpdate();

            unsafe
            {
                GetMouseState(ref mousestate);
                for (int i = 0; i < MaxGamePads; i++)
                {
                    if (!IsDeviceConnected((uint)InputPlatform.GamePad, i))
                        continue;

                    GamepadState gamepad = gamepads[i];
                    GetGamepadState(ref gamepad, (uint)i);
                    gamepads[i] = gamepad;
                }
                mouseWheelDelta += mousestate.lZ;
            }

            for (int i = 0; i < InputAxisList.Count; i++)
            {
                var axis = InputAxisList[i];
                UpdateInputAxis(axis);
            }

            for (int i = 0; i < MaxGamePads; i++)
            {
                if (!IsDeviceConnected((uint)InputPlatform.GamePad, i))
                    continue;
                foreach (GamePadButton button in Enum.GetValues(typeof(GamePadButton)))
                {
                    if ((gamepads[i].wButtons & (ushort)button) != 0)
                        Debug.Log("Button {0} is down on Controller{1}", button, i);
                }
            }
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (IsKeyDown(key))
                    Debug.Log("{0} is down", key);
            }

            mousePos.X += mousestate.lX;
            mousePos.Y += mousestate.lY;
            mousePos = Vector2.Clamp(mousePos, Vector2.Zero, new Vector2(Screen.Width, Screen.Height));
        }

        private void UpdateInputAxis(InputAxis axis)
        {
            switch (axis.Platform)
            {
                case InputPlatform.Keyboard:
                    if (IsKeyDown(axis.PositiveButtonAs<KeyCode>()))
                        axis.Value += axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                    if (IsKeyDown(axis.NegitiveButtonAs<KeyCode>()))
                        axis.Value -= axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                    break;
                case InputPlatform.GamePad:
                    if (!string.IsNullOrEmpty(axis.Axis))
                    {
                        int val = 0;
                        if (axis.DeviceIndex == AnyControler)
                        {
                            for (int i = 0; i < MaxGamePads; i++)
                            {
                                if (IsDeviceConnected((uint)InputPlatform.GamePad, i))
                                    val += (short)typeof(GamepadState).InvokeMember(axis.Axis, BindingFlags.GetField, null, gamepads[i], null);
                            }
                        }
                        else
                            val = (short)typeof(GamepadState).InvokeMember(axis.Axis, BindingFlags.GetField, null, gamepads[axis.DeviceIndex], null);
                        float axisValue = val / (float)short.MaxValue;
                        if (Math.Abs(axisValue) < axis.DeadZone)
                            axisValue = 0.0f;
                        axis.Value = axis.Invert ? -axisValue : axisValue;
                        break;
                    }

                    bool hasNegitiveButton = !string.IsNullOrEmpty(axis.NegitiveButton);

                    GamePadButton negitiveButton = GamePadButton.None;
                    GamePadButton positiveButton = axis.PositiveButtonAs<GamePadButton>();

                    if (hasNegitiveButton)
                        negitiveButton = axis.NegitiveButtonAs<GamePadButton>();
                    unsafe
                    {
                        if (axis.DeviceIndex == AnyControler)
                        {
                            for (int i = 0; i < MaxGamePads; i++)
                            {
                                if ((gamepads[i].wButtons & (uint)positiveButton) != 0)
                                    axis.Value += axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                                if (hasNegitiveButton && (gamepads[i].wButtons & (uint)negitiveButton) != 0)
                                    axis.Value -= axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                            }
                        }
                        else
                        {
                            if ((gamepads[axis.DeviceIndex].wButtons & (uint)positiveButton) != 0)
                                axis.Value += axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                            if (hasNegitiveButton && (gamepads[axis.DeviceIndex].wButtons & (uint)negitiveButton) != 0)
                                axis.Value -= axis.Invert ? -axis.Sensitivity : axis.Sensitivity;
                        }
                    }
                    break;
            }

            axis.Value = MathHelper.Lerp(axis.Value, 0.0f, axis.Gravity);
            axis.Value = MathHelper.Clamp(axis.Value, -1.0f, 1.0f);
        }

        private InputAxis FindAxis(string name)
        {
            return InputAxisList.Find(axis => axis.Name == name);
        }

        public void Dispose()
        {
            Close();
        }

        public void Draw(GameTime gameTime)
        {
            var spriteBatch = GameCore.spriteBatch;
            string output = "Input Axis List\n";
            foreach (var axis in InputAxisList)
                output += string.Format("{0}: {1}\n", axis.Name, axis.Value);
            spriteBatch.Begin();
            spriteBatch.DrawString(GameCore.fonts["SegoeUI"], output, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}