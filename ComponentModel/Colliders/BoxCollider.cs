using System.Collections;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ComponentModel
{
    public class BoxCollider : Collider
    {
        public Vector3 size;

        public Vector3 Min
        {
            get { return Location - Vector3.Transform(size, rotation); }
        }

        public Vector3 Max
        {
            get { return Location + Vector3.Transform(size, rotation); }
        }

        public override bool Contains(Vector3 point)
        {
            var localPoint = Vector3.Transform(point, Matrix.Invert(transform.worldPose));

            if (localPoint.X < -size.X || localPoint.X > size.X || localPoint.Y < -size.Y || localPoint.Y > size.Y | localPoint.Z < -size.Z || localPoint.Z > size.Z)
                return false;

            return true;
        }

        public void LoadXml(XmlElement node)
        {
            var sizeNode = node.SelectSingleNode("Size") as XmlElement;
            var centerNode = node.SelectSingleNode("Center") as XmlElement;

            size = LinearAlgebraUtil.Vector3FromXml(sizeNode);
            center = LinearAlgebraUtil.Vector3FromXml(centerNode);
        }

        public override Vector3 ClosestPoint(Vector3 point)
        {
            var L = point - Location;
            var dist_x = Vector3.Dot(L, transform.worldPose.Right);
            var dist_y = Vector3.Dot(L, transform.worldPose.Up);
            var dist_z = Vector3.Dot(L, transform.worldPose.Forward);

            //Clamp inside box
            dist_x = MathHelper.Clamp(dist_x, -size.X, size.X);
            dist_y = MathHelper.Clamp(dist_y, -size.Y, size.X);
            dist_z = MathHelper.Clamp(dist_z, -size.Z, size.Z);

            var outval = Location;
            outval += dist_x * transform.worldPose.Right;
            outval += dist_y * transform.worldPose.Up;
            outval += dist_z * transform.worldPose.Forward;

            return outval;
        }

        public override Vector3 ClosestPoint(Vector3 point, out Vector3 normal)
        {
            var L = point - Location;
            var dist_x = Vector3.Dot(L, transform.worldPose.Right);
            var dist_y = Vector3.Dot(L, transform.worldPose.Up);
            var dist_z = Vector3.Dot(L, transform.worldPose.Forward);

            //Clamp inside box
            dist_x = MathHelper.Clamp(dist_x, -size.X, size.X);
            dist_y = MathHelper.Clamp(dist_y, -size.Y, size.X);
            dist_z = MathHelper.Clamp(dist_z, -size.Z, size.Z);

            var outval = Location;
            outval += dist_x * transform.worldPose.Right;
            outval += dist_y * transform.worldPose.Up;
            outval += dist_z * transform.worldPose.Forward;

            normal = Vector3.Zero;

            return outval;
        }
    }
}