using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace FPX
{
    public static class Settings
    {
        static Dictionary<string, object> settings = new Dictionary<string, object>();

        static FileInfo SettingsFile;

        public static bool isInilitized { get; private set; }

        public static IEnumerable<KeyValuePair<string, object>> Collection
        {
            get
            {
                foreach (var setting in settings)
                    yield return setting;
            }
        }

        public static T GetSetting<T>(string name)
        {
            return (T)settings[name];
        }

        public static void SetSetting<T>(string name, T value)
        {
            if (!settings.ContainsKey(name))
                settings.Add(name, value);
            else
                settings[name] = value;
        }

        public static void Initialize()
        {
            if (isInilitized)
                return;

            SettingsFile = new FileInfo(Environment.CurrentDirectory + "//Settings.xml");
            if (!SettingsFile.Exists)
            {
                using (var fileStream = SettingsFile.CreateText())
                {
                    fileStream.WriteLine("<?xml version=\"1.0\" encoding=\"utf - 8\" ?>");
                    fileStream.WriteLine("<Settings></Settings>");
                }
            }
            XmlDocument doc = new XmlDocument();
            using (StreamReader stream = new StreamReader(SettingsFile.OpenRead(), Encoding.UTF8))
            {
                doc.Load(stream);
                var root = doc.FirstChild.NextSibling;

                foreach (XmlElement setting in root.ChildNodes)
                {
                    ReadSetting(setting);
                }
            }

            isInilitized = true;
        }

        private static void ReadSetting(XmlElement setting, string existingText = "")
        {
            float floatVal = 0.0f;
            int intVal = 0;
            bool boolVal = false;
            string strVal = setting.InnerText;

            var childNodes = setting.ChildNodes.Cast<XmlNode>().ToList().FindAll(node => node is XmlElement).Cast<XmlElement>();
            if (childNodes.ToList().Count == 0)
            {
                if (int.TryParse(strVal, out intVal))
                    settings.Add(existingText + setting.Name, intVal);
                else if (bool.TryParse(strVal, out boolVal))
                    settings.Add(existingText + setting.Name, boolVal);
                else if (float.TryParse(strVal, out floatVal))
                    settings.Add(existingText + setting.Name, floatVal);
                else
                    settings.Add(existingText + setting.Name, strVal);
            }
            else
            {
                existingText += setting.Name + "/";
                foreach (XmlElement childSetting in childNodes)
                    ReadSetting(childSetting, existingText);
            }
        }

        public static void ShutDown()
        {
            return;
            XmlDocument doc = new XmlDocument();
            var root = doc.CreateNode(XmlNodeType.Element, "Settings", null);
            doc.AppendChild(root);
            foreach (var v in settings)
            {
                var node = doc.CreateNode(XmlNodeType.Element, v.Key, null) as XmlElement;
                node.InnerText = v.Value.ToString();
                root.AppendChild(node);
            }
            using (StreamWriter stream = new StreamWriter(SettingsFile.Open(FileMode.Truncate)))
                doc.Save(stream);
        }
    }
}
