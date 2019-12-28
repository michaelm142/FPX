using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPX.ComponentModel
{
    public class Collision
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
