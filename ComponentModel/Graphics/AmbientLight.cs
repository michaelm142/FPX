using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX
{
    [Editor(typeof(AmbientLightEditor))]
    public class AmbientLight : Component, ILightSource
    {
        public Color DiffuseColor { get; set; }

        public Color SpecularColor { get; set; }

        public float SpecularIntensity { get; set; }

        public float SpecularPower { get; set; }

        public static Color DefaultColor { get { return Color.Gray; } }

        public void LoadXml(XmlElement element)
        {
            XmlElement diffuseColorElement = element.SelectSingleNode("DiffuseColor") as XmlElement;
            XmlElement specularColorElement = element.SelectSingleNode("SpecularColor") as XmlElement;
            XmlElement specularIntensityelement = element.SelectSingleNode("SpecularIntensity") as XmlElement;
            XmlElement specularPowerelement = element.SelectSingleNode("SpecularPower") as XmlElement;

            if (diffuseColorElement != null)
                DiffuseColor = new Color(LinearAlgebraUtil.Vector4FromXml(diffuseColorElement));
            if (specularColorElement != null)
               SpecularColor = new Color(LinearAlgebraUtil.Vector4FromXml(specularColorElement));
            if (specularIntensityelement != null)
                SpecularIntensity = float.Parse(specularIntensityelement.InnerText);
            if (specularPowerelement != null)
                SpecularPower = float.Parse(specularPowerelement.InnerText);
        }
    }
}
