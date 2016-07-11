using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.Delete
{
    class DeleteFragment : Circuit.S_Action.PlayerClasses.Programms.Fragment
    {
        public static Texture2D mGlowLight;
        public static Texture2D mGlowDark;
        
        public DeleteFragment(Vector3 _pos, Game _GAME)
            : base(new BEPUphysics.Entities.Prefabs.Box(_pos, BASESIZE, BASESIZE, BASESIZE, BASEMASS),
            DeleteFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Delete/GlowRedDark"))
        {
            ModelScaling = new Vector3(Data.DEL_FRAGSCALE);
            base.Type = Data.ProgrammType.DELETE;
            if(mGlowLight == null) mGlowLight = _GAME.Content.Load<Texture2D>("Programms/Delete/GlowRed");
            if (mGlowDark == null) mGlowDark = _GAME.Content.Load<Texture2D>("Programms/Delete/GlowRedDark");
        }

        public override void GlowLight()
        {
            this.GlowTex = mGlowLight;
        }

        public override void GlowDark()
        {
            this.GlowTex = mGlowDark;
        }
    }
}
