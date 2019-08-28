using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ComponentModel;

namespace ComponentModel
{
    public class Physics : IGameComponent, IUpdateable
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

            // Update previous positions
            foreach (var collider in colliders)
            {
                if (previousPositions.ContainsKey(collider))
                    previousPositions[collider] = collider.Location;
                else
                    previousPositions.Add(collider, collider.position);
            }

            // move objects
            foreach (var rBody in Rigidbodies)
                UpdateRigidbody(rBody, gameTime);

        }

        private void UpdateRigidbody(Rigidbody rBody, GameTime gameTime)
        {
            if (rBody.isKinematic || (rBody.acceleration.Length() == 0.0f && rBody.velocity.Length() == 0.0f))
                return;

            rBody.acceleration -= rBody.velocity * rBody.drag * (float)gameTime.ElapsedGameTime.TotalSeconds;
            rBody.torque -= rBody.angularVelocity * rBody.angularDrag * (float)gameTime.ElapsedGameTime.TotalSeconds;

            rBody.velocity += rBody.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            rBody.transform.position += rBody.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            rBody.angularVelocity += rBody.torque * (float)gameTime.ElapsedGameTime.TotalSeconds;
            rBody.transform.rotation *= Quaternion.CreateFromYawPitchRoll(rBody.angularVelocity.Y, rBody.angularVelocity.X, rBody.angularVelocity.Z);

            if (LinearAlgebraUtil.isEpsilon(rBody.acceleration))
                rBody.acceleration = Vector3.Zero;
            if (LinearAlgebraUtil.isEpsilon(rBody.velocity))
                rBody.velocity = Vector3.Zero;
        }

        private void DetectCollisions(ref List<Collision> collisions)
        {
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                Collider a = colliders[i];
                for (int ii = i + 1; ii < colliders.Count; ii++)
                {
                    Collider b = colliders[ii];
                    if (Collide(a, b))
                        collisions.Add(new Collision(a, b));
                }
            }
        }

        private void UpdateCollisions()
        {
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                var a = colliders[i];
                for (int ii = i + 1; ii < colliders.Count; ii++)
                {
                    var b = colliders[ii];

                    Collide(a, b);
                }
            }


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

        private bool Collide(Collider a, Collider b)
        {
            if (previousPositions.Count == 0)
                return false;

            if (a is SphereCollider && b is SphereCollider)
            {
                bool value = SphereToSphere(a as SphereCollider, b as SphereCollider);
                SendColisionMessages(a, b, value);
                if (value)
                    return true;
            }
            if (a is BoxCollider && b is BoxCollider)
            {
                bool value = BoxToBox(a as BoxCollider, b as BoxCollider);
                SendColisionMessages(a, b, value);
                if (value)
                    return true;
            }
            if (a is BoxCollider && b is SphereCollider)
            {
                bool value = BoxToSphere(a as BoxCollider, b as SphereCollider);
                SendColisionMessages(a, b, value);
                if (value)
                    return true;
            }
            if (a is SphereCollider && b is BoxCollider)
            {
                bool value = SphereToBox(a as SphereCollider, b as BoxCollider);
                SendColisionMessages(a, b, value);
                if (value)
                    return true;
            }

            return false;
        }

        #region Collision 

        private bool SphereToSphere(SphereCollider a, SphereCollider b)
        {
            return Vector3.Distance(a.Location, b.localPosition) < a.radius + b.radius;
        }

        private bool SphereToBox(SphereCollider a, BoxCollider b)
        {
            var closestPoint = b.ClosestPoint(a.Location);

            Vector3 L = closestPoint - a.Location;
            if (Vector3.Dot(L, L) <= a.radius * a.radius)
                return true;

            return false;
        }

        private bool BoxToSphere(BoxCollider a, SphereCollider b)
        {
            return SphereToBox(b, a);
        }

        private bool BoxToBox(BoxCollider a, BoxCollider b)
        {
            float ra, rb;
            Matrix R = Matrix.Identity,
                AbsR = Matrix.Identity;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    R = LinearAlgebraUtil.SetRowColumn(R, i, j, Vector3.Dot(a.transform.worldPose.GetRow(i).ToVector3(), b.transform.worldPose.GetRow(j).ToVector3()));

            Vector3 t = b.Location - a.Location;
            t = new Vector3(Vector3.Dot(t, a.transform.worldPose.GetRow(0).ToVector3()),
                Vector3.Dot(t, a.transform.worldPose.GetRow(1).ToVector3()),
                Vector3.Dot(t, a.transform.worldPose.GetRow(2).ToVector3()));

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    AbsR = LinearAlgebraUtil.SetRowColumn(AbsR, i, j, Math.Abs(R.GetRowColumn(i, j)) + float.Epsilon);

            // test axis L = A0, L = A1, L = A2
            for (int i = 0; i < 3; i++)
            {
                ra = LinearAlgebraUtil.GetVectorIndex(a.size, i);
                rb = LinearAlgebraUtil.GetVectorIndex(b.size, 0) * AbsR.GetRowColumn(i, 0) +
                    LinearAlgebraUtil.GetVectorIndex(b.size, 1) * AbsR.GetRowColumn(i, 1) +
                    LinearAlgebraUtil.GetVectorIndex(b.size, 2) * AbsR.GetRowColumn(i, 2);
                if (Math.Abs(LinearAlgebraUtil.GetVectorIndex(t, i)) > ra + rb)
                    return false;
            }

            // test axis L = B0, L = B1, L = B2
            for (int i = 0; i < 3; i++)
            {
                ra = a.size.GetIndex(0) * AbsR.GetRowColumn(0, i) + a.size.GetIndex(1) * AbsR.GetRowColumn(1, i) + a.size.GetIndex(2) * AbsR.GetRowColumn(2, i);
                rb = b.size.GetIndex(i);

                if ((Math.Abs(t.GetIndex(0) * R.GetRowColumn(0, i)) + t.GetIndex(1) * R.GetRowColumn(1, i) + t.GetIndex(2) * R.GetRowColumn(2, i)) > ra + rb)
                    return false;
            }

            // Test axis L = A0 x B0
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 0) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(0, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(0, 1);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 0) - t.GetIndex(1) * R.GetRowColumn(2, 0)) > ra + rb)
                return false;

            // Test axis L = A0 x B1
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 1) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(0, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(0, 0);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 1) - t.GetIndex(1) * R.GetRowColumn(2, 1)) > ra + rb)
                return false;

            // Test axis L = A0 x B2
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(0, 1) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 0);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 2) - t.GetIndex(1) * R.GetRowColumn(2, 2)) > ra + rb)
                return false;

            // Test axis L = A1 x B0
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 0) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(1, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 1);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 0) - t.GetIndex(2) * R.GetRowColumn(0, 0)) > ra + rb)
                return false;

            // Test axis L = A1 x B1
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 1) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(1, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(1, 0);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 1) - t.GetIndex(2) * R.GetRowColumn(0, 1)) > ra + rb)
                return false;

            // Test axis L = A1 x B2
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(1, 1) + b.size.GetIndex(1) * AbsR.GetRowColumn(1, 0);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 2) - t.GetIndex(2) * R.GetRowColumn(0, 2)) > ra + rb)
                return false;

            // Test axis L = A2 x B0
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 0) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(2, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(2, 1);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 0) - t.GetIndex(0) * R.GetRowColumn(1, 0)) > ra + rb)
                return false;

            // Test axis L = A2 x B1
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 1) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(2, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(2, 0);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 1) - t.GetIndex(0) * R.GetRowColumn(1, 1)) > ra + rb)
                return false;

            // Test axis L = A2 x B2
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 2) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(2, 1) + b.size.GetIndex(1) * AbsR.GetRowColumn(2, 0);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 2) - t.GetIndex(0) * R.GetRowColumn(1, 2)) > ra + rb)
                return false;

            return true;
            if (activeCollisions.Find(activeCollision => activeCollision.colliders.Contains(a) || activeCollision.colliders.Contains(b)) == null)
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


        #endregion

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

        private class Collision
        {
            public IEnumerable<Collider> colliders
            {
                get
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                            yield return a;
                        else
                            yield return b;
                    }
                }
            }

            Collider a;
            Collider b;

            public Collision(Collider a, Collider b)
            {
                this.a = a;
                this.b = b;

            }
        }
    }
}
