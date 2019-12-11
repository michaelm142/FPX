using System;
using System.Collections.Generic;
using ComponentModel;

namespace FPX.Editor
{
    public static class Selection
    {
        public static List<GameObject> SelectedObjects { get; private set; } = new List<GameObject>();
        public static GameObject selectedObject
        {
            get { return SelectedObjects[0]; }

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
        }
    }
}