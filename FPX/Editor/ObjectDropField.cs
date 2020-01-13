using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPX.Editor
{
    public partial class ObjectDropField : UserControl
    {
        private object _value;
        public object Value
        {
            get { return _value; }

            set
            {
                string[] nameData = value.GetType().ToString().Split(new char[] { '.' });
                nameLabel.Text = nameData[nameData.Length - 1];
                _value = value;
            }
        }

        public ObjectDropField()
        {
            InitializeComponent();
        }

        private void ObjectDropField_DragEnter(object sender, DragEventArgs e)
        {
            foreach (var format in e.Data.GetFormats())
            {
                if (format.IndexOf("ContentReference") != -1)
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void ObjectDropField_DragDrop(object sender, DragEventArgs e)
        {
            string filename = e.Data.GetData(typeof(string)) as string;
            nameLabel.Text = Path.GetFileNameWithoutExtension(filename);

        }
    }
}
