using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FPX
{
    public class InputManager : IGameComponent, IUpdateable
    {
        public bool Enabled { get { return true; } }

        public int UpdateOrder { get; set; } = -1;
        const int MaxGamePads = 8;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        internal string Filename { get; set; }

        private List<GamePadState> gamePads = new List<GamePadState>();

        private List<InputAxis> InputAxisList = new List<InputAxis>();

        internal static InputManager Instance { get; set; }

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException("More than one instance of Input Manager!");
            Instance = this;

            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory + "\\" + Filename);

            var rootNode = doc.SelectSingleNode("Scene") as XmlElement;
            var inputNode = rootNode.SelectSingleNode("Input") as XmlElement;
            if (inputNode == null)
                Debug.LogError("No input Axis list loaded for scene {0}", Filename);
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

            for (int i = 0; i < MaxGamePads; i++)
                gamePads.Add(new GamePadState());
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < MaxGamePads; i++)
            {
                gamePads[i] = GamePad.GetState(i);
            }
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