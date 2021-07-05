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

        public override void Raycast(Vector3 point)
        {
            var rt = transform as RectTransform;
        }

        private void raycast(RectTransform leaf, Vector3 point)
        {

        }
    }
}
