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

namespace Circuit.S_Action.PlayerClasses.Programms.Delete
{
    class DeleteFactory : ProgrammFactory
    {
        static BEPUphysics.CollisionShapes.MobileMeshShape DEL_BASESHAPE;
        static Model DEL_BASEMODEL;
        //public static Texture2D mGlowLight;
        //public static Texture2D mGlowDark;

        Boost mCurBoost;

        internal Run CurRun
        {
            get { return mCurRun; }
        }

        public static void Setup(Microsoft.Xna.Framework.Content.ContentManager _CM)
        {
            if (DEL_BASEMODEL == null)
                DEL_BASEMODEL = _CM.Load<Model>(Data.DEL_PATH + "/" + Data.DEL_MODELPATH);

            //mGlowLight = _CM.Load<Texture2D>();
            //mGlowDark = _CM.Load<Texture2D>();
        }

        public static Model GetModelCopy()
        {
            Model tCopy = DEL_BASEMODEL;
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


            return new BEPUphysics.Entities.Entity(new ConvexCollidable<BoxShape>(new BoxShape(1 * _scale, 1 * _scale, 1 * _scale)), Data.DEL_SHOTBASEMASS);
        }

        public DeleteFactory(Game _GAME, Player _player)
            : base(_GAME, _player)
        {

        }

        public override float CreateShot(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null)       // A Shot not yet already been build
            {
                Shot tShot = new DeleteShot(Data.DEL_PATH,
                    GAME,
                    mShotPos,
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    _index);

                mCurRun = tShot;
                _space.AddForDraw(tShot.Model);
                tShot.Entity.IsAffectedByGravity = Data.DEL_GRAVITYAFFECTED;

                return Data.DEL_SHOTCOST;
            }
            else
            {
                EmpowerShot();

                return Data.DEL_SHOTGROWTH;
            }
        }

        private void EmpowerShot()
        {
            if (mCurRun != null && mCurRun is DeleteShot)
            {
                (mCurRun as DeleteShot).Empower(Data.DEL_GROWTHFACTOR);
            }
        }

        public override float CreateTrap(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index)
        {
            if (mCurRun == null)
            {
                Trap tTrap = new DeleteTrap(Data.DEL_PATH,
                    GAME,
                    mShotPos,
                    Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(mShotDir.X, mShotDir.Y, mShotDir.Z),
                    PlayerIndex.Four);

                mCurRun = tTrap;
                _SCREEN.Add(tTrap.Model);
                tTrap.Entity.IsAffectedByGravity = Data.DEL_GRAVITYAFFECTED;

                return Data.DEL_TRAPCOST;
            }

            return 0;
        }

        public override float CreateBoost(DisposeHandler _space, GameScreen _SCREEN)
        {
            mCurBoost = new DeleteBoost();

            return Data.DEL_BOOSTCOST;
        }

        public override void ReleaseShot(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                (mCurRun as EntityModel).Entity.BecomeDynamic(1);
                (mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.DEL_SHOTSPEED;
                (mCurRun as EntityModel).Entity.IsAffectedByGravity = Data.DEL_GRAVITYAFFECTED;
                _space.Add((mCurRun as EntityModel));
            }

            mCurRun = null;
        }

        public override void ReleaseTrap(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space)
        {
            if (mCurRun != null && mCurRun is EntityModel)
            {
                (mCurRun as EntityModel).Entity.BecomeDynamic(1);
                (mCurRun as EntityModel).Entity.LinearVelocity = mShotDir * Data.DEL_SHOTSPEED;
                (mCurRun as EntityModel).Entity.IsAffectedByGravity = Data.DEL_GRAVITYAFFECTED;
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

        #endregion
    }
}
