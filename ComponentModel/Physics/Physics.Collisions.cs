using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPX
{
    public partial class Physics
    {

        private void DetectCollisions(ref List<Collision> collisions)
        {
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                Collider a = colliders[i];
                for (int ii = i + 1; ii < colliders.Count; ii++)
                {
                    Collider b = colliders[ii];
                    var collision = Collide(a, b);
                    if (collision != null)
                        collisions.Add(collision);
                }
            }
        }

        private Collision Collide(Collider a, Collider b)
        {
            if (previousPositions.Count == 0)
                return null;

            if (a is SphereCollider && b is SphereCollider)
            {
                Collision c = SphereToSphere(a as SphereCollider, b as SphereCollider);
                SendColisionMessages(a, b, c != null);
                return c;
            }
            if (a is BoxCollider && b is BoxCollider)
            {
                Collision c = BoxToBox(a as BoxCollider, b as BoxCollider);
                SendColisionMessages(a, b, c != null);
                return c;
            }
            if (a is BoxCollider && b is SphereCollider)
            {
                Collision c = BoxToSphere(a as BoxCollider, b as SphereCollider);
                SendColisionMessages(a, b, c != null);
                return c;
            }
            if (a is SphereCollider && b is BoxCollider)
            {
                Collision c = SphereToBox(a as SphereCollider, b as BoxCollider);
                SendColisionMessages(a, b, c != null);
                return c;
            }

            return null;
        }

        #region Collision Detection

        private Collision SphereToSphere(SphereCollider a, SphereCollider b)
        {
            float distance = Vector3.Distance(a.Location, b.Location);
            if (distance > a.radius + b.radius)
                return null;

            Collision collision = new Collision(a, b);
            float penetratingRadius = Vector3.Distance(a.position, b.position) - (a.radius + b.radius);
            Vector3 L = b.position - a.position;
            collision.PenetrationDistance = penetratingRadius;
            L.Normalize();
            collision.L = L;

            Vector3 Ra = L * a.radius;
            Vector3 Rb = L * b.radius;

            Vector3 nPrime = Vector3.Cross(Vector3.Up, L);
            collision.ContactNormal = Vector3.Cross(L, nPrime);

            return collision;
        }

        private Collision SphereToBox(SphereCollider a, BoxCollider b)
        {
            var closestPoint = b.ClosestPoint(a.Location);

            Vector3 L = closestPoint - a.Location;
            if (Vector3.Dot(L, L) <= a.radius * a.radius)
            {
                Collision c = new Collision(a, b);
                return c;
            }

            return null;
        }

        private Collision BoxToSphere(BoxCollider a, SphereCollider b)
        {
            return SphereToBox(b, a);
        }

        private Collision BoxToBox(BoxCollider a, BoxCollider b)
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
                    return null;
            }

            // test axis L = B0, L = B1, L = B2
            for (int i = 0; i < 3; i++)
            {
                ra = a.size.GetIndex(0) * AbsR.GetRowColumn(0, i) + a.size.GetIndex(1) * AbsR.GetRowColumn(1, i) + a.size.GetIndex(2) * AbsR.GetRowColumn(2, i);
                rb = b.size.GetIndex(i);

                if ((Math.Abs(t.GetIndex(0) * R.GetRowColumn(0, i)) + t.GetIndex(1) * R.GetRowColumn(1, i) + t.GetIndex(2) * R.GetRowColumn(2, i)) > ra + rb)
                    return null;
            }

            // Test axis L = A0 x B0
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 0) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(0, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(0, 1);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 0) - t.GetIndex(1) * R.GetRowColumn(2, 0)) > ra + rb)
                return null;

            // Test axis L = A0 x B1
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 1) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(0, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(0, 0);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 1) - t.GetIndex(1) * R.GetRowColumn(2, 1)) > ra + rb)
                return null;

            // Test axis L = A0 x B2
            ra = a.size.GetIndex(1) * AbsR.GetRowColumn(2, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(0, 1) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 0);
            if (Math.Abs(t.GetIndex(2) * R.GetRowColumn(1, 2) - t.GetIndex(1) * R.GetRowColumn(2, 2)) > ra + rb)
                return null;

            // Test axis L = A1 x B0
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 0) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(1, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(1, 1);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 0) - t.GetIndex(2) * R.GetRowColumn(0, 0)) > ra + rb)
                return null;

            // Test axis L = A1 x B1
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 1) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(1, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(1, 0);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 1) - t.GetIndex(2) * R.GetRowColumn(0, 1)) > ra + rb)
                return null;

            // Test axis L = A1 x B2
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(2, 2) + a.size.GetIndex(2) * AbsR.GetRowColumn(0, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(1, 1) + b.size.GetIndex(1) * AbsR.GetRowColumn(1, 0);
            if (Math.Abs(t.GetIndex(0) * R.GetRowColumn(2, 2) - t.GetIndex(2) * R.GetRowColumn(0, 2)) > ra + rb)
                return null;

            // Test axis L = A2 x B0
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 0) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 0);
            rb = b.size.GetIndex(1) * AbsR.GetRowColumn(2, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(2, 1);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 0) - t.GetIndex(0) * R.GetRowColumn(1, 0)) > ra + rb)
                return null;

            // Test axis L = A2 x B1
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 1) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 1);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(2, 2) + b.size.GetIndex(2) * AbsR.GetRowColumn(2, 0);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 1) - t.GetIndex(0) * R.GetRowColumn(1, 1)) > ra + rb)
                return null;

            // Test axis L = A2 x B2
            ra = a.size.GetIndex(0) * AbsR.GetRowColumn(1, 2) + a.size.GetIndex(1) * AbsR.GetRowColumn(0, 2);
            rb = b.size.GetIndex(0) * AbsR.GetRowColumn(2, 1) + b.size.GetIndex(1) * AbsR.GetRowColumn(2, 0);
            if (Math.Abs(t.GetIndex(1) * R.GetRowColumn(0, 2) - t.GetIndex(0) * R.GetRowColumn(1, 2)) > ra + rb)
                return null;

            Collision c = new Collision(a, b);
            Vector3 normalA = Vector3.Zero;
            Vector3 normalB = Vector3.Zero;

            a.ClosestPoint(b.position, out normalA);
            b.ClosestPoint(a.position, out normalB);

            c.ContactNormal = ((normalA + normalB) * 0.5f).Normalized();

            return c;
        }


        #endregion

        #region Collision Resolution

        private void ResolveCollision(Collision collision)
        {
            var a = collision.colliders.ToList()[0];
            var b = collision.colliders.ToList()[1];

            if (a.GetComponent<Rigidbody>() == null || b.GetComponent<Rigidbody>() == null)
                return;

            if (a is SphereCollider && b is SphereCollider)
                ResoveSphereToSphere(collision);
            if (a is BoxCollider && b is BoxCollider)
                ResolveBoxToBox(collision);
        }

        private float AngleBetweenVectors(Vector3 u, Vector3 v)
        {
            double d = (double)u.Length();
            float value = MathHelper.ToDegrees((float)Math.Acos((double)(Vector3.Dot(u, v)) / (double)(u.Length() * v.Length())));
            if (LinearAlgebraUtil.isEpsilon(value))
                return 0.0f;

            return value;
        }

        private void ResoveSphereToSphere(Collision collision)
        {
            SphereCollider a = collision[0] as SphereCollider;
            SphereCollider b = collision[1] as SphereCollider;

            a.position += collision.L *collision.PenetrationDistance;
            b.position -= collision.L * collision.PenetrationDistance;

            var bodyA = a.GetComponent<Rigidbody>();
            var bodyB = b.GetComponent<Rigidbody>();

            if (bodyA == null || bodyB == null)
                return;

            var velocityA = Vector3.Reflect(bodyB.velocity, collision.ContactNormal);
            var velocityB = Vector3.Reflect(bodyA.velocity, collision.ContactNormal);

            bodyA.velocity = velocityA;
            bodyB.velocity = velocityB;

            var accelerationA = Vector3.Reflect(bodyB.acceleration, collision.ContactNormal);
            var accelerationB = Vector3.Reflect(bodyA.acceleration, collision.ContactNormal);

            bodyA.acceleration = accelerationA;
            bodyB.acceleration = accelerationB;

            Matrix contactTensorA = Matrix.Identity;
            Matrix velocityTensorA = Matrix.Identity;

            contactTensorA.Forward = collision.L * a.radius;
            contactTensorA.Up = collision.ContactNormal;
            contactTensorA.Right = Vector3.Cross(contactTensorA.Up, contactTensorA.Forward).Normalized();

            velocityTensorA.Forward = bodyA.velocity;
            velocityTensorA.Right = Vector3.Cross(Vector3.Up, bodyA.velocity).Normalized();
            velocityTensorA.Up = Vector3.Cross(velocityTensorA.Forward, velocityTensorA.Right).Normalized();

            float yawA = Vector3.Dot(contactTensorA.Right, velocityTensorA.Forward);
            float pitchA = Vector3.Dot(contactTensorA.Forward, velocityTensorA.Up);
            float rollA = Vector3.Dot(contactTensorA.Up, velocityTensorA.Right);

            if (float.IsNaN(yawA)) yawA = 0.0f;
            if (float.IsNaN(pitchA)) pitchA = 0.0f;
            if (float.IsNaN(rollA)) rollA = 0.0f;

            bodyA.angularVelocity += new Vector3(pitchA, yawA, rollA);
            Debug.Log("Yaw: {0} Pitch: {1} Roll: {2}", yawA, pitchA, rollA);

            Matrix contactTensorB = Matrix.Identity;
            Matrix velocityTensorB = Matrix.Identity;

            contactTensorB.Forward = collision.L * b.radius;
            contactTensorB.Up = collision.ContactNormal;
            contactTensorB.Right = Vector3.Cross(contactTensorB.Up, contactTensorB.Forward).Normalized();

            velocityTensorB.Forward = bodyB.velocity;
            velocityTensorB.Right = Vector3.Cross(Vector3.Up, bodyB.velocity).Normalized();
            velocityTensorB.Up = Vector3.Cross(velocityTensorB.Forward, velocityTensorB.Right).Normalized();

            float yawB = Vector3.Dot(   velocityTensorB.Right,      contactTensorB.Forward);
            float pitchB = Vector3.Dot( velocityTensorB.Forward,    contactTensorB.Up);
            float rollB = Vector3.Dot(  velocityTensorB.Up,         contactTensorB.Right);

            if (float.IsNaN(yawB)) yawB = 0.0f;
            if (float.IsNaN(pitchB)) pitchB = 0.0f;
            if (float.IsNaN(rollB)) rollB = 0.0f;

            bodyB.angularVelocity += new Vector3(pitchB, yawB, rollB);
            Debug.Log("Yaw: {0} Pitch: {1} Roll: {2}", yawB, pitchB, rollB);
        }

        private void ResolveBoxToBox(Collision collision)
        {
            BoxCollider a = collision[0] as BoxCollider;
            BoxCollider b = collision[1] as BoxCollider;

            Vector3 closestPointA = a.ClosestPoint(b.position);
            Vector3 closestPointB = b.ClosestPoint(closestPointA);

            Vector3 L_a = closestPointB - a.position;
            Vector3 L_b = closestPointA - b.position;

            var bodyA = a.GetComponent<Rigidbody>();
            var bodyB = b.GetComponent<Rigidbody>();

            if (bodyA == null || bodyB == null)
                return;

            bodyA.position -= closestPointA - closestPointB;
            bodyB.position += closestPointB - closestPointA;

            var velocityA = Vector3.Reflect(bodyB.velocity, collision.ContactNormal);
            var velocityB = Vector3.Reflect(bodyA.velocity, collision.ContactNormal);

            bodyA.velocity = velocityA;
            bodyB.velocity = velocityB;

            var accelerationA = Vector3.Reflect(bodyB.acceleration, collision.ContactNormal);
            var accelerationB = Vector3.Reflect(bodyA.acceleration, collision.ContactNormal);

            bodyA.acceleration = accelerationA;
            bodyB.acceleration = accelerationB;

            Matrix contactTensorA = Matrix.Identity;
            Matrix velocityTensorA = Matrix.Identity;

            contactTensorA.Forward = L_a.Normalized();
            contactTensorA.Up = collision.ContactNormal;
            contactTensorA.Right = Vector3.Cross(contactTensorA.Up, contactTensorA.Forward).Normalized();

            velocityTensorA.Forward = bodyA.velocity;
            velocityTensorA.Right = Vector3.Cross(Vector3.Up, bodyA.velocity).Normalized();
            velocityTensorA.Up = Vector3.Cross(velocityTensorA.Forward, velocityTensorA.Right).Normalized();

            float yawA = Vector3.Dot(contactTensorA.Right, velocityTensorA.Forward);
            float pitchA = Vector3.Dot(contactTensorA.Up, velocityTensorA.Forward);
            float rollA = Vector3.Dot(contactTensorA.Up, velocityTensorA.Right);

            if (float.IsNaN(yawA)) yawA = 0.0f;
            if (float.IsNaN(pitchA)) pitchA = 0.0f;
            if (float.IsNaN(rollA)) rollA = 0.0f;

            bodyA.angularVelocity += new Vector3(pitchA, yawA, rollA);
            Debug.Log("{0} Yaw: {1} Pitch: {2} Roll: {3}", bodyA.gameObject.Name, yawA, pitchA, rollA);


            Matrix contactTensorB = Matrix.Identity;
            Matrix velocityTensorB = Matrix.Identity;

            contactTensorB.Forward = L_b.Normalized();
            contactTensorB.Up = collision.ContactNormal;
            contactTensorB.Right = Vector3.Cross(contactTensorB.Up, contactTensorB.Forward).Normalized();

            velocityTensorB.Forward = bodyB.velocity;
            velocityTensorB.Right = Vector3.Cross(Vector3.Up, bodyB.velocity).Normalized();
            velocityTensorB.Up = Vector3.Cross(velocityTensorB.Forward, velocityTensorB.Right).Normalized();

            float yawB = Vector3.Dot(contactTensorB.Right, velocityTensorB.Forward);
            float pitchB = Vector3.Dot(contactTensorB.Up, velocityTensorB.Forward);
            float rollB = Vector3.Dot(contactTensorB.Up, velocityTensorB.Right);

            if (float.IsNaN(yawB)) yawB = 0.0f;
            if (float.IsNaN(pitchB)) pitchB = 0.0f;
            if (float.IsNaN(rollB)) rollB = 0.0f;

            bodyB.angularVelocity += new Vector3(pitchB, yawB, rollB);
            Debug.Log("{0} Yaw: {1} Pitch: {2} Roll: {3}", bodyB.gameObject.Name, yawB, pitchB, rollB);
        }

        #endregion
    }
}
