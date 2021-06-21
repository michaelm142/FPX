using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPX
{
    public partial class GameForm : Form
    {
        public event EventHandler GameLoop;

        public GameForm()
        {
            InitializeComponent();
        }

        // pass the window handle to game events
        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (GameLoop != null) GameLoop.Invoke(sender, e);
        }
    }
}
