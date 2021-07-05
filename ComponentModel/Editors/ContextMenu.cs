using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPX.Editor
{
    public sealed class ContextMenu : UIElement
    {
        public List<ContextMenuItem> items = new List<ContextMenuItem>();

        public ContextMenu(IEnumerable<ContextMenuItem> items)
        {
            this.items.AddRange(items);
        }

        public override void Initialize()
        {
            items.ForEach(i => i.Initialize());
        }

        public override void Draw(GameTime gameTime)
        {
            GameCore.spriteBatch.Draw(Editor.uiPannelTexture, (transform as RectTransform).rect, Color.White);
        }

        public static void Show(Vector2 location, params ContextMenuItem[] items)
        {

        }
    }

    public class ContextMenuItem : UIElement
    {
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
