using System;
using System.Xml;
using Microsoft.Xna.Framework;

namespace FPX
{
    public class Rigidbody : Component
    {
        public Vector3 velocity = Vector3.Zero;
        public Vector3 acceleration = Vector3.Zero;
        public Vector3 torque = Vector3.Zero;
        public Vector3 angularVelocity = Vector3.Zero;
        private Vector3 startPosition;
        private Vector3 startRotation;

        public float mass = 1.0F;
        public float drag = 0.0F;
        public float angularDrag = 0.0f;

        public bool isKinematic { get; set; }

        public Collider collider
        {
            get { return GetComponent<Collider>(); }
        }

        public void Start()
        {
            startPosition = transform.position;
            startRotation = transform.rotation.GetEulerAngles();
        }

        public void Update(GameTime gameTime)
        {
            if (isKinematic)
            {
                transform.position = startPosition;
                transform.rotation.SetEulerAngles(startRotation);
            }
            //    if (isKinematic || (acceleration.Length() == 0.0f && velocity.Length() == 0.0f))
            //        return;

            //    acceleration -= velocity * drag * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    torque -= angularVelocity * angularDrag * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //    velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    transform.position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    angularVelocity += torque * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    transform.rotation *= Quaternion.CreateFromYawPitchRoll(angularVelocity.Y, angularVelocity.X, angularVelocity.Z);

            //    if (LinearAlgebraUtil.isEpsilon(acceleration))
            //        acceleration = Vector3.Zero;
            //    if (LinearAlgebraUtil.isEpsilon(velocity))
            //        velocity = Vector3.Zero;
        }

        public void OnCollisionEnter(Collider other)
        {

        }

        public void AddForce(Vector3 force)
        {
            acceleration += force / mass;
        }

        public void AddTorque(Vector3 torque)
        {
            this.torque += torque / mass;
        }

        public void AddRelitiveTorque(Vector3 torque)
        {
            Vector3 N = Vector3.Cross(transform.forward, Vector3.Forward).Normalized();

            float phi = MathHelper.ToRadians(LinearAlgebraUtil.AngleBetween(transform.up * torque.Y, Vector3.Up));
            float psi = MathHelper.ToRadians(LinearAlgebraUtil.AngleBetween(transform.right, N));
            float theta = MathHelper.ToRadians(LinearAlgebraUtil.AngleBetween(Vector3.Right, N));

            this.torque += (float)(phi * Math.Cos(psi) + theta * Math.Sin(psi) * Math.Sin(phi)) * transform.right * torque.X;
            this.torque += (float)(-phi * Math.Sin(psi) + theta * Math.Cos(psi) * Math.Sin(phi)) * transform.up * torque.Y;
            this.torque += (float)(phi + theta * Math.Cos(phi)) * transform.forward * torque.Z;
        }

        public void Reset()
        {
            velocity = Vector3.Zero;
            acceleration = Vector3.Zero;
            torque = Vector3.Zero;
            angularVelocity = Vector3.Zero;
        }

        public override void LoadXml(XmlElement element)
        {
            var velocityNode = element.SelectSingleNode("Velocity") as XmlElement;
            var torqueNode = element.SelectSingleNode("Torque") as XmlElement;
            var angularDragNode = element.SelectSingleNode("AngularDrag");
            var massNode = element.SelectSingleNode("Mass") as XmlElement;
            var dragNode = element.SelectSingleNode("Drag") as XmlElement;
            var kinematicNode = element.SelectSingleNode("IsKinematic") as XmlElement;

            velocity = LinearAlgebraUtil.Vector3FromXml(velocityNode);
            torque = LinearAlgebraUtil.Vector3FromXml(torqueNode);

            if (massNode != null)
                mass = float.Parse(massNode.InnerText);
            if (dragNode != null)
                drag = float.Parse(dragNode.InnerText);
            if (angularDragNode != null)
                angularDrag = float.Parse(angularDragNode.InnerText);
            if (kinematicNode != null)
                isKinematic = bool.Parse(kinematicNode.InnerText);
        }

        public void SaveXml(XmlElement element)
        {
            var velocityNode = LinearAlgebraUtil.Vector3ToXml(element.OwnerDocument, "Velocity", velocity);
            var torqueNode = LinearAlgebraUtil.Vector3ToXml(element.OwnerDocument, "Torque", torque);
            var angularDragNode = element.OwnerDocument.CreateElement("AngularDrag");
            var massNode = element.OwnerDocument.CreateElement("Mass");
            var dragNode = element.OwnerDocument.CreateElement("Drag");
            var kinimaticNode = element.OwnerDocument.CreateElement("IsKinematic");

            angularDragNode.Value = angularDrag.ToString();
            massNode.Value = mass.ToString();
            dragNode.Value = drag.ToString();
            kinimaticNode.Value = isKinematic.ToString();

            element.AppendChild(velocityNode);
            element.AppendChild(torqueNode);
            element.AppendChild(angularDragNode);
            element.AppendChild(massNode);
            element.AppendChild(dragNode);
            element.AppendChild(kinimaticNode);
        }
    }
}
