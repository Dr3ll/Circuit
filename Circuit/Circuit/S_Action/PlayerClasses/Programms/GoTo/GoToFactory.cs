using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Screens;
using Circuit.Utilities;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using Circuit.Utilities.Counter;
using BEPUphysics.CollisionRuleManagement;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToFactory : ProgrammFactory
    {
        static BEPUphysics.CollisionShapes.MobileMeshShape GOTO_BASESHAPE;
        static Model GOTO_BASEMODEL;

        Boost mCurBoost;
        CounterManager mCounterManager;
        bool mCooldownOn;
        List<GoToShot> mShots;
        CollisionGroup mSplinterCG;

        EventHandler<PositionEventArgs> mEnemyMovementHandler;

        public List<GoToShot> Shots
        {
            get { return mShots; }
        }

        internal Run CurRun
        {
            get { return mCurRun; }
        }

        public EventHandler<PositionEventArgs> EMhandler
        {
            get
            {
                if (mEnemyMovementHandler == null)
                {
                    mEnemyMovementHandler = new EventHandler<PositionEventArgs>(OnEnemyMoved);

                    return mEnemyMovementHandler;
                }

                throw new Exception();
            }
        }

        public static void Setup(Microsoft.Xna.Framework.Content.ContentManager _CM)
        {
            if (GOTO_BASEMODEL == null)
                GOTO_BASEMODEL = _CM.Load<Model>(Data.GOTO_PATH + "/" + Data.GOTO_MODELPATH);
        }

        public static Model GetModelCopy()
        {
            Model tCopy = GOTO_BASEMODEL;

            return tCopy;
        }

        public static BEPUphysics.Entities.Entity GetPhysicalEntity(float _scale)
        {
            return new BEPUphysics.Entities.Entity(new ConvexCollidable<BoxShape>(new BoxShape(1 * _scale, 1 * _scale, 1 * _scale)), Data.GOTO_SHOTBASEMASS);
        }

        public GoToFactory(Game _GAME, Player _player, CollisionGroup _splinterCG)
            : base(_GAME, _player)
        {
            mCounterManager = new CounterManager();
            mShots = new List<GoToShot>();
            mSplinterCG = _splinterCG;

            mCounterManager.Bang += new EventHandler(OnBang);
        }

        public override float CreateShot(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null && !mCooldownOn)       // A Shot not yet already been build
            {
                Shot tShot = new GoToShot(Data.DEL_PATH,
                    GAME,
                    mShotPos,
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    _index);

                mCurRun = tShot;
                _SCREEN.Add(tShot.Model);
                mShots.Add(tShot as GoToShot);

                tShot.Entity.IsAffectedByGravity = Data.GOTO_GRAVITYAFFECTED;

                _space. Add(new BEPUphysics.Constraints.SingleEntity.MaximumLinearSpeedConstraint(tShot.Entity, 200));

                ReleaseShot(null, _space);

                mCooldownOn = true;
                mCounterManager.StartCounter(Data.GOTO_SHOTCOOLDOWN, false);

                return Data.GOTO_SHOTCOST;
            }

            return 0;
        }

        public void Update(GameTime _GT)
        {
            mCounterManager.Update(_GT);
        }

        public override float CreateTrap(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null)
            {
                Trap tTrap = new GoToTrap(Data.GOTO_PATH,
                    GAME,
                    mShotPos,
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    _space,
                    mSplinterCG,
                    _index);

                mCurRun = tTrap;
                _SCREEN.Add(tTrap.Model);

                return Data.GOTO_TRAPCOST;
            }

            return 0;
        }

        public override float CreateBoost(DisposeHandler _space, GameScreen _SCREEN)
        {
            mCurBoost = new GoToBoost();

            return Data.GOTO_BOOSTCOST;
        }

        public override void ReleaseShot(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                (mCurRun as EntityModel).Entity.BecomeDynamic(1);
                (mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.DEL_SHOTSPEED;
                _space.Add((mCurRun as EntityModel));

                (mCurRun as EntityModel).Entity.CollisionInformation.CollisionRules.Group = _group;
            }

            mCurRun = null;
        }

        public override void ReleaseTrap(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                (mCurRun as EntityModel).Entity.BecomeDynamic(1);
                (mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.GOTO_SHOTSPEED;
                _space.Add((mCurRun as EntityModel));

                (mCurRun as EntityModel).Entity.CollisionInformation.CollisionRules.Group = _group;
            }

            mCurRun = null;
        }

        public override Boost ReleaseBoost(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            return mCurBoost;
        }

        #region Subscribed events

        private void OnEnemyMoved(object _sender, PositionEventArgs _e)
        {
            foreach (GoToShot _s in mShots)
            {
                _s.ProcessHoming(_e.Position);
            }
        }

        private void OnBang(object _sender, EventArgs _e)
        {
            mCooldownOn = false;
        }

        #endregion
    }
}
