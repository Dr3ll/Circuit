using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms.Delete
{
    class DeleteBoost : Boost
    {
        public DeleteBoost()
        {
            base.mPower = Data.DEL_BOOSTPOWER;
        }
    }
}
