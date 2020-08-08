using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX
{
    [Editor(typeof(LightEditor))]
    public class Light : Component, ILightSource
    {
        public Color DiffuseColor { get; set; }

        public Color SpecularColor { get; set; }

        public float SpecularIntensity { get; set; } = 0.0f;
        public float SpecularPower { get; set; } = 0.2f;
        public float Intensity { get; set; } = 1.0f;
        public float Range { get; set; } = 5.0f;

        public LightType LightType { get; set; }

        public static Color DefaultColor { get { return Color.Gray; } }

        public override void LoadXml(XmlElement element)
        {
            XmlElement diffuseColorElement = element.SelectSingleNode("DiffuseColor") as XmlElement;
            XmlElement specularColorElement = element.SelectSingleNode("SpecularColor") as XmlElement;
            XmlElement specularIntensityelement = element.SelectSingleNode("SpecularIntensity") as XmlElement;
            XmlElement specularPowerelement = element.SelectSingleNode("SpecularPower") as XmlElement;
            XmlElement typeElement = element.SelectSingleNode("LightType") as XmlElement;
            XmlElement intensityElement = element.SelectSingleNode("Intensity") as XmlElement;
            XmlElement rangeElement = element.SelectSingleNode("Range") as XmlElement;

            if (diffuseColorElement != null)
                DiffuseColor = LinearAlgebraUtil.ColorFromXml(diffuseColorElement);
            if (specularColorElement != null)
                SpecularColor = LinearAlgebraUtil.ColorFromXml(specularColorElement);
            if (specularIntensityelement != null)
                SpecularIntensity = float.Parse(specularIntensityelement.InnerText);
            if (specularPowerelement != null)
                SpecularPower = float.Parse(specularPowerelement.InnerText);
            if (typeElement != null)
                LightType = (LightType)Enum.Parse(typeof(LightType), typeElement.InnerText);
            if (intensityElement != null)
                Intensity = float.Parse(intensityElement.InnerText);
            if (rangeElement != null)
                Range = float.Parse(rangeElement.InnerText);
        }
    }

    public enum LightType
    {
        Ambient,
        Point,
        Directional,
    }
}
