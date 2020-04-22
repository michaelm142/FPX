using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FPX;

namespace FPX
{
    public partial class Physics : IGameComponent, IUpdateable
    {
        public bool Enabled { get; set; } = true;

        public int UpdateOrder { get; set; } = 100;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private Dictionary<Collider, Vector3> previousPositions = new Dictionary<Collider, Vector3>();

        IEnumerable<GameObject> colliderObjects
        {
            get { return GameCore.currentLevel.Objects.ToList().FindAll(o => o.GetComponent<Collider>() != null); }
        }

        IEnumerable<GameObject> rigidbodyObjects
        {
            get { return GameCore.currentLevel.Objects.ToList().FindAll(o => o.GetComponent<Rigidbody>() != null); }
        }

        IEnumerable<Collider> colliderEnumerable
        {
            get
            {
                foreach (var gameObject in colliderObjects)
                    yield return gameObject.GetComponent<Collider>();
            }
        }

        IEnumerable<Rigidbody> Rigidbodies
        {
            get
            {
                foreach (var gameObject in rigidbodyObjects)
                    yield return gameObject.GetComponent<Rigidbody>();
            }
        }

        List<Collider> colliders
        {
            get { return colliderEnumerable.ToList(); }
        }

        List<Collision> activeCollisions = new List<Collision>();

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime)
        {
            // detect and cull collisions
            List<Collision> collisions = new List<Collision>();
            DetectCollisions(ref collisions);

            // resolve collisions and update physics state
            collisions.ForEach(c => ResolveCollision(c));

            // Update previous positions
            foreach (var collider in colliders)
            {
                if (previousPositions.ContainsKey(collider))
                    previousPositions[collider] = collider.Location;
                else
                    previousPositions.Add(collider, collider.Location);
            }

            // move objects
            foreach (var rBody in Rigidbodies)
                UpdateRigidbody(rBody);

        }

        private void UpdateRigidbody(Rigidbody rBody)
        {
            float t = (float)Time.ElapsedTime.TotalSeconds;
            float dt = Time.deltaTime;
            float halfDT = 0.5f * dt;
            float sixthDT = dt / 6.0f;
            float TpHalfDT = t + halfDT;
            float TpDT = t + dt;
            float mInvMass = 1.0f / rBody.mass;

            Vector3 newPosition, newLinearMomentum, newAngularMomentum,
                newLinearVelocity, newAngularVelocity;
            Quaternion newQuatOrient;
            Matrix newRotOrient;
            Matrix Intertia = Matrix.CreateFromYawPitchRoll(rBody.AngularMomentum.Y, rBody.AngularMomentum.X, rBody.AngularMomentum.Z) * Matrix.CreateTranslation(rBody.LinearMomentum);
            Matrix mInvInertia = Matrix.Invert(Intertia);

            // A1 = G(T,S0), B1 = S0 + (DT/2)*A1
            Vector3 A1DXDT = rBody.velocity;
            Quaternion W = new Quaternion(0.0f, rBody.angularVelocity.X,
                rBody.angularVelocity.Y, rBody.angularVelocity.Z);
            Quaternion A1DQDT = LinearAlgebraUtil.ComponentMultiply(W, 0.5f) * rBody.rotation;

            // account for gravity
            Vector3 A1DPDT = Vector3.Zero;

            Vector3 A1DLDT = Vector3.Zero;

            newPosition = rBody.position + halfDT * A1DXDT;
            newQuatOrient = rBody.rotation + LinearAlgebraUtil.ComponentMultiply(A1DQDT, halfDT);
            newLinearMomentum = rBody.LinearMomentum + halfDT * A1DPDT;
            newAngularMomentum = rBody.AngularMomentum + halfDT * A1DLDT;
            newRotOrient = Matrix.CreateFromQuaternion(newQuatOrient);
            newLinearVelocity = mInvMass * newLinearMomentum;
            newAngularVelocity = Vector3.Transform(newAngularMomentum, newRotOrient * mInvInertia * Matrix.Transpose(newRotOrient));

            // A2 = G(T+DT/2,B1), B2 = S0 + (DT/2)*A2
            Vector3 A2DXDT = newLinearVelocity;
            W = new Quaternion(0.0f, newAngularVelocity.X,
                newAngularVelocity.Y, newAngularVelocity.Z);
            Quaternion A2DQDT = LinearAlgebraUtil.ComponentMultiply(W, 0.5f) * newQuatOrient;

            // account for gravity
            Vector3 A2DPDT = Vector3.Zero;

            Vector3 A2DLDT = Vector3.Zero;

            newPosition = rBody.position + halfDT * A2DXDT;
            newQuatOrient = rBody.rotation + LinearAlgebraUtil.ComponentMultiply(A2DQDT, halfDT);
            newLinearMomentum = rBody.LinearMomentum + halfDT * A2DPDT;
            newAngularMomentum = rBody.AngularMomentum + halfDT * A2DLDT;
            newRotOrient = Matrix.CreateFromQuaternion(newQuatOrient);
            newLinearVelocity = mInvMass * newLinearMomentum;
            newAngularVelocity = Vector3.Transform(newAngularMomentum, newRotOrient * mInvInertia * Matrix.Transpose(newRotOrient));

            // A3 = G(T+DT/2,B2), B3 = S0 + DT*A3
            Vector3 A3DXDT = newLinearVelocity;
            W = new Quaternion(0.0f, newAngularVelocity.X,
                newAngularVelocity.Y, newAngularVelocity.Z);
            Quaternion A3DQDT = LinearAlgebraUtil.ComponentMultiply(W, 0.5f) * newQuatOrient;

            Vector3 A3DPDT = Vector3.Zero;

            Vector3 A3DLDT = Vector3.Zero;

            newPosition = rBody.position + dt * A3DXDT;
            newQuatOrient = rBody.rotation + LinearAlgebraUtil.ComponentMultiply(A3DQDT, dt);
            newLinearMomentum = rBody.LinearMomentum + dt * A3DPDT;
            newAngularMomentum = rBody.AngularMomentum + dt * A3DLDT;
            newRotOrient = Matrix.CreateFromQuaternion(newQuatOrient);
            newLinearVelocity = mInvMass * newLinearMomentum;
            newAngularVelocity = Vector3.Transform(newAngularMomentum, newRotOrient * mInvInertia * Matrix.Transpose(newRotOrient));

            // A4 = G(T+DT,B3), S1 = S0 + (DT/6)*(A1+2*(A2+A3)+A4)
            Vector3 A4DXDT = newLinearVelocity;
            W = new Quaternion(0.5f, newAngularVelocity.X,
                newAngularVelocity.Y, newAngularVelocity.Z);
            Quaternion A4DQDT = LinearAlgebraUtil.ComponentMultiply(W, 0.5f) * newQuatOrient;

            Vector3 A4DPDT = Vector3.Zero;

            Vector3 A4DLDT = Vector3.Zero;

            rBody.position = rBody.position + sixthDT * (A1DXDT +
                2.0f * (A2DXDT + A3DXDT) + A4DXDT);

            rBody.rotation = rBody.rotation * LinearAlgebraUtil.ComponentMultiply(A1DQDT *
                LinearAlgebraUtil.ComponentMultiply(A2DQDT * A3DQDT, 2.0f) * A4DQDT, sixthDT);

            rBody.LinearMomentum = rBody.LinearMomentum + sixthDT * (A1DPDT +
                2.0f * (A2DPDT + A3DPDT) + A4DPDT);

            rBody.AngularMomentum = rBody.AngularMomentum + sixthDT * (A1DLDT +
                2.0f * (A2DLDT + A3DLDT) + A4DLDT);

            Matrix mRotOrient = Matrix.CreateFromQuaternion(rBody.rotation);
            rBody.velocity = mInvMass * rBody.LinearMomentum;
            rBody.angularVelocity = Vector3.Transform(rBody.AngularMomentum, mRotOrient * mInvInertia * Matrix.Transpose(mRotOrient));
        }

        private void SendColisionMessages(Collider a, Collider b, bool value)
        {
            if (value)
            {
                if (activeCollisions.Find(activeCollision => activeCollision.colliders.Contains(a) && activeCollision.colliders.Contains(b)) == null)
                {
                    activeCollisions.Add(new Collision(a, b));
                    a.gameObject.BroadcastMessage("OnCollisionEnter", b);
                    b.gameObject.BroadcastMessage("OnCollisionEnter", a);
                }
                else
                {
                    a.gameObject.BroadcastMessage("OnCollision", b);
                    b.gameObject.BroadcastMessage("OnCollision", a);
                }
            }
            else if (activeCollisions.Find(activeCollision => activeCollision.colliders.Contains(a) || activeCollision.colliders.Contains(b)) != null)
            {
                a.gameObject.BroadcastMessage("OnCollisionExit", b);
                b.gameObject.BroadcastMessage("OnCollisionExit", a);
                activeCollisions.Remove(activeCollisions.Find(activeCollision => activeCollision.colliders.Contains(a) || activeCollision.colliders.Contains(b)));
            }
        }

        public static bool IntersectRaySphere(Ray ray, SphereCollider sphere, out float length, out Vector3 point)
        {
            Vector3 L = ray.Position - sphere.Location;
            float b = Vector3.Dot(L, ray.Direction);
            float c = Vector3.Dot(L, L) - sphere.radius * sphere.radius;

            if (c > 0.0f && b > 0.0f)
            {
                length = 0.0f;
                point = Vector3.Zero;
                return false;
            }

            float discriminant = b * b - c;
            if (discriminant < 0.0f)
            {
                length = 0.0f;
                point = Vector3.Zero;
                return false;
            }

            length = -b - (float)Math.Sqrt(discriminant);
            if (length < 0.0f)
                length = 0.0f;

            point = ray.Position + ray.Direction * length;
            return true;
        }

        public static bool IntersectRayBox(Ray ray, BoxCollider box, out float length, out Vector3 point)
        {
            float tMin = 0.0f;
            float tMax = float.PositiveInfinity;

            Matrix inv = Matrix.Invert(Matrix.CreateFromQuaternion(box.rotation) * Matrix.CreateTranslation(box.position));
            Vector3 localizedRayPosition = Vector3.Transform(ray.Position, inv);
            Vector3 localizedRayDirection = Vector3.TransformNormal(ray.Direction, inv);

            Ray localizedRay = new Ray(localizedRayPosition, localizedRayDirection);

            Vector3 max = box.size;
            Vector3 min = -box.size;

            for (int i = 0; i < 3; i++)
            {
                if (LinearAlgebraUtil.isEpsilon(Math.Abs(localizedRay.Direction.GetIndex(i))))
                {
                    if (localizedRay.Position.GetIndex(i) < min.GetIndex(i) || localizedRay.Position.GetIndex(i) > max.GetIndex(i))
                    {
                        length = 0.0f;
                        point = Vector3.Zero;
                        return false;
                    }
                }

                float ood = 1.0f / localizedRay.Direction.GetIndex(i);
                float t1 = (min.GetIndex(i) - localizedRay.Position.GetIndex(i)) * ood;
                float t2 = (max.GetIndex(i) - localizedRay.Position.GetIndex(i)) * ood;

                if (t1 > t2)
                {
                    float w = t1;
                    t1 = t2;
                    t2 = w;
                }

                tMin = Math.Max(tMin, t1);
                tMax = Math.Min(tMax, t2);

                if (tMin > tMax)
                {
                    length = 0.0f;
                    point = Vector3.Zero;
                    return false;
                }
            }

            point = ray.Position + ray.Direction * tMin;
            length = tMin;

            return true;
        }
    }
}
