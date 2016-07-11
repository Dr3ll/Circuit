using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Circuit.S_Action.PlayerClasses.Programms;

namespace Circuit.ArenaClasses.Destructibles
{
    abstract class DestructiblesCreator
    {
        public abstract DestructibleObject CreateDestructible(Game _GAME, List<Fragment> _frags);
    }
}