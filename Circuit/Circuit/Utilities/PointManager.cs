using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities
{
    class PointManager
    {
        public delegate void EndGameHandler(PointManager _pointManager, PlayerWinArgs e);
        public event EndGameHandler EndGame;

        public int[] mPoints;

        public PointManager()
        {
            mPoints = new int[2];
            mPoints[0] = 0;
            mPoints[1] = 0;
        }

        public void IncreaseScore(PlayerIndex _index)
        {
            if (_index == PlayerIndex.One) mPoints[0]++;
            else mPoints[1]++;

            if (Math.Abs(mPoints[0] - mPoints[1]) >= Data.POINTS_TO_WIN)
            {
                if (mPoints[0] > mPoints[1]) EndGame(this, new PlayerWinArgs(PlayerIndex.One));
                else EndGame(this, new PlayerWinArgs(PlayerIndex.Two));
            }

            //if (mPoints[0] == Data.POINTS_TO_WIN)
            //{
            //    EndGame(this, new PlayerWinArgs(PlayerIndex.One));
            //}

            //if (mPoints[1] == Data.POINTS_TO_WIN)
            //{
            //    EndGame(this, new PlayerWinArgs(PlayerIndex.Two));
            //}
        }

        public void DecreaseScore(PlayerIndex _index)
        {
            if (_index == PlayerIndex.One) mPoints[0]--;
            else mPoints[1]--;

            if (Math.Abs(mPoints[0] - mPoints[1]) >= Data.POINTS_TO_WIN)
            {
                if (mPoints[0] > mPoints[1]) EndGame(this, new PlayerWinArgs(PlayerIndex.One));
                else EndGame(this, new PlayerWinArgs(PlayerIndex.Two));
            }
        }
    }
}
