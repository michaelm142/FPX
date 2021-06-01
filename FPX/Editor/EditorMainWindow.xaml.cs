using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FPX.Editor
{
    /// <summary>
    /// Interaction logic for EditorMainWindow.xaml
    /// </summary>
    public partial class EditorMainWindow : Window
    {
        public static EditorMainWindow Instance { get; private set; }
        private string sceneName;
        public EditorMainWindow(string sceneName = null)
        {
            if (Instance != null)
                throw new InvalidOperationException("More than one instance Main Window");
            Instance = this;

            InitializeComponent();
            this.sceneName = sceneName;
        }

        private void LoadScene(object sender, RoutedEventArgs e)
        {
            Debug.Log("Scene Loaded");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            var scene = new Scene();
            scene.Load(sceneName);
            GameCore.gameInstance.Components.Add(scene);

        }
    }
}
