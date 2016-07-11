using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.S_Action.PlayerClasses.Programms;
using Microsoft.Xna.Framework;


namespace Circuit.S_Action.PlayerClasses
{
    class FragmentCloud
    {
        Random mRand;
        List<Fragment> mFragments;
        Vector3 mPosition;
        Vector3 mShotPos;
        EventHandler mHandleUsed;
        int mIndex = 0;

        public int Index
        {
            get { return mIndex; }
        }

        public int Count
        {
            get { return mFragments.Count; }
        }

        public void SetPosition(Vector3 _pos, Vector3 _shotPos)
        {
            mPosition = _pos;
            mShotPos = _shotPos;
        }

        public FragmentCloud(int _index)
        {
            mRand = new Random();
            mFragments = new List<Fragment>();
            mHandleUsed = new EventHandler(HandleUsed);
            mIndex = _index;
        }

        public void Add(Fragment _frag)
        {
            _frag.SetupRotation((float)mRand.NextDouble());
            mFragments.Add(_frag);
            _frag.Used += mHandleUsed;
        }

        public void Substract()
        {
            Fragment tFrag = mFragments.Find(
                delegate(Fragment _f)
                {
                    return !_f.Detached;
                }
                );
            if (tFrag != null)
            {
                tFrag.Detach();
            }
        }

        public void Update(GameTime _GT)
        {
            try
            {
                foreach (Fragment _f in mFragments)
                {
                    _f.Update(mPosition, _GT, mShotPos - mPosition);
                }
            }
            catch { }
        }

        public void GlowLight()
        {
            foreach (Fragment _f in mFragments)
            {
                _f.GlowLight();
            }
        }

        public void GlowDark()
        {
            foreach (Fragment _f in mFragments)
            {
                _f.GlowDark();
            }
        }

        private void HandleUsed(object _sender, EventArgs _e)
        {
            mFragments.Remove(_sender as Fragment);
        }
    }
}
