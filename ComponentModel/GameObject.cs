using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX
{
    public sealed class GameObject : WeakReference
    {
        private _gameObject value { get { return Target as _gameObject; } }

        // ==<Wrapper Functions>==
        internal List<Component> Components
        {
            get { return value.Components; }
        }
        public IEnumerable<Component> GetComponents()
        {
            return value.GetComponents();
        }
        internal bool instanciated
        {
            get { return value.instanciated; }
            set { this.value.instanciated = value; }
        }

        internal bool destroyed
        {
            get { return value.destroyed; }
            set { this.value.destroyed = value; }
        }

        public int DrawOrder
        {
            get { return value.DrawOrder; }
            set { this.value.DrawOrder = value; }
        }

        public bool Enabled
        {
            get { return value.Enabled; }
            set { this.value.Enabled = value; }
        }

        public int UpdateOrder
        {
            get { return value.UpdateOrder; }
            set { this.value.UpdateOrder = value; }
        }

        public bool Visible
        {
            get { return value.Visible; }
            set { this.value.Visible = value; }
        }

        public string Name
        {
            get { return value.Name; }
            set { this.value.Name = value; }
        }

        public string Tag
        {
            get { return value.Tag; }
            set { this.value.Tag = value; }
        }
        public Transform transform
        {
            get { return value.transform; }
        }
        public Vector3 localPosition
        {
            get { return value.localPosition; }
            set { this.value.localPosition = value; }
        }
        public Vector3 position
        {
            get { return value.position; }
            set { this.value.position = value; }
        }
        public Quaternion localRotation
        {
            get { return value.localRotation; }
            set { this.value.localRotation = value; }
        }
        public Quaternion rotation
        {
            get { return value.rotation; }
            set { this.value.rotation = value; }
        }
        internal uint Id
        {
            get { return value.Id; }
            set { this.value.Id = value; }
        }

        private GameObject(_gameObject value)
            : base(value)
        {
            if (_instances.IndexOf(value) == -1)
                _instances.Add(value);
        }

        private GameObject(bool isEmpty)
            : base(new _gameObject(isEmpty), true)
        {
            _instances.Add(Target as _gameObject);
        }

        public GameObject()
        : base(new _gameObject(), true)
        {
            _instances.Add(Target as _gameObject);
        }

        public GameObject(string Name)
            : base(new _gameObject(Name), true)
        {
            _instances.Add(Target as _gameObject);
        }

        public GameObject(string Name, params Type[] Components)
            : base(new _gameObject(Name, Components), true)
        {
            _instances.Add(Target as _gameObject);
        }

        public T AddComponent<T>()
            where T : Component
        {
            return value.AddComponent<T>();
        }

        public void AddComponent(Component c)
        {
            value.AddComponent(c);
        }

        public Component AddComponent(Type t)
        {
            return value.AddComponent(t) as Component;
        }

        public T GetComponent<T>()
            where T : Component
        {
            return value.GetComponent<T>();
        }

        public List<T> GetComponents<T>()
            where T : Component
        {
            return value.GetComponents<T>();
        }

        public override string ToString()
        {
            return value != null ? value.ToString() : "null";
        }

        public void Draw(GameTime gameTime)
        {
            value.Draw(gameTime);
        }

        public void BroadcastMessage(string Name, params object[] parameters)
        {
            value.BroadcastMessage(Name, parameters);
        }

        public void Run(GameTime gameTime)
        {
            value.Run(gameTime);
        }

        public bool KnowsMessage(string Name)
        {
            return value.KnowsMessage(Name);
        }

        // ==<Static Functions>==

        public static void Destroy(GameObject gameObject)
        {
            _gameObject.Destroy(gameObject.value);

            int index = _instances.IndexOf(_instances.Find(i => i == gameObject.value));
            _instances[index] = null;
            gameObject.Target = null;
        }
        public static GameObject Empty
        {
            get { return new GameObject(true); }
        }

        public static GameObject Find(string name)
        {
            return Scene.Active.Objects.ToList().Find(i => i.Name == name);
        }

        public static GameObject Find(uint Id)
        {
            return Scene.Active.Objects.ToList().Find(i => i.Id == Id);
        }

        public static GameObject Load(XmlElement node)
        {
            return _gameObject.Load(node);
        }

        public static List<Component> FindObjectsOfType<T>()
            where T : Component
        {
            return Component.g_collection.FindAll(c => c is T);
        }

        public bool Destroyed
        {
            get { return value is null; }
        }

        internal bool CompareTo(GameObject obj)
        {
            if (obj is null)
            {
                if (value is null)
                    return true;

                return false;
            }

            return value.GetHashCode() == obj.value.GetHashCode();
        }

        private static List<_gameObject> _instances = new List<_gameObject>();
        public static implicit operator _gameObject(GameObject obj) => obj.value;
        public static implicit operator GameObject(_gameObject obj) => new GameObject(obj);

        public static GameObject FindByTag(string Tag)
        {
            return _instances.Find(i => i.Tag == Tag);
        }

    }
    public sealed class _gameObject
    {
        internal bool instanciated;
        internal bool destroyed;

        internal List<Component> Components = new List<Component>();
        public IEnumerable<Component> GetComponents()
        {
            return Components.AsEnumerable();
        }

        public GraphicsDevice graphics;

        public int DrawOrder { get; set; }

        public bool Enabled { get; set; } = true;

        public int UpdateOrder { get; set; }

        public bool Visible { get; set; } = true;

        public string Name { get; set; } = "GameObject";

        public string Tag { get; set; } = string.Empty;

        public Transform transform
        {
            get { return GetComponent<Transform>(); }
        }

        public Vector3 localPosition
        {
            get { return transform.localPosition; }

            set { transform.localPosition = value; }
        }

        public Vector3 position
        {
            get { return transform.position; }

            set { transform.position = value; }
        }

        public Quaternion localRotation
        {
            get { return transform.localRotation; }

            set { transform.localRotation = value; }
        }

        public Quaternion rotation
        {
            get { return transform.rotation; }

            set { transform.rotation = value; }
        }

        internal uint Id { get; set; }


        internal _gameObject(bool isEmpty)
        {
            Id = (uint)GetHashCode();
        }

        public _gameObject()
        {
            AddComponent<Transform>();
            Id = (uint)GetHashCode();
        }

        public _gameObject(string Name)
        {
            AddComponent<Transform>();
            Id = (uint)GetHashCode();

            this.Name = Name;
        }

        public _gameObject(string Name, params Type[] ComponentTypes)
        {
            AddComponent<Transform>();
            Id = (uint)GetHashCode();

            this.Name = Name;
            foreach (var t in ComponentTypes)
                AddComponent(t);
        }

        public bool KnowsMessage(string message)
        {
            return Components.Find(c => c.KnowsMessage(message)) != null;
        }

        public void BroadcastMessage(string Name, params object[] parameters)
        {
            foreach (var comp in Components.FindAll(c => c.KnowsMessage(Name)))
                comp.SendMessage(Name, parameters);
        }

        public void Run(GameTime gameTime)
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].Run();
        }

        public void Draw(GameTime gameTime)
        {
            BroadcastMessage("Draw", new object[] { gameTime });
        }

        public T AddComponent<T>()
            where T : Component
        {
            Component comp = Activator.CreateInstance<T>();
            if (instanciated)
                comp.SendMessage("Awake");
            Components.Add(comp);
            comp.gameObject = this;

            return comp as T;
        }

        public void AddComponent(Component c)
        {
            Components.Add(c);
            c.gameObject = this;
        }

        public object AddComponent(Type t)
        {
            Component comp = Activator.CreateInstance(t) as Component;
            Components.Add(comp);
            comp.gameObject = this;

            return comp;
        }

        public T GetComponent<T>()
             where T : Component
        {
            return Components.Find(c => c is T) as T;
        }
        public List<T> GetComponents<T>()
            where T : Component
        {
            return Components.FindAll(c => c is T) as List<T>;
        }

        public override string ToString()
        {
            return Name;
        }

        public static _gameObject Load(XmlElement node)
        {
            _gameObject obj = new _gameObject(true);
            var nameAttr = node.Attributes["Name"];
            if (nameAttr != null)
                obj.Name = nameAttr.Value;
            var tagAttr = node.Attributes["Tag"];
            if (tagAttr != null)
                obj.Tag = tagAttr.Value;

            var enabledAttribute = node.Attributes["Enabled"];
            if (enabledAttribute != null)
                obj.Enabled = bool.Parse(enabledAttribute.InnerText);
            var visibleAttribute = node.Attributes["Visible"];
            if (visibleAttribute != null)
                obj.Visible = bool.Parse(visibleAttribute.Value);
            var idAttr = node.Attributes["Id"];
            if (idAttr != null)
                obj.Id = uint.Parse(idAttr.Value);

            foreach (XmlElement componentNode in node.ChildNodes)
            {
                var createType = Utill.FindTypeFromAssemblies(componentNode.Name);
                if (createType == null)
                {
                    Debug.LogError("Could not find type {0} in assembly", componentNode.Name);
                    continue;
                }
                Component c = obj.AddComponent(createType) as Component;
                c.LoadXml(componentNode);
            }

            var transform = obj.GetComponent<Transform>();
            if (transform == null)
                obj.AddComponent<Transform>();

            return obj;
        }

        public static void Destroy(_gameObject @object)
        {
            while (@object.Components.Count > 0)
                Component.Destroy(@object.Components[0]);
            @object.destroyed = true;
        }
    }
}
