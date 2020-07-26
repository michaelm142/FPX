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
                if (c != null)
                    Debug.Log("PsudoDistance: {0}", c.Psudodistance);
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

        private void ResolveCollision(Collision collision)
        {
            foreach (var c in collision.colliders)
                if (c.GetComponent<Rigidbody>() == null)
                    return;

            Collider a = collision[0];
            Collider b = collision[1];

            Vector3 closestPointA = a.ClosestPoint(b.position);
            Vector3 closestPointB = b.ClosestPoint(closestPointA);

            Vector3 L_a = closestPointB - a.position;
            Vector3 L_b = closestPointA - b.position;

            var bodyA = a.GetComponent<Rigidbody>();
            var bodyB = b.GetComponent<Rigidbody>();

            if (bodyA == null || bodyB == null)
                return;

            for (int i = 0; i < MaxPhysIterations; i++)
            {
                Vector3 psudoDistance = Psudodistance(a, b);
                Debug.Log("Iteration {0}, Psudodistance: {1}", i, psudoDistance);
                Vector3 offset = (a.Psudosize + b.Psudosize) - psudoDistance;
                bodyA.position += offset * Time.deltaTime;
                bodyB.position -= offset * Time.deltaTime;
            }

            //bodyA.position -= closestPointA - closestPointB;
            //bodyB.position += closestPointB - closestPointA;

            var velocityA = Vector3.Reflect(bodyB.velocity, collision.ContactNormal);
            var velocityB = Vector3.Reflect(bodyA.velocity, collision.ContactNormal);
            Debug.Log("Contact Normal: {0}", collision.ContactNormal);

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

        #region Collision Detection

        private Vector3 Psudodistance(Collider colliderA, Collider colliderB)
        {
            var bodyA = colliderA.GetComponent<Rigidbody>();
            var bodyB = colliderB.GetComponent<Rigidbody>();

            var boxA_dotRight = Vector3.Dot(colliderA.transform.right * colliderA.Psudosize.X, Vector3.Right);
            var boxA_dotUp = Vector3.Dot(colliderA.transform.up * colliderA.Psudosize.Y, Vector3.Up);
            var boxA_dotFront = Vector3.Dot(colliderA.transform.forward * colliderA.Psudosize.Z, Vector3.Forward);

            var boxB_dotRight = Vector3.Dot(colliderB.transform.right * colliderA.Psudosize.X, Vector3.Right);
            var boxB_dotUp = Vector3.Dot(colliderB.transform.up * colliderB.Psudosize.Y, Vector3.Up);
            var boxB_dotFront = Vector3.Dot(colliderB.transform.forward * colliderB.Psudosize.Z, Vector3.Forward);

            var boxA_maxRight = colliderA.Location + Vector3.Right * boxA_dotRight;
            var boxA_minRight = colliderA.Location - Vector3.Right * boxA_dotRight;

            var boxA_maxUp = colliderA.Location + Vector3.Up * boxA_dotUp;
            var boxA_minUp = colliderA.Location - Vector3.Up * boxA_dotUp;

            var boxA_maxFront = colliderA.Location + Vector3.Forward * boxA_dotFront;
            var boxA_minFront = colliderA.Location - Vector3.Forward * boxA_dotFront;

            var boxB_maxRight = colliderB.Location + Vector3.Right * boxB_dotRight;
            var boxB_minRight = colliderB.Location - Vector3.Right * boxB_dotRight;

            var boxB_maxUp = colliderB.Location + Vector3.Up * boxB_dotUp;
            var boxB_minUp = colliderB.Location - Vector3.Up * boxB_dotUp;

            var boxB_maxFront = colliderB.Location + Vector3.Forward * boxB_dotFront;
            var boxB_minFront = colliderB.Location - Vector3.Forward * boxB_dotFront;

            var psudodistanceX = Psudodistance(boxA_minRight.X, boxA_maxRight.X, boxB_minRight.X, boxB_maxRight.X, bodyA.velocity.X, bodyB.velocity.X, Time.deltaTime);
            var psudodistanceY = Psudodistance(boxA_minUp.Y, boxA_maxUp.Y, boxB_minUp.Y, boxB_maxUp.Y, bodyA.velocity.Y, bodyB.velocity.Y, Time.deltaTime);
            var psudodistanceZ = Psudodistance(boxA_minFront.Z, boxA_maxFront.Z, boxB_minFront.Z, boxB_maxFront.Z, bodyA.velocity.Z, bodyB.velocity.Z, Time.deltaTime);

            return Vector3.Right * psudodistanceX + Vector3.Up * psudodistanceY + Vector3.Forward * psudodistanceZ;
        }

        private float Psudodistance(float p0, float p1, float q0, float q1, float u, float v, float t)
        {
            return (u - v) * (u - v) * (t * t) + 2 * (u - v) * ((p0 - p1) / 2.0F - (q1 + q0) / 2.0F) * t + (p0 - q1) * (p1 - q0);
        }

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

            collision.Psudodistance = Psudodistance(a, b);

            return collision;
        }

        private Collision SphereToBox(SphereCollider a, BoxCollider b)
        {
            if (MathHelper.Distance(a.Psudosize.Length(), b.Psudosize.Length()) > a.Psudosize.Length() + b.Psudosize.Length())
                return null;

            var closestPoint = b.ClosestPoint(a.Location);

            Vector3 L = closestPoint - a.Location;
            if (Vector3.Dot(L, L) <= a.radius * a.radius)
            {
                Collision c = new Collision(a, b);
                c.L = L;
                c.ContactNormal = L.Normalized();
                c.Psudodistance = Psudodistance(a, b);
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
            if (MathHelper.Distance(a.Psudosize.Length(), b.Psudosize.Length()) > a.Psudosize.Length() + b.Psudosize.Length())
                return null;

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

            c.ContactNormal = normalA;// ((normalA + normalB) * 0.5f).Normalized();
            if (a.GetComponent<Rigidbody>() != null && b.GetComponent<Rigidbody>() != null)
                c.Psudodistance = Psudodistance(a, b);

            return c;
        }


        #endregion
    }
}
