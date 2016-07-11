using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    class CollisionTag
    {
        Data.RunType mRunType;
        Data.ProgrammType mProgramType;
        float mScale;
        PlayerIndex mPlayer;

        public PlayerIndex Player
        {
            get { return mPlayer; }
        }

        public Data.RunType RunType
        {
            get { return mRunType; }
        }

        public Data.ProgrammType ProgramType
        {
            get { return mProgramType; }
            set { mProgramType = value; }
        }

        public float ScaleFactor
        {
            get { return mScale; }
        }

        public void MakeDeleteBoost()
        {
            mRunType = Data.RunType.BOOST;
            mProgramType = Data.ProgrammType.DELETE;
        }

        public void MakePlayer()
        {
            mRunType = Data.RunType.PLAYER;
            mProgramType = Data.ProgrammType.DEFAULT;
        }

        public CollisionTag(object _runType, object _programType, float _scaling, PlayerIndex _index)
        {
            mRunType = (Data.RunType)_runType;
            mProgramType = (Data.ProgrammType)_programType;
            mScale = _scaling;
            mPlayer = _index;

        }

    }
}
