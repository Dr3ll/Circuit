using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallFragment : Circuit.S_Action.PlayerClasses.Programms.Fragment
    {
        public static Texture2D mGlowLight;
        public static Texture2D mGlowDark;
        
        public FirewallFragment(Vector3 _pos, Game _GAME)
            : base(new BEPUphysics.Entities.Prefabs.Box(_pos, BASESIZE, BASESIZE, BASESIZE, BASEMASS),
            FirewallFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/Würfel/GlowCyanDark"))
        {
            ModelScaling = new Vector3(Data.FIRE_FRAGSCALE);
            base.Type = Data.ProgrammType.FIREWALL;

            base.Coloring(new Vector3(0,255,0));
            if (mGlowLight == null) mGlowLight = _GAME.Content.Load<Texture2D>("Programms/Würfel/GlowCyan");
            if (mGlowDark == null) mGlowDark = _GAME.Content.Load<Texture2D>("Programms/Würfel/GlowCyanDark");
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
