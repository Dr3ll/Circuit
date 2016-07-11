using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    class ShotCollisionTag
    {
        int mRunType;
        int mProgramType;
        float mScale;

        public int RunType
        {
            get { return mRunType; }
        }

        public int ProgramType
        {
            get { return mProgramType; }
        }

        public float ScaleFactor
        {
            get { return mScale; }
        }

        public ShotCollisionTag(object _runType, object _programType, float _scaling)
        {
            mRunType = (int)_runType;
            mProgramType = (int)_programType;
            mScale = _scaling;
        }
    }
}
