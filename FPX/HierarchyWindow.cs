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
using FPX.Editor;

namespace FPX
{
    public partial class HierarchyWindow : DockableToolWindow
    {
        private List<GameObject> Objects = new List<GameObject>();

        public static HierarchyWindow instance { get; private set; }

        public IEnumerable<TreeNode> Nodes(TreeNode parent = null)
        {
            var stack = new Stack<IEnumerator<TreeNode>>();
            IEnumerator<TreeNode> enumerator = treeView1.Nodes.GetEnumerator() as IEnumerator<TreeNode>;

            while (true)
            {
                if (enumerator.MoveNext())
                {
                    var node = enumerator.Current;
                    yield return node;

                    if (node.Nodes.Count > 0)
                    {
                        stack.Push(enumerator);
                        enumerator = node.Nodes.GetEnumerator() as IEnumerator<TreeNode>;
                    }
                }
                else if (stack.Count > 0)
                    enumerator = stack.Pop();
                else
                    break;
            }
        }

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
            if (!obj.Visible)
                node.ForeColor = Color.Gray;

            var childObjects = Objects.FindAll(o => o.transform.parent == obj.transform);
            foreach (var cn in childObjects)
            {
                var childNode = BuildTreeNode(cn);
                node.Nodes.Add(childNode);
            }

            if (Selection.SelectedObjects.Contains(obj))
                treeView1.SelectedNode = node;
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
                var addedNode = treeView1.Nodes.Add(node);

            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Selection.Select(e.Node.Tag as GameObject);
        }
    }
}
