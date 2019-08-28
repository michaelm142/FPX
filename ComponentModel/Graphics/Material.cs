using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentModel
{
    public class Material : Component
    {
        public Color AmbientColor = Color.White;
        public Color SpecularColor;
        public Color DiffuseColor = Color.White;

        public Texture2D DiffuseMap;
        public Texture2D NormalMap;
        public Texture2D SpecularMap;

        public void LoadXml(XmlElement node)
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

            if (diffuseMapNode != null)
            {
                DiffuseMap = GameCore.content.Load<Texture2D>(diffuseMapNode.Attributes["FileName"].Value);
                DiffuseMap.Tag = diffuseMapNode.Attributes["FileName"].Value;
            }
            if (normalMapNode != null)
            {
                NormalMap = GameCore.content.Load<Texture2D>(normalMapNode.Attributes["FileName"].Value);
                NormalMap.Tag = normalMapNode.Attributes["FileName"].Value;
            }
            if (specularMapNode != null)
            {
                SpecularMap = GameCore.content.Load<Texture2D>(specularMapNode.Attributes["FileName"].Value);
                SpecularMap.Tag = specularMapNode.Attributes["FileName"].Value;
            }
        }
    }
}
