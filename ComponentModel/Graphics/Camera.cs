using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace FPX
{
    [Editor(typeof(CameraEditor))]
    public class Camera : Component
    {
        public static Camera Active { get; set; }

        public Mode cameraMode;

        /// <summary>
        /// Width of the camera viewport in Orthographic mode
        /// </summary>
        public float W = 1.0f;
        private float Width
        {
            get { return Screen.Width * W; }
        }
        /// <summary>
        /// Height of the camera in Orthographic mode
        /// </summary>
        public float H = 1.0f;
        private float Height
        {
            get { return Screen.Height * H; }
        }

        public float fieldOfView = 60.0f;
        public float nearPlaneDistance = 0.01f;
        public float farPlaneDistance = 1000.0f;
        public float aspectRatio
        {
            get { return GameCore.viewport.Width / (float)GameCore.viewport.Height; }
        }

        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        public Matrix ViewMatrix
        {
            get { return cameraMode == Mode.Perspective ? Matrix.CreateLookAt(transform.position, transform.position + transform.worldPose.Forward, transform.worldPose.Up) :
                    Matrix.Identity; }
        }

        public Matrix ProjectionMatrix
        {
            get { return cameraMode == Mode.Perspective ? Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, nearPlaneDistance, farPlaneDistance)
                    : Matrix.CreateOrthographicOffCenter(position.X, position.X + Width, position.Y + Height, position.Y, nearPlaneDistance, farPlaneDistance); }
        }

        Vector3 startPosition;
        Quaternion startRotation;

        public void Start()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        public void Reset()
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        public override void LoadXml(XmlElement node)
        {
            base.LoadXml(node);

            var fieldOfViewNode = node.SelectSingleNode("FieldOfView");
            var nearPlaneDistanceNode = node.SelectSingleNode("NearPlaneDistance");
            var farPlaneDistanceNode = node.SelectSingleNode("FarPlaneDistance");
            var clearColorNode = node.SelectSingleNode("ClearColor") as XmlElement;
            var modeNode = node.SelectSingleNode("Mode");
            var wNode = node.SelectSingleNode("W");
            var hNode = node.SelectSingleNode("H");

            if (fieldOfViewNode != null)
                fieldOfView = float.Parse(fieldOfViewNode.InnerText);
            if (nearPlaneDistanceNode != null)
                nearPlaneDistance = float.Parse(nearPlaneDistanceNode.InnerText);
            if (farPlaneDistanceNode != null)
                farPlaneDistance = float.Parse(farPlaneDistanceNode.InnerText);
            if (clearColorNode != null)
                ClearColor = LinearAlgebraUtil.ColorFromXml(clearColorNode);
            if (modeNode != null)
                cameraMode = (Mode)Enum.Parse(typeof(Mode), modeNode.InnerText);
            if (wNode != null)
                W = float.Parse(wNode.InnerText);
            if (hNode != null)
                H = float.Parse(hNode.InnerText);
        }

        public void SaveXml(XmlElement node)
        {
            var fieldOfViewNode = node.OwnerDocument.CreateElement("FieldOfView");
            var nearPlaneNode = node.OwnerDocument.CreateElement("NearPlaneDistance");
            var farPlaneNode = node.OwnerDocument.CreateElement("FarPlaneDistance");
            var clearColorNode = LinearAlgebraUtil.ColorToXml(node.OwnerDocument, "ClearColor", ClearColor);

            fieldOfViewNode.InnerText = fieldOfView.ToString();
            nearPlaneNode.InnerText = nearPlaneDistance.ToString();
            farPlaneNode.InnerText = farPlaneDistance.ToString();

            node.AppendChild(fieldOfViewNode);
            node.AppendChild(nearPlaneNode);
            node.AppendChild(farPlaneNode);
            node.AppendChild(clearColorNode);
        }

        public Ray ScreenPointToRay(Vector2 scrPos)
        {
            scrPos.X = (scrPos.X - GameCore.viewport.Width / 2.0f) / (GameCore.viewport.Width / 2.0f) * 2.0f;
            scrPos.Y = (scrPos.Y - GameCore.viewport.Height / 2.0f) / (GameCore.viewport.Height / -2.0f) * 2.0f;
            var invViewProj = Matrix.Invert(ProjectionMatrix * ViewMatrix);
            var near = Vector3.Transform(new Vector3(scrPos, nearPlaneDistance), invViewProj);
            var far = Vector3.Transform(new Vector3(scrPos, farPlaneDistance), invViewProj);
            var camDir = far - near;
            camDir.Normalize();

            return new Ray(near, camDir);
        }

        public enum Mode
        {
            Perspective,
            Orthographic
        }
    }
}
