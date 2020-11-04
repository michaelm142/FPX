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
    public partial class Input : IGameComponent, IUpdateable, IDisposable
    {
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
        private static extern void GetGamepadState(ref GamepadState state);

        private Vector2 mousePos;
        public static Vector2 mousePosition { get { return Instance.mousePos; } }

        private int mouseWheelDelta;
        public static int MouseWheelDelta { get { return Instance.mouseWheelDelta; } }

        public bool Enabled { get { return true; } }

        public int UpdateOrder { get; set; } = -1;
        const int MaxGamePads = 8;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private List<InputAxis> InputAxisList = new List<InputAxis>();

        private GamepadState gamepadstate;
        private MouseState mousestate;

        internal static Input Instance { get; set; }

        public static float GetAxis(string name)
        {
            var axis = Instance.FindAxis(name);
            if (axis == null)
            {
                Debug.LogError("Input Axis {0} does not exit", name);
                return 0.0f;
            }

            return Instance.ParseAxisValue(axis);
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

                    float sensitivity = 0.0f;
                    if (!string.IsNullOrEmpty(sensitivityAttr))
                        sensitivity = float.Parse(sensitivityAttr);
                    float deadZone = 0.0f;
                    if (!string.IsNullOrEmpty(deadZoneAttr))
                        deadZone = float.Parse(deadZoneAttr);
                    float gravity = 0.0f;
                    if (!string.IsNullOrEmpty(gravityAttr))
                        gravity = float.Parse(gravityAttr);

                    InputPlatform platform = (InputPlatform)Enum.Parse(typeof(InputPlatform), platformAttr);
                    InputAxisList.Add(new InputAxis(nameAttr, axisAttr, platform, sensitivity, deadZone, gravity, positiveButtonAttr, negitiveButtonAttr));
                }
            }

            Debug.Log("Input Axis List:");
            InputAxisList.ForEach(a => Debug.Log("\tName:{0}\tPlatform:{1}\tType:{2}", a.Name, a.Platform, a.Axis));

            mousePos = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            gamepadstate = new GamepadState();
            mousestate = new MouseState();
        }

        public void Update(GameTime gameTime)
        {
            InputUpdate();

            unsafe
            {
                GetMouseState(ref mousestate);
                GetGamepadState(ref gamepadstate);
                mouseWheelDelta += mousestate.lZ;
            }

            for (int i = 0; i < InputAxisList.Count; i++)
            {
                var axis = InputAxisList[i];
                UpdateInputAxis(axis);
                Debug.Log("Axis: {0} Value:{1}", axis.Name, ParseAxisValue(axis));
            }

            foreach (GamePadButton button in Enum.GetValues(typeof(GamePadButton)))
            {
                unsafe
                {
                    Debug.Log("Button {0} is {1}", button, gamepadstate.rgbButtons[(int)button]);
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
                        axis.Value += axis.Sensitivity;
                    if (IsKeyDown(axis.NegitiveButtonAs<KeyCode>()))
                        axis.Value -= axis.Sensitivity;
                    break;
                case InputPlatform.GamePad:
                    if (!string.IsNullOrEmpty(axis.Axis))
                        break;

                    bool hasNegitiveButton = !string.IsNullOrEmpty(axis.NegitiveButton);

                    GamePadButton negitiveButton = GamePadButton.None;
                    GamePadButton positiveButton = axis.PositiveButtonAs<GamePadButton>();

                    if (hasNegitiveButton)
                        negitiveButton = axis.NegitiveButtonAs<GamePadButton>();
                    unsafe
                    {
                        if (gamepadstate.rgbButtons[(int)positiveButton] != 0)
                            axis.Value += axis.Sensitivity;
                        if (hasNegitiveButton && gamepadstate.rgbButtons[(int)negitiveButton] != 0)
                            axis.Value -= axis.Sensitivity;
                    }
                    break;
            }

            axis.Value = MathHelper.Lerp(axis.Value, 0.0f, axis.Gravity);
            axis.Value = MathHelper.Clamp(axis.Value, -1.0f, 1.0f);
        }

        private float ParseAxisValue(InputAxis axis)
        {
            switch (axis.Platform)
            {
                case InputPlatform.GamePad:
                    if (!string.IsNullOrEmpty(axis.Axis))
                    {
                        int val = (int)typeof(GamepadState).InvokeMember(axis.Axis, BindingFlags.GetField, null, gamepadstate, null);
                        float axisVal = (val - 32767) / 32767.0f;

                        if (Math.Abs(axisVal) < axis.DeadZone)
                            return 0.0f;
                        return axisVal;
                    }
                    return axis.Value;

                case InputPlatform.Keyboard:
                    return axis.Value;
            }

            return 0.0f;
        }

        private InputAxis FindAxis(string name)
        {
            return InputAxisList.Find(axis => axis.Name == name);
        }

        public void Dispose()
        {

        }
    }
}