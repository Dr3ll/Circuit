using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallTrap : Trap
    {
        public bool mReleased;
        bool mWeighted;

        public FirewallTrap(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(FirewallFactory.GetPhysicalEntity(Data.FIRE_TRAPENTITYSCALE),
            FirewallFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Würfel/GlowCyan")
            )
        {
            ModelScaling = new Vector3(
                Data.FIRE_TRAPMODELSCALE,
                Data.FIRE_TRAPMODELSCALE,
                Data.FIRE_TRAPMODELSCALE);
            EntityScaling = Data.FIRE_TRAPENTITYSCALE;

            mCM.CancelCounter(mNormCounter);

            //this.Entity = FirewallFactory.GetPhysicalEntity(Data.FIRE_TRAPENTITYSCALE);

            Entity.CollisionInformation.Events.ContactCreated += OnCollision;
            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.TRAP, Data.ProgrammType.FIREWALL, this.EntityScaling, _index);

            Entity.Position = _pos;
        }

        private void OnCollision(object _sender, BEPUphysics.Collidables.Collidable _other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair, BEPUphysics.CollisionTests.ContactData _contact)
        {
            if (mReleased && !mWeighted && this.Entity.LinearVelocity.LengthSquared() <= 8)
            {
                // Set a heavier weight for the Trap so it cant be moved while placing other Traps
                this.Entity.BecomeKinematic();
                this.Entity.BecomeDynamic(5000);
                mWeighted = true;
            }
        }
    }
}
