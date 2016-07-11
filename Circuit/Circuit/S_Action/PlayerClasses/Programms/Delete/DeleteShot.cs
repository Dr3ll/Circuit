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

namespace Circuit.S_Action.PlayerClasses.Programms.Delete
{
    class DeleteShot : Shot
    {
        private PlayerIndex mIndex;
        public DeleteShot(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(DeleteFactory.GetPhysicalEntity(Data.DEL_SHOTENTITYSCALE),
            DeleteFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Delete/GlowRed")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;
            mIndex = _index;
            ModelScaling = new Vector3(Data.DEL_SHOTMODELSCALE);
            EntityScaling = Data.DEL_SHOTENTITYSCALE;
        }

        public void Empower(float _mult)
        {
            this.ModelScaling *= _mult;
            this.EntityScaling *= _mult;

            this.Entity = DeleteFactory.GetPhysicalEntity(EntityScaling * 5);

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.SHOT, Data.ProgrammType.DELETE, this.EntityScaling, mIndex);
            base.RefreshSubscription();
        }
    }
}
