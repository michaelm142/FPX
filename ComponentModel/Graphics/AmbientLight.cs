using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentModel
{
    public class AmbientLight : Component, ILightSource
    {
        public Color DiffuseColor { get; set; }

        public Color SpecularColor { get; set; }

        public float SpecularIntensity { get; set; }

        public float SpecularPower { get; set; }
    }
}
