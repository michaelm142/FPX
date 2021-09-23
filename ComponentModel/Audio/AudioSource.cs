using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FPX
{
    public class AudioSource : Component
    {
        public SoundEffect clip;

        public bool Loop;

        public float volume = 1.0f;
        public float pitch;
        public float pan;

        private List<SoundEffectInstance> activeInstances = new List<SoundEffectInstance>();

        public bool isPlaying { get { return activeInstances.Count > 0; } }

        public void Play()
        {
            var instance = clip.CreateInstance();
            instance.IsLooped = Loop;
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.Play();

            activeInstances.Add(instance);
        }

        public void Update(GameTime gameTime)
        {
            activeInstances.RemoveAll(i => i.IsDisposed);
        }

        public override void LoadXml(XmlElement node)
        {
            XmlElement filenameNode = node.SelectSingleNode("Filename") as XmlElement;

            if (filenameNode != null)
                clip = GameCore.content.Load<SoundEffect>(filenameNode.InnerText);
        }

    }
}