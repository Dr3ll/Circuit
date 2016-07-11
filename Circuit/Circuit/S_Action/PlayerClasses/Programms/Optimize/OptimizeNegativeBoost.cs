using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms.Optimize
{
    class OptimizeNegativeBoost : Boost
    {
        public OptimizeNegativeBoost()
        {
            base.mPower = Data.OPT_NEGBOOSTPOWER;
        }
    }
}
