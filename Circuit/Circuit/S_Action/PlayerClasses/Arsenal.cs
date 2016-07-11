using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.ArenaClasses;
using Microsoft.Xna.Framework;
using BEPUphysics;
using BEPUphysics.Constraints.TwoEntity.Joints;
using Circuit.Utilities;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using Microsoft.Xna.Framework.Input;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using Circuit.S_Action.PlayerClasses.Programms.Optimize;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;
using Circuit.S_Action.PlayerClasses.Programms.GoTo;


namespace Circuit.S_Action.PlayerClasses
{
    class Arsenal
    {
        Prog mProg;
        BoostManager mBoostManager;
        int mCurIndex;                                  // CurIndex == -1 is error-code for out of ammo
        int mCircleQueue;
        ProgrammFactory[] mFactories;
        float[] mAmmo;
        Data.ProgrammType[] mTypes;
        CloudManager mCloudManager;
        PlayerIndex mIndex;
        GoToFactory mGoToFactory;
        FirewallFactory mFirewallFactory;

        bool mRunFired;
        bool mStalled;

        // Player Vectors
        Vector3 mPlayerDir;
        Vector3 mPlayerShotPos;

        EventHandler mHandleUsed;

        Arena mArena;

        public Data.ProgrammType CurProgramm
        {
            get { return mTypes[mCurIndex]; }
        }

        public float GetAmmo()
        {
            if (mCurIndex == -1) return 0;
            else return mAmmo[mCurIndex];
        }

        public BoostManager BoostManager
        {
            get { return mBoostManager; }
        }

        public EventHandler<PositionEventArgs> EMhandler
        {
            get { return mGoToFactory.EMhandler; }
        }

        public List<GoToShot> GoToShots
        {
            get { return mGoToFactory.Shots; }
        }

        public MotorizedGrabSpring Grabber
        {
            get { return mFirewallFactory.Grabber; }
        }

        public Arsenal(Arena _arena, Player _player)//, Player _enemy)
        {
            mPlayerDir = _player.ShotDirection;
            mPlayerShotPos = _player.RelShotPosition;

            mHandleUsed = new EventHandler(OnHandleUsed);
            //_enemy.Moved += mGoToFactory.EMhandler;

            mBoostManager = new BoostManager();
            mCloudManager = new CloudManager(_player);

            mArena = _arena;
            mCurIndex = -1;
            mIndex = _player.Index;
            mTypes = new Data.ProgrammType[Data.A_NUMBEROFWEAPONS];
            mTypes[0] = Data.ProgrammType.DELETE;
            mTypes[1] = Data.ProgrammType.OPTIMIZE;
            mTypes[2] = Data.ProgrammType.GOTO;
            mTypes[3] = Data.ProgrammType.FIREWALL;

            mFactories = new ProgrammFactory[Data.A_NUMBEROFWEAPONS];
            mFactories[0] = new Circuit.S_Action.PlayerClasses.Programms.Delete.DeleteFactory(_arena.Screen.Game, _player);
            mFactories[1] = new Circuit.S_Action.PlayerClasses.Programms.Optimize.OptimizeFactory(_arena.Screen.Game, _player);
            mFactories[2] = new Circuit.S_Action.PlayerClasses.Programms.GoTo.GoToFactory(_arena.Screen.Game, _player, mArena.GoToSplinterCG);
            mFactories[3] = new Circuit.S_Action.PlayerClasses.Programms.Firewall.FirewallFactory(_arena.Screen.Game, _player);

            mGoToFactory = (GoToFactory)mFactories[2];
            mFirewallFactory = (FirewallFactory)mFactories[3];

            mAmmo = new float[Data.A_NUMBEROFWEAPONS];
            mAmmo[0] = 0;
            mAmmo[1] = 0;
            mAmmo[2] = 0;
            mAmmo[3] = 0;

            InitializeAmmunition();

            // Debug Ammo
            //mAmmo[0] = 999999999;mAmmo[1] = 999999999;mAmmo[2] = 999999999;mAmmo[3] = 999999999;

            RefreshProg();
            //mProg.Clear();
        }

        public void InitializeAmmunition()
        {
            for (int i = 0; i < Data.A_START_DELETE; i++)
            {
                List<Fragment> tFragments = new List<Fragment>();
                Circuit.S_Action.PlayerClasses.Programms.Delete.DeleteFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Delete.DeleteFragment(new Vector3(0, 0, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
                mArena.Add(tFragments);
                Pickup(tFrag);
            }

            for (int i = 0; i < Data.A_START_OPTIMIZE; i++)
            {
                List<Fragment> tFragments = new List<Fragment>();
                Circuit.S_Action.PlayerClasses.Programms.Optimize.OptimizeFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Optimize.OptimizeFragment(new Vector3(0, 0, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
                mArena.Add(tFragments);
                Pickup(tFrag);
            }

            for (int i = 0; i < Data.A_START_FIREWALL; i++)
            {
                List<Fragment> tFragments = new List<Fragment>();
                Circuit.S_Action.PlayerClasses.Programms.Firewall.FirewallFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Firewall.FirewallFragment(new Vector3(0, 0, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
                mArena.Add(tFragments);
                Pickup(tFrag);
            }

            for (int i = 0; i < Data.A_START_GOTO; i++)
            {
                List<Fragment> tFragments = new List<Fragment>();
                Circuit.S_Action.PlayerClasses.Programms.GoTo.GoToFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.GoTo.GoToFragment(new Vector3(0, 0, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
                mArena.Add(tFragments);
                Pickup(tFrag);
            }
        }

        public void TriggerShot()
        {
            float tAmmo = mProg.Ammo;

            // Refresh the current Programm because the Cloud may have run out of ammo
            if (mProg.Ammo < 1)
                RefreshProg();

            if (mProg.CurSize < Data.A_MAXSIZE
             && !mCloudManager.Busy)
            {
                float tCost = mProg.Factory.CreateShot(mArena.Space, mArena.Screen, mIndex);
                mProg.Ammo -= tCost;

                if (!(mProg.Type == Data.ProgrammType.GOTO))
                    mProg.CurSize += tCost;

                RefreshAmmo();

                mStalled = true;
                mRunFired = true;
            }
            CheckCloudSubstraction(tAmmo);
            mCircleQueue = 0;
        }

        public void TriggerTrap()
        {
            float tAmmo = mProg.Ammo;

            // If no trap is fired yet create one
            if (mProg.Ammo >= 1
             && !mCloudManager.Busy)
            {
                float tCost = mProg.Factory.CreateTrap(mArena.Space, mArena.Screen, mIndex);
                mProg.Ammo -= tCost;

                RefreshAmmo();

                mStalled = true;
                mRunFired = true;
            }
            CheckCloudSubstraction(tAmmo);
            mCircleQueue = 0;
        }

        public void TriggerBoost()
        {
            float tAmmo = mProg.Ammo;

            if (mBoostManager.Empty(mProg.Type))
            {
                if (mProg.Ammo >= 1
                 && !mCloudManager.Busy)
                {
                    mProg.Ammo -= mProg.Factory.CreateBoost(mArena.Space, mArena.Screen);

                    mStalled = true;
                    mRunFired = true;

                    RefreshAmmo();
                }
            }
            CheckCloudSubstraction(tAmmo);
            mCircleQueue = 0;
        }

        public void TriggerNegativeBoost()
        {
            if (mBoostManager.Empty(Data.ProgrammType.OPTIMIZE_N))
            {
                mBoostManager.AddBoost(new OptimizeNegativeBoost());
            }
        }

        public void ReleaseShot()
        {
            mProg.Factory.ReleaseShot(null, mArena.Space);
            mRunFired = false;
            mProg.CurSize = 0;

            UpdateAmmo();
            mStalled = false;
            RefreshProg();
        }

        public void ReleaseTrap()
        {
            if (mProg.Type == Data.ProgrammType.DELETE && (mProg.Factory as DeleteFactory).CurRun != null)
            {
                mArena.PlaceTrap(((mProg.Factory as DeleteFactory).CurRun as Trap));
            }

            mProg.Factory.ReleaseTrap(mArena.TrapsCG, mArena.Space);
            mRunFired = false;
            mStalled = false;
            RefreshProg();
        }

        public void ReleaseBoost()
        {
            mBoostManager.AddBoost(mProg.Factory.ReleaseBoost(mArena.FirewallChipCG, null));
            mStalled = false;
            mRunFired = false;
            RefreshProg();
        }

        private void RefreshAmmo()
        {
            //mAmmo[mCurIndex] = mProg.Ammo;
        }

        private void CheckCloudSubstraction(float _ammo)
        {
            if (mCurIndex != -1)
            {
                int tDiff = -((int)mProg.Ammo - (int)_ammo);
                while (tDiff >= 1)
                {
                    mProg.Cloud.Substract();
                    --tDiff;
                }
            }
        }

        private void UpdateAmmo()
        {
            //if (mProg.Ammo <= 0)
            //    mProg.Ammo = -.00000001f;

            if (mCurIndex != -1)
                mAmmo[mCurIndex] = mProg.Ammo >= 1 ? mProg.Ammo : 0;
        }

        public void Pickup(Fragment _frag)
        {
            if (_frag != null)
            {
                switch (_frag.Type)
                {
                    case Data.ProgrammType.DELETE:
                        if (mAmmo[0] < Data.A_CAPACITY)
                        {
                            ++mAmmo[0];
                            mArena.Space.Remove(_frag.Entity);
                            _frag.PickUp();
                            mCloudManager.Pickup(_frag);
                            _frag.Used += mHandleUsed;
                        }
                        break;
                    case Data.ProgrammType.OPTIMIZE:
                        if (mAmmo[1] < Data.A_CAPACITY)
                        {
                            ++mAmmo[1];
                            mArena.Space.Remove(_frag.Entity);
                            _frag.PickUp();
                            mCloudManager.Pickup(_frag);
                            _frag.Used += mHandleUsed;
                        }
                        break;
                    case Data.ProgrammType.GOTO:
                        if (mAmmo[2] < Data.A_CAPACITY)
                        {
                            ++mAmmo[2];
                            mArena.Space.Remove(_frag.Entity);
                            _frag.PickUp();
                            mCloudManager.Pickup(_frag);
                            _frag.Used += mHandleUsed;
                        }
                        break;
                    case Data.ProgrammType.FIREWALL:
                        if (mAmmo[3] < Data.A_CAPACITY)
                        {
                            ++mAmmo[3];
                            mArena.Space.Remove(_frag.Entity);
                            _frag.PickUp();
                            mCloudManager.Pickup(_frag);
                            _frag.Used += mHandleUsed;
                        }
                        break;
                }

                RefreshProg();
            }
        }

        private void ProcessCycle()
        {
            if (mCircleQueue != 0)
                if (!mRunFired)
                    if (!mCloudManager.Busy)
                    {
                        if (mCircleQueue > 0)
                        {
                            mCloudManager.CycleRight();
                            mCircleQueue--;
                        }
                        if (mCircleQueue < 0)
                        {
                            mCloudManager.CycleLeft();
                            mCircleQueue++;
                        }

                        UpdateAmmo();
                        RefreshProg();
                    }
        }

        public void CycleRight()
        {
            if (!mRunFired)
                if (mCircleQueue >= 0)
                    mCircleQueue++;
        }

        public void CycleLeft()
        {
            if (!mRunFired)
                if (mCircleQueue <= 0)
                    mCircleQueue--;
        }

        private void RefreshProg()
        {
            if (!mStalled)
            {
                mCurIndex = mCloudManager.WeaponIndex;

                switch (mCurIndex)
                {
                    case 0:
                        mProg.ShotCost = Data.DEL_SHOTCOST;
                        mProg.TrapCost = Data.DEL_TRAPCOST;
                        mProg.BoostCost = Data.DEL_BOOSTCOST;
                        break;

                    case 1:
                        mProg.ShotCost = Data.OPT_SHOTCOST;
                        mProg.TrapCost = Data.OPT_TRAPCOST;
                        mProg.BoostCost = Data.OPT_BOOSTCOST;
                        break;

                    case 2:
                        mProg.ShotCost = Data.GOTO_SHOTCOST;
                        mProg.TrapCost = Data.GOTO_TRAPCOST;
                        mProg.BoostCost = Data.GOTO_BOOSTCOST;
                        break;

                    case 3:
                        mProg.ShotCost = Data.FIRE_SHOTCOST;
                        mProg.TrapCost = Data.FIRE_TRAPCOST;
                        mProg.BoostCost = Data.FIRE_BOOSTCOST;
                        break;

                    case 4:     //TODO: Costs for Weapon 5
                        break;

                    case -1:
                        mProg.Clear();
                        break;

                    default:
                        break;
                }

                if (mCurIndex != -1)
                {
                    mProg.Cloud = mCloudManager.Cloud;
                    mProg.Type = mTypes[mCurIndex];
                    mProg.Ammo = mAmmo[mCurIndex];
                    mProg.Factory = mFactories[mCurIndex];
                }

                mCloudManager.UpdateGlow();
            }
        }

        public void Update(GameTime _GT)
        {
            UpdateAmmo();

            ProcessCycle();

            mBoostManager.Update(_GT);
            mCloudManager.Update(_GT);

            mGoToFactory.Update(_GT);

            // This doesnt show correct values if PlayerTWO is beeing updated

        }


        #region Subscribed Events

        private void OnHandleUsed(object _sender, EventArgs _e)
        {
            mArena.Remove(_sender as Fragment);
        }

        #endregion

    }
    struct Prog
    {
        public FragmentCloud Cloud;
        public float Ammo;
        public ProgrammFactory Factory;
        public Data.ProgrammType Type;
        public float CurSize;
        public float ShotCost;
        public float TrapCost;
        public float BoostCost;

        public void Clear()
        {
            Cloud = null;
            Ammo = 0;
            Factory = new DummieFactory();
            Type = Data.ProgrammType.DEFAULT;
            CurSize = 0;
            ShotCost = 9001;
            TrapCost = 9001;
            BoostCost = 9001;
        }
    }
}
