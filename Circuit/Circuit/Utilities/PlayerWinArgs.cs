using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities
{
    class PlayerWinArgs:EventArgs
    {
        PlayerIndex mIndex;

        public PlayerIndex Index
        {
            get { return mIndex; }
        }

        public PlayerWinArgs(PlayerIndex _index)
        {
            _index = mIndex;
        }
    }
}
