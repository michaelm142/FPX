﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using FPX.Editor;
using System.Collections;

namespace FPX
{
    [Editor(typeof(TransformEditor))]
    public class Transform : Component, IEnumerable<Transform>
    {
        public new Transform transform
        {
            get { return this; }
        }

        [IgnoreInGUI]
        public new Vector3 position
        {
            get { return GetPosition(parent, localPosition); }

            set { localPosition = parent == null ? value : Vector3.Transform(value, Matrix.CreateTranslation(parent.position)); }
        }

        [IgnoreInGUI]
        public new Quaternion rotation
        {
            get { return GetRotation(parent, localRotation); }

            set { localRotation = parent == null ? value : parent.rotation * value; }
        }

        public new Vector3 localPosition = Vector3.Zero;
        public new Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = Vector3.One;

        public Matrix worldToLocalMatrix
        {
            get { return Matrix.CreateTranslation(-position) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateScale(-localScale); }
        }

        public virtual Matrix localToWorldMatrix
        {
            get { return Matrix.CreateScale(localScale) * Matrix.CreateFromQuaternion(localRotation) * Matrix.CreateTranslation(localPosition); }
        }

        public virtual Matrix worldPose
        {
            get { return Matrix.CreateScale(localScale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position); }

            set
            {
                position = value.Translation;
                rotation = value.Rotation;
                scale = value.Scale;
            }
        }

        public Vector3 up
        {
            get { return worldPose.Up; }

            set
            {
                Matrix m = worldPose;
                m.Up = value;
                worldPose = m;
            }
        }

        public Vector3 right
        {
            get { return worldPose.Right; }

            set
            {
                Matrix m = worldPose;
                m.Right = value;
                worldPose = m;
            }
        }

        public Vector3 forward
        {
            get { return worldPose.Forward; }

            set
            {
                Matrix m = worldPose;
                m.Forward = value;
                worldPose = m;
            }
        }

        private Transform _parent;
        public virtual Transform parent
        {
            get { return _parent; }
            set
            {
                if (value == this)
                    return;

                if (_parent != null && value == null)
                    _parent.childTransforms.Remove(this);
                else if (value != null)
                    value.childTransforms.Add(this);

                _parent = value;
            }
        }

        protected Vector3 GetPosition(Transform parent, Vector3 position)
        {
            if (parent == null)
                return position;

            return GetPosition(parent.parent, Vector3.Transform(position, parent.localToWorldMatrix));
        }

        protected Quaternion GetRotation(Transform parent, Quaternion rotation)
        {
            if (parent == null)
                return rotation;

            return GetRotation(parent.parent, parent.localRotation * rotation);
        }

        private string parentName;

        public override void LoadXml(XmlElement node)
        {
            parentName = node.Attributes["Parent"] == null ? null : node.Attributes["Parent"].Value;
            var idAttr = node.Attributes["Id"];
            if (idAttr != null)
                Id = uint.Parse(idAttr.Value);
            else
                Id = (uint)FindObjectsOfType<Transform>().Count;

            var positionNode = node.SelectSingleNode("Position") as XmlElement;
            var rotationNode = node.SelectSingleNode("Rotation") as XmlElement;
            var scaleNode = node.SelectSingleNode("Scale") as XmlElement;

            if (positionNode != null)
                position = LinearAlgebraUtil.Vector3FromXml(positionNode);
            if (rotationNode != null)
                rotation = LinearAlgebraUtil.EulerFromXml(rotationNode);
            if (scaleNode != null)
                localScale = LinearAlgebraUtil.Vector3FromXml(scaleNode);
        }

        public void SaveXml(XmlElement node)
        {
            if (parent != null)
            {
                XmlAttribute parentAttr = node.OwnerDocument.CreateAttribute("Parent");
                parentAttr.Value = parentName;
                node.Attributes.Append(parentAttr);
            }
            var positionNode = LinearAlgebraUtil.Vector3ToXml(node.OwnerDocument, "Position", localPosition);

            Vector3 eulerRotation = rotation.GetEulerAngles();
            var rotationNode = LinearAlgebraUtil.Vector3ToXml(node.OwnerDocument, "Rotation", eulerRotation);

            var scaleNode = LinearAlgebraUtil.Vector3ToXml(node.OwnerDocument, "Scale", localScale);

            var idAttr = node.OwnerDocument.CreateAttribute("Id");
            idAttr.Value = Id.ToString();
            node.Attributes.Append(idAttr);

            node.AppendChild(positionNode);
            node.AppendChild(rotationNode);
            node.AppendChild(scaleNode);
        }

        public void Start()
        {
            if (!string.IsNullOrEmpty(parentName) && parent == null)
            {
                parent = GameObject.Find(parentName).transform;
                parentName = null;
            }
        }

        private List<Transform> childTransforms = new List<Transform>();
        public Transform this[int index]
        {
            get { return childTransforms[index]; }

            set { childTransforms[index] = value; }
        }

        public Transform GetChild(int index)
        {
            return this[index];
        }

        public Transform Find(string name)
        {
            return childTransforms.Find(t => t.Name == name);
        }

        public List<Transform> FindAll(string name)
        {
            return childTransforms.FindAll(t => t.Name == name);
        }

        public IEnumerator<Transform> GetEnumerator()
        {
            return childTransforms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return childTransforms.GetEnumerator();
        }
    }
}
