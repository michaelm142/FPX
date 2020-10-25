using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework;

namespace FPX
{
    public class Input : IGameComponent, IUpdateable
    {
        [DllImport("InputModule.dll", EntryPoint = "InitializeInputModule", CharSet = CharSet.Unicode)]
        private static extern int InitializeInputModule(int hwnd);
        [DllImport("InputModule.dll", EntryPoint = "InputUpdate", CharSet = CharSet.Unicode)]
        private static extern void InputUpdate();
        [DllImport("InputModule.dll", CharSet = CharSet.Unicode)]
        public static extern void IsKeyDown(KeyCode keyCode);

        public bool Enabled { get { return true; } }

        public int UpdateOrder { get; set; } = -1;
        const int MaxGamePads = 8;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private List<InputAxis> InputAxisList = new List<InputAxis>();

        internal static Input Instance { get; set; }

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException("More than one instance of Input Manager!");
            Instance = this;
            int windowHandle = (int)GameCore.gameInstance.Window.Handle;
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
                    var typeAttr = node.GetAttribute("Type");

                    InputPlatform platform = (InputPlatform)Enum.Parse(typeof(InputPlatform), platformAttr);
                    InputAxisList.Add(new InputAxis(nameAttr, typeAttr, platform));
                }
            }

            Debug.Log("Input Axis List:");
            InputAxisList.ForEach(a => Debug.Log("\tName:{0}\tPlatform:{1}\tType:{2}", a.Name, a.Platform, a.Type));
        }

        public void Update(GameTime gameTime)
        {
            InputUpdate();
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