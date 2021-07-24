using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FPX
{
    public abstract class Raycaster : Component
    {
        public abstract bool Raycast(Vector3 point, out RectTransform hit);
    }
}
