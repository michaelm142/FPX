using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using XColor = Microsoft.Xna.Framework.Color;

namespace FPX.Editor
{
    public partial class TexturePicker : UserControl
    {
        private Bitmap Image;

        Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }

            set
            {
                _texture = value;

                if (Image != null)
                    Image.Dispose();
                Image = new Bitmap(value.Width, value.Height);
                XColor[] data = new XColor[Image.Width * Image.Height];
                value.GetData(data);

                for (int y = 0; y < Image.Height; y++)
                {
                    for (int x = 0; x < Image.Width; x++)
                    {
                        XColor pixel = data[x + y * Image.Width];
                        Image.SetPixel(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B));
                    }
                }
                textureBox.Image = Image;
                OnTextureChanged(this, new EventArgs());
            }
        }

        public event EventHandler OnTextureChanged;

        public TexturePicker()
        {
            InitializeComponent();
            OnTextureChanged += TexturePicker_OnTextureChanged;
        }

        private void TexturePicker_OnTextureChanged(object sender, EventArgs e)
        {

        }

        private void textureBox_DragEnter(object sender, DragEventArgs e)
        {
            foreach (var format in e.Data.GetFormats())
            {
                if (format.IndexOf("ContentReference") != -1)
                {
                    AssetManager.ContentReference data = e.Data.GetData(format) as AssetManager.ContentReference;
                    if (data.contentType == AssetManager.ContentType.Texture)
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void textureBox_DragDrop(object sender, DragEventArgs e)
        {
            string format = e.Data.GetFormats().ToList().Find(s => s.IndexOf("ContentReference") != -1);
            AssetManager.ContentReference data = e.Data.GetData(format) as AssetManager.ContentReference;

            Texture = data.Data as Texture2D;
        }
    }
}
