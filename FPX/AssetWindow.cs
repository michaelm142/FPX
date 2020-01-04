using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Crom.Controls;

namespace FPX
{
    public partial class AssetWindow : DockableToolWindow
    {
        private DirectoryInfo activeDirectory;

        public AssetWindow()
        {
            InitializeComponent();
            Invalidated += AssetWindow_Validated;
        }

        private void AssetWindow_Validated(object sender, EventArgs e)
        {
            if (activeDirectory == null)
                return;

            assetListView.Clear();
            foreach (var directory in activeDirectory.GetDirectories())
            {
                var listItem = assetListView.Items.Add(directory.Name, (int)AssetManager.ContentType.Folder);
            }
            foreach (var file in activeDirectory.GetFiles().ToList().FindAll(file => file.Extension == ".xnb"))
            {
                if (!AssetManager.Assets.ContainsKey(file.Name))
                    continue;

                var listItem = assetListView.Items.Add(file.Name, (int)AssetManager.Assets[file.Name]);
                listItem.Tag = AssetManager.Assets[file.Name];
            }
        }

        private void AssetWindow_Load(object sender, EventArgs e)
        {
            activeDirectory = new DirectoryInfo(GameCore.content.RootDirectory);
        }

        private void assetListView_DoubleClick(object sender, EventArgs e)
        {
            if (assetListView.Items.Count == 0)
                return;

            var selectedItem = assetListView.SelectedItems[0];
            if (selectedItem.ImageIndex == (int)AssetManager.ContentType.Folder)
            {
                activeDirectory = new DirectoryInfo(activeDirectory.FullName + "//" + selectedItem.Text);
                Invalidate(true);
            }
        }

        private void folderUpButton_Click(object sender, EventArgs e)
        {
            if (activeDirectory.Name == GameCore.content.RootDirectory && activeDirectory.Parent.Name != GameCore.content.RootDirectory)
                return;

            activeDirectory = activeDirectory.Parent;
            Invalidate(true);
        }

        private void assetListView_DragLeave(object sender, EventArgs e)
        {
            Debug.Log("Asset data left asset window");
        }

        private void assetListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (assetListView.SelectedItems.Count == 0)
                return;

            var selectedItem = assetListView.SelectedItems[0];
            if (selectedItem.ImageIndex == (int)AssetManager.ContentType.Folder)
                return;

            DoDragDrop(selectedItem.Tag, DragDropEffects.All);
        }
    }
}
