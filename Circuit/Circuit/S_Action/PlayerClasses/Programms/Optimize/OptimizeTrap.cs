using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using Circuit.S_Action.PlayerClasses.Character;

namespace Circuit.S_Action.PlayerClasses.Programms.Optimize
{
    class OptimizeTrap : Trap
    {
        List<Vector3> mCollisions = new List<Vector3>();
        bool mConstraint;
        private bool mPlaced;
        private Vector3 mLaserDir;

        public OptimizeTrap(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(OptimizeFactory.GetPhysicalEntity(Data.OPT_TRAPENTITYSCALE),
            OptimizeFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Optimize/GlowWhite")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;

            ModelScaling = new Vector3(
                Data.DEL_TRAPMODELSCALE * 1.9f,
                Data.DEL_TRAPMODELSCALE,
                Data.DEL_TRAPMODELSCALE * 1.9f);
            EntityScaling = Data.DEL_TRAPENTITYSCALE;

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.TRAP, Data.ProgrammType.OPTIMIZE, this.EntityScaling, _index);

            Entity.CollisionInformation.Events.ContactCreated += OnCollision;
        }

        private void OnCollision(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            if (!mPlaced)
            {
                if (!mConstraint)
                {
                    AnchorTrap(_pair, _contact.Position);
                }
                else
                {
                    if (mCollisions.Count < 3)
                    {
                        mCollisions.Add(_contact.Position);
                    }
                    else
                    {
                        Align();
                        mPlaced = true;
                        mCheckingEnabled = true;
                    }
                }
            }
            else
            {
                if (_other.Tag != null && _other.Tag is CharacterSynchronizer)
                {
                    Trigger();
                }
            }
        }

        protected override void Trigger()
        {
            DisposeThis();
        }

        private void AnchorTrap(BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair, Vector3 _pos)
        {
            Entity.AngularVelocity = Vector3.Zero;
            Entity.LinearVelocity = Vector3.Zero;

            Entity.IsAffectedByGravity = true;

            _pair.EntityA.Space.Add(new BEPUphysics.Constraints.TwoEntity.Joints.DistanceJoint(_pair.EntityA, _pair.EntityB, _pos, _pos));
            mConstraint = true;
        }

        private void Align()
        {
            Vector3 tVectorA = mCollisions[0] - mCollisions[1];
            Vector3 tVectorB = mCollisions[0] - mCollisions[2];
            Vector3 tOrientation = Vector3.Cross(tVectorA, tVectorB);
            tOrientation.Normalize();

            Entity.BecomeKinematic();
            Entity.CollisionInformation.CollisionRules.Group = null;

            Entity.AngularVelocity = Vector3.Zero;
            Entity.LinearVelocity = Vector3.Zero;

            mLaserDir = tOrientation;

            Entity.Orientation = Quaternion.CreateFromYawPitchRoll(tOrientation.Y, tOrientation.X, tOrientation.Z);
        }
    }
}
