using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToFragment : Circuit.S_Action.PlayerClasses.Programms.Fragment
    {
        public static Texture2D mGlowLight;
        public static Texture2D mGlowDark;
        
        public GoToFragment(Vector3 _pos, Game _GAME)
            : base(new BEPUphysics.Entities.Prefabs.Box(_pos, BASESIZE, BASESIZE, BASESIZE, BASEMASS),
            GoToFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreenDark"))
        {
            ModelScaling = new Vector3(Data.GOTO_FRAGSCALE);
            base.Type = Data.ProgrammType.GOTO;

            base.Coloring(new Vector3(0, 0, 200));
            if (mGlowLight == null) mGlowLight = _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreen");
            if (mGlowDark == null) mGlowDark = _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreenDark");
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
