using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crom.Controls;

namespace FPX
{
    public partial class HierarchyWindow : DockableToolWindow
    {
        public static HierarchyWindow instance { get; private set; }

        public HierarchyWindow()
        {
            instance = this;
            InitializeComponent();
        }
    }
}
