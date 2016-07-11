using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.S_Action.PlayerClasses
{
    class BoolEventArgs : EventArgs
    {
        internal bool Down;

        internal BoolEventArgs(bool _bool)
            : base()
        {
            Down = _bool;
        }
    }
}
