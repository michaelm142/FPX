using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX.Editor
{
    public class Image : UIElement
    {
        public Texture2D image;

        public Color color;
        public override void Initialize()
        {
        }

        public override void Draw(GameTime gameTime)
        {
            GameCore.spriteBatch.Draw(image, GetComponent<RectTransform>().rect, color);
        }

    }
}
