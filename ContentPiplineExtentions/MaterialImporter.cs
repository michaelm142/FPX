using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
using TImport = ContentPiplineExtentions.MaterialContent;

namespace ContentPiplineExtentions
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".xml", DisplayName = "Material Importer")]
    public class MaterialImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new FileStream(filename, FileMode.Open));

            var root = doc.SelectSingleNode("Material");
            
            var DiffuseMapNode = root.SelectSingleNode("DiffuseMap");
            var NormalMapNode = root.SelectSingleNode("NormalMap");
            var SpecularMapNode = root.SelectSingleNode("SpecularMap");

            string DiffuseMap = DiffuseMapNode == null ? "" : DiffuseMapNode.InnerText;
            string SpecularMap = NormalMapNode == null ? "" : NormalMapNode.InnerText;
            string NormalMap = SpecularMapNode == null ? "" : SpecularMapNode.InnerText;
            
            float diffuseR = float.Parse(DiffuseMapNode.Attributes["DiffuseColorR"].InnerText);
            float diffuseG = float.Parse(DiffuseMapNode.Attributes["DiffuseColorG"].InnerText);
            float diffuseB = float.Parse(DiffuseMapNode.Attributes["DiffuseColorB"].InnerText);
            float diffuseA = float.Parse(DiffuseMapNode.Attributes["DiffuseColorA"].InnerText);

            float roughness = float.Parse(NormalMapNode.Attributes["Roughness"].InnerText);

            float specularIntensity = float.Parse(SpecularMapNode.Attributes["SpecularIntensity"].InnerText);
            float specularPower = float.Parse(SpecularMapNode.Attributes["SpecularPower"].InnerText);
            
            float specularR = float.Parse(SpecularMapNode.Attributes["SpecularColorR"].InnerText);
            float specularG = float.Parse(SpecularMapNode.Attributes["SpecularColorG"].InnerText);
            float specularB = float.Parse(SpecularMapNode.Attributes["SpecularColorB"].InnerText);

            return new TImport(DiffuseMap, NormalMap, SpecularMap, roughness, specularPower, specularIntensity, new Vector4(diffuseR, diffuseG, diffuseB, diffuseA), new Vector4(specularR, specularG, specularB, 1.0f));
        }
    }

    public struct MaterialContent
    {
        public string DiffuseMap;
        public string NormalMap;
        public string SpecularMap;

        public float Roughness;

        public float SpecularPower;
        public float SpecularIntensity;

        public Vector4 diffuseColor;
        public Vector4 specularColor;

        public MaterialContent(string DiffuseMap, string NormalMap, string SpecularMap, float Roughness, float SpecularPower, float SpecularIntensity, Vector4 diffuseColor, Vector4 specularColor)
        {
            this.SpecularMap = SpecularMap;
            this.NormalMap = NormalMap;
            this.DiffuseMap = DiffuseMap;
            this.Roughness = Roughness;
            this.SpecularPower = SpecularPower;
            this.SpecularIntensity = SpecularIntensity;
            this.diffuseColor = diffuseColor;
            this.specularColor = specularColor;
        }
    }
}
