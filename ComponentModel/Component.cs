using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FPX.Editor;

namespace FPX
{
    public abstract class Component
    {
        public GameObject gameObject;

        public Transform transform
        {
            get { return GetComponent<Transform>(); }
        }

        [IgnoreInGUI]
        public Vector3 position
        {
            get { return transform.position; }

            set { transform.position = value; }
        }

        [IgnoreInGUI]
        public Vector3 scale
        {
            get { return transform.localScale; }

            set { transform.localScale = value; }
        }

        [IgnoreInGUI]
        public Vector3 localPosition
        {
            get { return transform.localPosition; }

            set { transform.localPosition = value; }
        }

        [IgnoreInGUI]
        public Quaternion rotation
        {
            get { return transform.rotation; }

            set { transform.rotation = value; }
        }

        [IgnoreInGUI]
        public Quaternion localRotation
        {
            get { return GetComponent<Transform>().localRotation; }

            set { GetComponent<Transform>().localRotation = value; }
        }

        internal static List<Component> g_collection = new List<Component>();

        public Component()
        {
            g_collection.Add(this);
        }

        public T GetComponent<T>()
            where T : Component
        {
            return gameObject.GetComponent<T>();
        }


        public List<T> GetComponents<T>()
            where T : Component
        {
            return gameObject.GetComponents<T>();
        }

        public bool KnowsMessage(string message)
        {
            return GetType().GetMethods().ToList().Find(x => x.Name == message) != null;
        }

        public void SendMessage(string message, params object[] parameters)
        {
            if (message == "SendMessage")
                return;

            var method = GetType().GetMethods().ToList().Find(x => x.Name == message);
            if (method != null)
            {
                try
                {
                    method.Invoke(this, parameters);
                }
                catch (System.Reflection.TargetInvocationException e)
                {
                    Exception innerException = e;
                    while (innerException is System.Reflection.TargetInvocationException&& innerException != null)
                        innerException = e.InnerException;

                    Debug.LogError("Sending message raised exception of type {0}. {1} \nStack Trace:\n{2}", innerException.GetType(), innerException.Message, innerException.StackTrace);
                } 
            }
        }

        public List<T> FindObjectsOfType<T>()
            where T : Component
        {
            return g_collection.FindAll(o => o is T).Cast<T>().ToList();
        }

        public T FindObjectOfType<T>()
            where T : Component
        {
            return g_collection.Find(o => o is T) as T;
        }

        public static GameObject Instantiate(Prefab prefab)
        {
            return GameCore.currentLevel.Spawn(prefab);
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", GetType().ToString(), gameObject.Name);
        }
    }
}
