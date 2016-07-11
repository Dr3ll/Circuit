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
using BEPUphysics.CollisionRuleManagement;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallFactory : ProgrammFactory
    {
        static BEPUphysics.CollisionShapes.MobileMeshShape FIRE_BASESHAPE;
        static Model FIRE_BASEMODEL;

        Boost mCurBoost;
        MotorizedGrabSpring mGrabber;
        Player mPlayer;
        
        float mGrabDistance = 6;


        public MotorizedGrabSpring Grabber
        {
            get { return mGrabber; }
        }

        internal Run CurRun
        {
            get { return mCurRun; }
        }

        public static void Setup(Microsoft.Xna.Framework.Content.ContentManager _CM)
        {
            if (FIRE_BASEMODEL == null)
                FIRE_BASEMODEL = _CM.Load<Model>(Data.FIRE_PATH + "/" + Data.FIRE_MODELPATH);
        }

        public static Model GetModelCopy()
        {
            Model tCopy = FIRE_BASEMODEL;
            return tCopy;
        }

        public static BEPUphysics.Entities.Entity GetPhysicalEntity(float _scale)
        {
            //Vector3[] tVertices;
            //int[] tIndices;
            //BEPUphysics.DataStructures.TriangleMesh.GetVerticesAndIndicesFromModel(DEL_BASEMODEL, out tVertices, out tIndices);

            //return new BEPUphysics.Entities.Entity(new BEPUphysics.CollisionShapes.MobileMeshShape(
            //        tVertices, tIndices, new BEPUphysics.MathExtensions.AffineTransform(new Vector3(_scale), Quaternion.CreateFromYawPitchRoll(0, .5f, 0), Vector3.Zero),
            //        BEPUphysics.CollisionShapes.MobileMeshSolidity.Counterclockwise));


            return new BEPUphysics.Entities.Entity(new ConvexCollidable<BoxShape>(new BoxShape(Data.FIRE_TRAPENTITYSCALE, Data.FIRE_TRAPENTITYSCALE, Data.FIRE_TRAPENTITYSCALE)), Data.OPT_SHOTBASEMASS);
        }

        public FirewallFactory(Game _GAME, Player _player)
            : base(_GAME, _player)
        {
            mGrabber = new MotorizedGrabSpring();

            mPlayer = _player;
        }

        public override float CreateShot(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null)       // A Shot not yet already been build
            {
                Shot tShot = new FirewallShot(Data.FIRE_PATH,
                    GAME,
                    mShotPos,
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    _index);

                mCurRun = tShot;
                _SCREEN.Add(tShot.Model);
                tShot.Entity.IsAffectedByGravity = Data.FIRE_GRAVITYAFFECTED;

                return Data.FIRE_SHOTCOST;
            }

            return 0;
        }

        public override float CreateTrap(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null)
            {
                Trap tTrap = new FirewallTrap(Data.FIRE_PATH,
                    GAME,
                    mShotPos + mShotDir * (mGrabDistance - 2),
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    _index);

                mCurRun = tTrap;
                _SCREEN.Add(tTrap.Model);
                _space.Add(tTrap.Model);
                (mCurRun as EntityModel).Entity.BecomeDynamic(Data.FIRE_TRAPMASS);
                tTrap.Entity.IsAffectedByGravity = false;
                mCurRun.UpdatePos(mShotPos + mShotDir * (mGrabDistance - 2), CalcTrapOrientation(mShotRot));

                mGrabber.Setup(tTrap.Model.Entity, tTrap.Model.Position);

                return Data.FIRE_TRAPCOST;
            }

            return 0;
        }

        public override float CreateBoost(DisposeHandler _space, GameScreen _SCREEN)
        {
            mCurBoost = new FirewallBoost(_SCREEN, _space, mPlayer);

            return Data.FIRE_BOOSTCOST;
        }

        public override void ReleaseShot(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                (mCurRun as EntityModel).Entity.BecomeDynamic(1);
                (mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.FIRE_SHOTSPEED;
                (mCurRun as EntityModel).Entity.IsAffectedByGravity = Data.FIRE_GRAVITYAFFECTED;
                _space.Add((mCurRun as EntityModel));
            }

            mCurRun = null;
        }

        public override void ReleaseTrap(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                //(mCurRun as EntityModel).Entity.BecomeDynamic(1);
                //(mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.FIRE_SHOTSPEED;
                (mCurRun as EntityModel).Entity.IsAffectedByGravity = true;
                (mCurRun as FirewallTrap).mReleased = true;
                mGrabber.Release();

                //_space.Add(new BEPUphysics.Constraints.SingleEntity.MaximumLinearSpeedConstraint((mCurRun as Trap).Model.Entity, .5f));

                //(mCurRun as EntityModel).Entity.CollisionInformation.CollisionRules.Group = _group;
            }

            mCurRun = null;
        }

        public override Boost ReleaseBoost(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurBoost != null)
                (mCurBoost as FirewallBoost).SetCollisionGroup(_group);

            FirewallBoost tBoost = mCurBoost as FirewallBoost;
            mCurBoost = null;
            return tBoost;
        }

        private Vector3 CalcTrapOrientation(Vector3 _shotRot)
        {
            float tRotCorrection = -.41f;
            return new Vector3(_shotRot.X + tRotCorrection, 0, _shotRot.Z);
        }

        #region Subscribed events

        protected override void OnPlayerMoved(object _sender, PositionEventArgs _e)
        {
            if (!(mCurRun is FirewallTrap))
                base.OnPlayerMoved(null, _e);

            mGrabber.GoalPosition = _e.Position + _e.Direction * mGrabDistance;
            mGrabber.GoalOrientation = CalcTrapOrientation(_e.ShotRot);
        }

        #endregion
    }
}
