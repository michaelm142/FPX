﻿using System;
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
                for (int ii = 0; ii < colliders.Count; ii++)
                {
                    if (i == ii) continue;

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

        private bool Collide(Collider a, Collider b)
        {
            if (previousPositions.Count == 0)
                return false;

            if (a is SphereCollider && b is SphereCollider)
            {
                bool value = SphereToSphere(a as SphereCollider, b as SphereCollider);
                SendColisionMessages(a, b, value);
                return value;
            }
            if (a is BoxCollider && b is BoxCollider)
            {
                bool value = BoxToBox(a as BoxCollider, b as BoxCollider);
                SendColisionMessages(a, b, value);
                return value;
            }
            if (a is BoxCollider && b is SphereCollider)
            {
                bool value = BoxToSphere(a as BoxCollider, b as SphereCollider);
                SendColisionMessages(a, b, value);
                return value;
            }
            if (a is SphereCollider && b is BoxCollider)
            {
                bool value = SphereToBox(a as SphereCollider, b as BoxCollider);
                SendColisionMessages(a, b, value);
                return value;
            }

            return false;
        }

        #region Collision Detection

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

        #region Collision Resolution

        private void ResolveCollision(Collision collision)
        {
            var a = collision.colliders.ToList()[0];
            var b = collision.colliders.ToList()[1];

            if (a is SphereCollider && b is SphereCollider)
                ResoveSphereToSphere(a as SphereCollider, b as SphereCollider);
        }

        private float AngleBetweenVectors(Vector3 u, Vector3 v)
        {
            double d = (double)u.Length();
            float value = MathHelper.ToDegrees((float)Math.Acos((double)(Vector3.Dot(u, v)) / (double)(u.Length() * v.Length())));
            if (LinearAlgebraUtil.isEpsilon(value))
                return 0.0f;

            return value;
        }

        private void ResoveSphereToSphere(SphereCollider a, SphereCollider b)
        {
            float penetratingRadius = (a.radius + b.radius) - Vector3.Distance(a.position, b.position);
            Vector3 L = b.position - a.position;
            L.Normalize();

            a.position -= L * penetratingRadius;
            b.position += L * penetratingRadius;

            Vector3 Ra = L * a.radius;
            Vector3 Rb = L * b.radius;

            Vector3 nPrime = Vector3.Cross(Vector3.Up, L);
            Vector3 ContactNormal = Vector3.Cross(L, nPrime);

            var bodyA = a.GetComponent<Rigidbody>();
            var bodyB = b.GetComponent<Rigidbody>();

            var accelerationA = Vector3.Reflect(bodyB.LinearMomentum, ContactNormal);
            var accelerationB = Vector3.Reflect(bodyA.LinearMomentum, ContactNormal);

            bodyA.LinearMomentum = accelerationA;
            bodyB.LinearMomentum = accelerationB;

            Matrix contactTensorA = Matrix.Identity;
            Matrix velocityTensorA = Matrix.Identity;

            contactTensorA.Forward = Ra;
            contactTensorA.Up = ContactNormal;
            contactTensorA.Right = Vector3.Cross(contactTensorA.Up, contactTensorA.Forward).Normalized();

            velocityTensorA.Forward = bodyA.velocity;
            velocityTensorA.Right = Vector3.Cross(Vector3.Up, bodyA.velocity).Normalized();
            velocityTensorA.Up = Vector3.Cross(velocityTensorA.Forward, velocityTensorA.Right).Normalized();

            float yawA = Vector3.Dot(contactTensorA.Right, velocityTensorA.Forward);
            float pitchA = Vector3.Dot(contactTensorA.Forward, velocityTensorA.Up);
            float rollA = Vector3.Dot(contactTensorA.Up, velocityTensorA.Right);

            bodyA.AngularMomentum += new Vector3(pitchA, yawA, rollA);
            Debug.Log("Yaw: {0} Pitch: {1} Roll: {2}", yawA, pitchA, rollA);

            Matrix contactTensorB = Matrix.Identity;
            Matrix velocityTensorB = Matrix.Identity;

            contactTensorB.Forward = Rb;
            contactTensorB.Up = ContactNormal;
            contactTensorB.Right = Vector3.Cross(contactTensorB.Up, contactTensorB.Forward).Normalized();

            velocityTensorB.Forward = bodyB.velocity;
            velocityTensorB.Right = Vector3.Cross(Vector3.Up, bodyB.velocity).Normalized();
            velocityTensorB.Up = Vector3.Cross(velocityTensorB.Forward, velocityTensorB.Right).Normalized();

            float yawB = Vector3.Dot(   velocityTensorB.Right,      contactTensorB.Forward);
            float pitchB = Vector3.Dot( velocityTensorB.Forward,    contactTensorB.Up);
            float rollB = Vector3.Dot(  velocityTensorB.Up,         contactTensorB.Right);

            bodyB.AngularMomentum += new Vector3(pitchB, yawB, rollB);
            Debug.Log("Yaw: {0} Pitch: {1} Roll: {2}", yawB, pitchB, rollB);
        }

        #endregion
    }
}
