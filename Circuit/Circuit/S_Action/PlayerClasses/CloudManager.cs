using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses
{
    class CloudManager
    {
        enum UpdateState { IDLE, CHANGING, ADJUSTING }
        enum Formation { ONE, TWO, THREE, FOUR }

        Formation mFormat;
        UpdateState mState;
        Vector3 mCenter;
        Vector3 mShotPos;
        Vector2 mBaseAngles;
        FragmentCloud mCurrentCloud;
        FragmentCloud[] mClouds;
        List<FragmentCloud> mActive;
        float mPosInc = 0;
        float mRotInc = 0;
        float mRotation = 0;
        float mInsurrance = 0;
        float mChangeDir = 1;
        float mAdjustDir = -1;

        float mBaseOffLength = 1f;
        float mBaseOffHeight = .8f;

        Offset[] mOffsets;

        // Primary Offset position
        Vector3 mPriOffset = new Vector3(0, -.3f, 1.5f);

        const float mTWOposIncrement = (float)(Math.PI);
        const float mTHREEposIncrement = (float)(2 * Math.PI / 3);
        const float mFOURposIncrement = (float)(Math.PI * .5f);

        const float mTWOrotIncrement = mTWOposIncrement * .04f;
        const float mTHREErotIncrement = mTHREEposIncrement * .03f;
        const float mFOURrotIncrement = mFOURposIncrement * .02f;

        const float mTolerance = .15f;
        const float mMinIns = 20;

        public int WeaponIndex
        {
            get { return mCurrentCloud != null ? mCurrentCloud.Index : -1; }
        }

        public FragmentCloud Cloud
        {
            get { return mCurrentCloud; }
        }

        public bool Busy
        {
            get { return mState != UpdateState.IDLE; }
        }

        public void UpdateGlow()
        {
            foreach (FragmentCloud _fc in mClouds)
            {
                _fc.GlowDark();
            }

            if(mCurrentCloud != null)
                mCurrentCloud.GlowLight();
        }

        public CloudManager(Player _player)
        {
            _player.Moved += new EventHandler<PositionEventArgs>(OnPlayerMoved);

            mActive = new List<FragmentCloud>();
            mState = UpdateState.IDLE;
            mFormat = Formation.ONE;
            mCenter = Vector3.Zero;
            mBaseAngles = Vector2.Zero;
            mClouds = new FragmentCloud[4];
            mClouds[0] = new FragmentCloud(0);
            mClouds[1] = new FragmentCloud(1);
            mClouds[2] = new FragmentCloud(2);
            mClouds[3] = new FragmentCloud(3);
            mCurrentCloud = null;
            mOffsets = new Offset[4];

            for (int i = 0; i < 4; ++i)
            {
                mOffsets[i] = new Offset(mPriOffset, mBaseOffLength, mBaseOffHeight);
            }
        }

        public void Update(GameTime _GT)
        {
            /*
            if(mCurrentCloud.Count == 0 && mActive.Count > 1)
            {
                mActive.Remove(mCurrentCloud);
                mCurrentCloud = mActive.First();
            }*/

            if (Math.Abs(mRotation) > 6.25f)
                mRotation = 0;

            CheckFormation();

            RefreshClouds(_GT);

            if (mState == UpdateState.CHANGING)
            {
                Rotate();

                if (Math.Abs(mRotation) % mPosInc < mTolerance
                 && mInsurrance >= mMinIns)
                {
                    mState = UpdateState.IDLE;
                    mInsurrance = 0;
                }
            }

            if (mState == UpdateState.ADJUSTING)
            {
                if (mRotation != 0)
                {
                    Adjust();

                    if (Math.Abs(mRotation) % mPosInc < mTolerance
                     && mInsurrance >= mMinIns)
                    {
                        mState = UpdateState.IDLE;
                        mInsurrance = 0;
                    }
                }
                else
                {
                    mState = UpdateState.IDLE;
                }
            }
        }

        private void Rotate()
        {
            mRotation += mRotInc * mChangeDir;
            ++mInsurrance;
        }

        private void Adjust()
        {
            if (mRotInc == 0)
                mRotation = 0;

            mRotation += mRotInc * mAdjustDir;
            ++mInsurrance;
        }

        private void CheckFormation()
        {
            for (int i = 0; i < 4; ++i)
            {
                // 1. case: Cloud i has Fragments in it   AND  i is not contained in the active-list
                // - Cloud i is added to the active list
                if (mClouds[i].Count > 0 && !mActive.Contains(mClouds[i]))
                {
                    mActive.Add(mClouds[i]);
                }
                // 2. case: The active-list has more than 1 entries   AND   Cloud i is empty   AND   Cloud i is contained in the active-list
                // - Cloud i is removed from the active-list
                // - The first Cloud in the active-list is made the CurrentCloud
                if (mActive.Count > 1 && mClouds[i].Count == 0 && mActive.Contains(mClouds[i]))
                {
                    //int tCurIndex = i;

                    //if (mChangeDir < 0)
                    //{
                    //    if (tCurIndex != mActive.Count - 1 && mActive.Count > 2)
                    //        mCurrentCloud = mActive[i + 1];
                    //    else
                    //        mCurrentCloud = mActive.First();
                    //}
                    //else
                    //{
                    //    if (tCurIndex != 0 && mActive.Count > 2)
                    //        mCurrentCloud = mActive[i];
                    //    else
                    //        mCurrentCloud = mActive.Last();
                    //}

                    int tCount = mActive.Count;
                    mActive.Remove(mClouds[i]);

                    mState = UpdateState.ADJUSTING;

                    CheckNewPrimary(tCount);

                    mAdjustDir *= -1;
                }

                if (mActive.Count == 1 && mCurrentCloud != null && mCurrentCloud.Count == 0)
                {
                    mActive.Remove(mClouds[i]);
                    mCurrentCloud = null;
                }
            }

            switch (mActive.Count)
            {
                case 0:
                    break;
                case 1:
                    mFormat = Formation.ONE;
                    mRotInc = 0;
                    break;
                case 2:
                    mFormat = Formation.TWO;
                    mRotInc = mTWOrotIncrement;
                    break;
                case 3:
                    mFormat = Formation.THREE;
                    mRotInc = mTHREErotIncrement;
                    break;
                case 4:
                    mFormat = Formation.FOUR;
                    mRotInc = mFOURrotIncrement;
                    break;
                default:
                    throw new Exception();
            }

            switch (mFormat)
            {
                case Formation.ONE:
                    mPosInc = 0;
                    break;
                case Formation.TWO:
                    mPosInc = mTWOposIncrement;
                    break;
                case Formation.THREE:
                    mPosInc = mTHREEposIncrement;
                    break;
                case Formation.FOUR:
                    mPosInc = mFOURposIncrement;
                    break;
                default:
                    throw new Exception();
            }
        }

        // This function determines which Cloud is the new primary after the last ran out of Fragments
        // For each of the three possible new Formations exists a limited number of possible cases
        private void CheckNewPrimary(int _lastCount)
        {
            // Case TWO
            // The trivial case is that only ONE Cloud remains (lastCount - 1), hence it becomes the new Primary
            if (_lastCount == 2)
            {
                mCurrentCloud = mActive.First();
                return;
            }

            if (_lastCount > 1)
            {
                if (ApproximatePosition(0))             // In each other case the choice is trivial if the Rotation is not modified
                {
                    mCurrentCloud = mActive.First();
                    mState = UpdateState.IDLE;
                    return;
                }
                // Case THREE
                if (_lastCount == 3)
                {
                    mCurrentCloud = mActive[0];
                    return;
                }
                // Case FOUR
                if (_lastCount == 4)
                {
                    if (ApproximatePosition(mFOURposIncrement))
                    {
                        if (mRotation < 0)
                            mCurrentCloud = mActive[2];
                        else
                            mCurrentCloud = mActive[0];
                        return;
                    }
                    if (ApproximatePosition(2 * mFOURposIncrement))
                    {
                        mCurrentCloud = mActive[1];
                        return;
                    }
                    if (ApproximatePosition(3 * mFOURposIncrement))
                    {
                        if (mRotation < 0)
                            mCurrentCloud = mActive[0];
                        else
                            mCurrentCloud = mActive[2];
                        return;
                    }
                }
            }
        }

        private bool ApproximatePosition(float _value)
        {
            float tRotation = Math.Abs(mRotation);
            if ((tRotation < mTolerance + _value && tRotation >= _value)
             || (tRotation > _value - mTolerance && tRotation <= _value))
            {
                return true;
            }

            return false;
        }

        private void RefreshClouds(GameTime _GT)
        {
            for (int i = 0; i < 4; ++i)
            {
                mOffsets[i].Update(new Vector3(0, mBaseAngles.Y + mRotation + mPosInc * i, 0));
                mClouds[i].Update(_GT);
            }

            int tCount = 0;
            foreach (FragmentCloud _c in mActive)
            {
                _c.SetPosition(mOffsets[tCount].RelPosition() + mCenter, mShotPos);
                ++tCount;
            }
        }

        public void Pickup(Fragment _frag)
        {
            int tTypeIndex = 0;

            switch (_frag.Type)
            {
                case Data.ProgrammType.DELETE:
                    tTypeIndex = 0;
                    break;
                case Data.ProgrammType.OPTIMIZE:
                    tTypeIndex = 1;
                    break;
                case Data.ProgrammType.GOTO:
                    tTypeIndex = 2;
                    break;
                case Data.ProgrammType.FIREWALL:
                    tTypeIndex = 3;
                    break;
            }

            mClouds[tTypeIndex].Add(_frag);

            if (mCurrentCloud == null)
            {
                mCurrentCloud = mClouds[tTypeIndex];
            }
        }

        public void CycleRight()
        {
            if (mState == UpdateState.IDLE)
                if (mActive.Count() > 1)
                {
                    int tCurIndex = mActive.IndexOf(mCurrentCloud);

                    if (tCurIndex != mActive.Count - 1)
                        mCurrentCloud = mActive[tCurIndex + 1];
                    else
                        mCurrentCloud = mActive.First();
                    mState = UpdateState.CHANGING;
                    mChangeDir = -1;
                    mAdjustDir *= -1;
                }
        }

        public void CycleLeft()
        {
            if (mState == UpdateState.IDLE)
                if (mActive.Count() > 1)
                {
                    int tCurIndex = mActive.IndexOf(mCurrentCloud);

                    if (tCurIndex > 0)
                        mCurrentCloud = mActive[tCurIndex - 1];
                    else
                        mCurrentCloud = mActive.Last();
                    mState = UpdateState.CHANGING;
                    mChangeDir = 1;
                    mAdjustDir *= -1;
                }
        }

        #region Subscribed events

        private void OnPlayerMoved(object _sender, PositionEventArgs _e)
        {
            mCenter = _e.Position;
            mBaseAngles = _e.Angles;
            mShotPos = _e.ShotPos;
        }

        #endregion
    }
}
