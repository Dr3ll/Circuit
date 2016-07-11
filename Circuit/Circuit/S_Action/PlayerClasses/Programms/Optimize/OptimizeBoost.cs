using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms.Optimize
{
    class OptimizeBoost : Boost
    {
        public OptimizeBoost()
        {
            base.mPower = Data.OPT_BOOSTPOWER;
        }
    }
}
