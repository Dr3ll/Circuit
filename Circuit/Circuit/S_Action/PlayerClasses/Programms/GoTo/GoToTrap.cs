using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using BEPUphysics.Constraints.TwoEntity.Joints;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToTrap : Trap
    {
        List<GoToSplinter> mSplinters = new List<GoToSplinter>();
        bool mTriggered;
        DisposeHandler mSpace;
        CollisionGroup mSplinterCG;

        public GoToTrap(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, DisposeHandler _space, CollisionGroup _splinterCG, PlayerIndex _index)
            : base(DeleteFactory.GetPhysicalEntity(Data.GOTO_TRAPENTITYSCALE),
            GoToFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreen")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;
            Entity.IsAffectedByGravity = true;
            mSplinterCG = _splinterCG;

            mCM.StartCounter(7000);
            mCM.Bang += new EventHandler(OnBang);

            //this.Entity.CollisionInformation.CollisionRules.Group = _gotoTrapCG;

            mSpace = _space;


            int tSplinterCount = 30;
            for (int i = 0; i < tSplinterCount; ++i)
            {
                GoToSplinter tSP = new GoToSplinter(_GAME);
                tSP.Entity.CollisionInformation.CollisionRules.Group = mSplinterCG;
                tSP.Position = this.Position;
                tSP.Initialize(mSpace);
                mSplinters.Add(tSP);
                tSP.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.TRAP, Data.ProgrammType.GOTO, this.EntityScaling, _index);
            }

            ModelScaling = new Vector3(
                Data.DEL_TRAPMODELSCALE * 3,
                Data.DEL_TRAPMODELSCALE * 3,
                Data.DEL_TRAPMODELSCALE * 3);
            EntityScaling = Data.GOTO_TRAPENTITYSCALE;
            Entity.Mass = Data.GOTO_TRAPBASEMASS;

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.TRAP, Data.ProgrammType.GOTO, this.EntityScaling, _index);

            Entity.CollisionInformation.Events.ContactCreated += OnCollision;
        }

        private void OnCollision(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            Trigger();
        }

        public override void Update(GameTime _GT)
        {
            base.Update(_GT);

            mCM.Update(_GT);

            if (!mTriggered)
            {
                foreach (GoToSplinter _s in mSplinters)
                    _s.mLinearMotor.Settings.Servo.Goal = this.Position;
            }
        }

        protected override void Trigger()
        {
            if (!mTriggered)
            {
                float tPower = 1;
                foreach (GoToSplinter _s in mSplinters)
                {
                    Vector3 tDir = GetRandomDirection();

                    _s.Entity.LinearVelocity = Vector3.Zero;
                    _s.Entity.ApplyImpulse(Vector3.Zero, tDir);
                    _s.Entity.CollisionInformation.Events.InitialCollisionDetected += OnInitialCollsion;
                    _s.mLinearMotor.IsActive = false;
                }

                mTriggered = true;
                mSpace.Remove(this.Entity);
                mSpace.RemoveFromDraw(this);
            }
        }

        private void OnInitialCollsion(EntityCollidable _sender, Collidable _other, CollidablePairHandler _pair)
        {
            //mSpace.Remove(_sender.Entity);

        }

        private Vector3 GetRandomDirection()
        {
            Random tRand = new Random();
            //return Vector3.Zero;
            float tMult = .0003f;

            return new Vector3(((float)tRand.NextDouble() - .5f) * tMult, ((float)tRand.NextDouble() -.5f) * tMult, ((float)tRand.NextDouble() -.5f) * tMult);
        }

        private void OnBang(object _sender, EventArgs _e)
        {
            foreach (GoToSplinter _s in mSplinters)
            {
                _s.DisposeThis();
            }

            if (!mTriggered)
            {
                mSpace.Remove(this.Entity);
                mSpace.RemoveFromDraw(this);
            }
        }
    }
}
