using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Microsoft.Xna.Framework;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    class Boost
    {
        private CounterManager mCM;
        protected int mPower;
        private EventHandler mBangHandler;
        private bool mBurnedOut;

        public bool BurnedOut
        {
            get { return mBurnedOut; }
        }

        public Boost()
        {
            mPower = 0;
            mCM = new CounterManager();
            mBangHandler = new EventHandler(OnCounterUsedUp);
            mCM.Bang += mBangHandler;
        }

        public virtual void Update(GameTime _GT)
        {
            mCM.Update(_GT);
        }

        protected virtual void OnCounterUsedUp(object sender, EventArgs _e)
        {
            mBurnedOut = true;
        }

        public virtual void Start()
        {
            mCM.StartCounter(mPower, true);
        }
    }
}
