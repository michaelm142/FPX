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
        private List<GameObject> Objects = new List<GameObject>();

        public static HierarchyWindow instance { get; private set; }

        public HierarchyWindow()
        {
            instance = this;
            InitializeComponent();

            Invalidated += TreeView1_Invalidated;
            Resize += TreeView1_Resize; ;
        }

        private void TreeView1_Resize(object sender, EventArgs e)
        {
            UpdateHeirarchy();
        }

        private TreeNode BuildTreeNode(GameObject obj)
        {
            TreeNode node = new TreeNode();
            node.Tag = obj;
            node.Text = obj.Name;

            var childObjects = Objects.FindAll(o => o.transform.parent == obj.transform);
            foreach (var cn in childObjects)
            {
                var childNode = BuildTreeNode(cn);
                node.Nodes.Add(childNode);
            }

            return node;
        }

        private void TreeView1_Invalidated(object sender, EventArgs e)
        {
            UpdateHeirarchy();
        }

        public void AddObject(GameObject obj)
        {
            Objects.Add(obj);
        }

        public bool RemoveObject(GameObject obj)
        {
            return Objects.Remove(obj);
        }

        private void UpdateHeirarchy()
        {
            treeView1.Nodes.Clear();
            foreach (var obj in Objects.FindAll(o => o.transform.parent == null))
            {
                TreeNode node = BuildTreeNode(obj);
                treeView1.Nodes.Add(node);
            }
        }
    }
}
