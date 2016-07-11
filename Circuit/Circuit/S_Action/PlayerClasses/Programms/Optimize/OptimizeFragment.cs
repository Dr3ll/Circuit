using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.Optimize
{
    class OptimizeFragment : Circuit.S_Action.PlayerClasses.Programms.Fragment
    {
        public static Texture2D mGlowLight;
        public static Texture2D mGlowDark;
        
        public OptimizeFragment(Vector3 _pos, Game _GAME)
            : base(new BEPUphysics.Entities.Prefabs.Box(_pos, BASESIZE, BASESIZE, BASESIZE, BASEMASS),
            OptimizeFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Optimize/GlowWhiteDark"))
        {
            ModelScaling = new Vector3(Data.DEL_FRAGSCALE);
            base.Type = Data.ProgrammType.OPTIMIZE;
            if (mGlowLight == null) mGlowLight = _GAME.Content.Load<Texture2D>("Programms/Optimize/GlowWhite");
            if (mGlowDark == null) mGlowDark = _GAME.Content.Load<Texture2D>("Programms/Optimize/GlowWhiteDark");
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
