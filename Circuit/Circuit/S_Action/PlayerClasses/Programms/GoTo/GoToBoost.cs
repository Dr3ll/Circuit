using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToBoost : Boost
    {
        public GoToBoost()
        {
            base.mPower = Data.GOTO_BOOSTPOWER;
        }
    }
}
