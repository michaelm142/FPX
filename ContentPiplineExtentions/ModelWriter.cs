using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = LodeObj.ModelContent;

namespace ContentPiplineExtentions
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class ModelWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.indicies.Length);
            for (int i = 0; i < value.indicies.Length; i++)
                output.Write(value.indicies[i]);

            output.Write(value.vertecies.Length);

            for (int i = 0; i < value.vertecies.Length; i++)
            {
                output.Write(value.vertecies[i].Position);
                output.Write(value.vertecies[i].Normal);
                output.Write(value.vertecies[i].TextureCoordinate);
                output.Write(value.binormals[i]);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "RenderModule.ModelReader, RenderModule";
        }
    }
}
