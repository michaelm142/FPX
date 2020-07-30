using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX
{
    public class Material : Component
    {
        public BlendState blendState = BlendState.Opaque;

        public Color AmbientColor = Color.White;
        public Color SpecularColor;
        public Color DiffuseColor = Color.White;

        public Texture2D DiffuseMap;
        public Texture2D NormalMap;
        public Texture2D SpecularMap;

        public override void LoadXml(XmlElement node)
        {
            var ambientNode = node.SelectSingleNode("AmbientColor") as XmlElement;
            var specularNode = node.SelectSingleNode("SpecularColor") as XmlElement;
            var diffuseNode = node.SelectSingleNode("DiffuseColor") as XmlElement;

            var diffuseMapNode = node.SelectSingleNode("DiffuseMap") as XmlElement;
            var normalMapNode = node.SelectSingleNode("NormalMap") as XmlElement;
            var specularMapNode = node.SelectSingleNode("SpecularMap") as XmlElement;

            if (ambientNode != null)
                AmbientColor = new Color(LinearAlgebraUtil.Vector4FromXml(ambientNode));
            if (specularNode != null)
                SpecularColor = new Color(LinearAlgebraUtil.Vector4FromXml(specularNode));
            if (diffuseNode != null)
                DiffuseColor = new Color(LinearAlgebraUtil.Vector4FromXml(diffuseNode));

            if (diffuseMapNode != null && !(diffuseMapNode.Attributes["FileName"] == null || diffuseMapNode.Attributes["FileName"].Value == "Default"))
            {
                DiffuseMap = GameCore.content.Load<Texture2D>(diffuseMapNode.Attributes["FileName"].Value);
                DiffuseMap.Tag = diffuseMapNode.Attributes["FileName"].Value;
            }
            else
            {
                DiffuseMap = DefaultTexture;
                DiffuseMap.Tag = "Default";
            }
            if (normalMapNode != null && !(normalMapNode.Attributes["FileName"] == null || normalMapNode.Attributes["FileName"].Value == "Default"))
            {
                NormalMap = GameCore.content.Load<Texture2D>(normalMapNode.Attributes["FileName"].Value);
                NormalMap.Tag = normalMapNode.Attributes["FileName"].Value;
            }
            else
            {
                NormalMap = DefaultTexture;
                NormalMap.Tag = "Default";
            }
            if (specularMapNode != null && !(specularMapNode.Attributes["FileName"] == null || specularMapNode.Attributes["FileName"].Value == "Default"))
            {
                SpecularMap = GameCore.content.Load<Texture2D>(specularMapNode.Attributes["FileName"].Value);
                SpecularMap.Tag = specularMapNode.Attributes["FileName"].Value;
            }
            else
            {
                SpecularMap = DefaultTexture;
                SpecularMap.Tag = "Default";
            }
        }

        public void SaveXml(XmlElement node)
        {
            var ambientNode = LinearAlgebraUtil.ColorToXml(node.OwnerDocument, "AmbientColor", AmbientColor);
            var specularNode = LinearAlgebraUtil.ColorToXml(node.OwnerDocument, "SpecularColor", SpecularColor);
            var diffuseNode = LinearAlgebraUtil.ColorToXml(node.OwnerDocument, "DiffuseColor", DiffuseColor);

            var diffuseMapNode = node.OwnerDocument.CreateElement("DiffuseMap");
            var normalMapNode = node.OwnerDocument.CreateElement("NormalMap");
            var specularMapNode = node.OwnerDocument.CreateElement("SpecularMap");

            var diffuseMapFilenameAttr = node.OwnerDocument.CreateAttribute("Filename");
            var specularMapFilenameAttr = node.OwnerDocument.CreateAttribute("Filename");
            var normalFilenameAttr = node.OwnerDocument.CreateAttribute("Filename");

            if (DiffuseMap != null)
            {
                diffuseMapFilenameAttr.Value = DiffuseMap.Tag.ToString();
                diffuseMapNode.Attributes.Append(diffuseMapFilenameAttr);
                node.AppendChild(diffuseMapNode);
            }
            if (SpecularMap != null)
            {
                specularMapFilenameAttr.Value = SpecularMap.Tag.ToString();
                specularMapNode.Attributes.Append(specularMapFilenameAttr);
                node.AppendChild(specularMapNode);
            }
            if (NormalMap != null)
            {
                normalFilenameAttr.Value = NormalMap.Tag.ToString();
                normalMapNode.Attributes.Append(normalFilenameAttr);
                node.AppendChild(normalMapNode);
            }

            if (AmbientColor != Color.White)
                node.AppendChild(ambientNode);
            if (DiffuseColor != Color.White)
                node.AppendChild(diffuseNode);
            if (SpecularColor != Color.Transparent)
                node.AppendChild(specularNode);
        }

        private static Texture2D g_defaultTexture;
        public static Texture2D DefaultTexture
        {
            get
            {
                if (g_defaultTexture == null)
                {
                    g_defaultTexture = new Texture2D(GameCore.graphicsDevice, 256, 256);
                    g_defaultTexture.MakeDefault();
                }

                return g_defaultTexture;
            }
        }
    }
}
