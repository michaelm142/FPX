using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FPX
{
    public class AudioSource : Component
    {
        private SoundEffect soundEffect;

        public void Play()
        {
            soundEffect.Play();
        }

        public void LoadXml(XmlElement node)
        {
            XmlElement filenameNode = node.SelectSingleNode("Filename") as XmlElement;

            if (filenameNode != null)
                soundEffect = GameCore.content.Load<SoundEffect>(filenameNode.InnerText);
        }

    }
}