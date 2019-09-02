﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComponentModel
{
    public class ComponentEditor : UserControl
    {
        public virtual Component Target { get; protected set; }

        public virtual void UpdateTarget() { }

    }
}
