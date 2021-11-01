using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LodeObj;

namespace FPX
{
    public interface IGraphicsObject
    {
        bool Visible { get; set; }

        int DrawOrder { get; set; }
        int PrimitiveCount { get; }
        int[] Indicies { get; }

        Microsoft.Xna.Framework.Graphics.PrimitiveType PrimitiveType { get; }

        VertexPositionNormalTextureBinormal[] Vertecies { get; }

        void Draw();
    }
}
