using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Collidables.MobileCollidables;
using Circuit.S_Action.PlayerClasses.Character;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallShot : Shot
    {
        private PlayerIndex mIndex;
        public FirewallShot(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(FirewallFactory.GetPhysicalEntity(Data.FIRE_SHOTENTITYSCALE),
            FirewallFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Würfel/GlowCyan")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;
            mIndex = _index;
            ModelScaling = new Vector3(Data.FIRE_SHOTMODELSCALE);
            EntityScaling = Data.FIRE_SHOTENTITYSCALE;

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.SHOT, Data.ProgrammType.FIREWALL, this.EntityScaling, _index);

            Entity.CollisionInformation.Events.ContactCreated += OnCollision;

            base.Coloring(new Vector3(0, 255, 0));
        }

        private void OnCollision(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            if (_other.Tag != null && _other.Tag is CharacterSynchronizer)
            {
                Entity.CollisionInformation.CollisionRules.Group = null;
            }
        }

        public void Empower(float _mult)
        {
            this.ModelScaling *= _mult;
            this.EntityScaling *= _mult;

            this.Entity = FirewallFactory.GetPhysicalEntity(EntityScaling * 5);

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.SHOT, Data.ProgrammType.FIREWALL, this.EntityScaling, mIndex);
        }
    }
}
