using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Circuit.Screens;
using Microsoft.Xna.Framework;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    class DummieFactory : ProgrammFactory
    {
        public DummieFactory()
            : base()
        { }

        public override float CreateShot(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index) { return 0; }
        public override float CreateTrap(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index) { return 0; }
        public override float CreateBoost(DisposeHandler _space, GameScreen _SCREEN) { return 0; }

        public override void ReleaseShot(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space) { }
        public override void ReleaseTrap(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space) { }
        public override Boost ReleaseBoost(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space) { return null; }
    }
}
