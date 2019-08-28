using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using LodeObj;

// TODO: replace this with the type you want to import.
using TImport = Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent;

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
    [ContentImporter(".obj", DisplayName = "OBJ Importer", DefaultProcessor = "ContentPiplineExtentions.ModelProcessor")]
    public class OBJImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return ObjLoader.Import(new FileStream(filename, FileMode.Open));
        }
    }
}
