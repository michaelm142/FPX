using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPX
{
    public class UIRaycaster : Raycaster
    {
        public void Start()
        {
            if (GetComponent<RectTransform>() == null)
                throw new InvalidOperationException("Raycaster does not have RectTransform");
        }

        public override bool Raycast(Vector3 point, out RectTransform hit)
        {
            var rt = transform as RectTransform;
            List<RectTransform> hits = new List<RectTransform>();
            if (raycast(rt, point, ref hits))
            {
                hit = hits[hits.Count - 1];
                return true;
            }

            hit = null;
            return false;
        }

        private bool raycast(RectTransform leaf, Vector3 point, ref List<RectTransform> hit)
        {
            if (leaf == null)
                return false;

            if (leaf.rect.Contains(point.ToVector2()))
            {
                hit.Add(leaf);
                foreach (var t in leaf)
                {
                    if (raycast(t as RectTransform, point, ref hit))
                        return true;
                }
            }

            return false;
        }
    }
}
