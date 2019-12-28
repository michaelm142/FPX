using System;
using System.Collections.Generic;
using FPX;

namespace FPX.Editor
{
    public static class Selection
    {
        public static event EventHandler<SelectionEventArgs> OnSelectionChanged = new EventHandler<SelectionEventArgs>(SelectionChanged);
        public static List<GameObject> SelectedObjects { get; private set; } = new List<GameObject>();
        public static GameObject selectedObject
        {
            get { return SelectedObjects.Count == 0 ? null : SelectedObjects[0]; }

            set
            {
                SelectedObjects.Clear();
                SelectedObjects.Add(value);
            }
        }

        public static void Select(params GameObject[] arg)
        {
            SelectedObjects.Clear();
            SelectedObjects.AddRange(arg);
            OnSelectionChanged(null, new SelectionEventArgs(arg));
        }

        private static void SelectionChanged(object sender, SelectionEventArgs e)
        {

        }
    }

    public class SelectionEventArgs
    {
        public GameObject[] Objects;

        public GameObject Object { get { return Objects[0]; } }

        public SelectionEventArgs(GameObject[] Objects)
        {
            this.Objects = Objects;
        }
    }
}