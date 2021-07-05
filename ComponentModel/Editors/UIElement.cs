using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FPX.Editor
{
    public abstract class UIElement : Component, IGameComponent
    {
        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public abstract void Draw(GameTime gameTime);

        public abstract void Initialize();
    }
}
