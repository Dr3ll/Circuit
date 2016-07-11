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

namespace Circuit.S_Action.PlayerClasses.Programms.Optimize
{
    class OptimizeShot : Shot
    {
        private PlayerIndex mIndex;

        public OptimizeShot(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(OptimizeFactory.GetPhysicalEntity(Data.OPT_SHOTENTITYSCALE),
            OptimizeFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Optimize/GlowWhite")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;
            mIndex = _index;
            ModelScaling = new Vector3(Data.OPT_SHOTMODELSCALE);
            EntityScaling = Data.OPT_SHOTENTITYSCALE;
        }

        public void Empower(float _mult)
        {
            this.ModelScaling *= _mult;
            this.EntityScaling *= _mult;

            this.Entity = OptimizeFactory.GetPhysicalEntity(EntityScaling * 5);

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.SHOT, Data.ProgrammType.OPTIMIZE, this.EntityScaling, mIndex);
        }
    }
}
