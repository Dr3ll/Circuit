using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.Utilities;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using Circuit.S_Action.PlayerClasses.Programms.GoTo;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;
using Circuit.S_Action.PlayerClasses.Programms.Optimize;

namespace Circuit.ArenaClasses.Destructibles
{
    abstract class DestructibleCreator
    {
        Game GAME;

        protected DestructibleCreator(Game _GAME)
        {
            GAME = _GAME;
        }

        public abstract DestructibleObject CreateDestructible(Data.ProgrammType _type);

        protected Fragment BuildPart(Vector3 _pos, Data.ProgrammType _type)
        {
            switch (_type)
            {
                case Data.ProgrammType.DELETE:
                    return new DeleteFragment(_pos, GAME);
                case Data.ProgrammType.OPTIMIZE:
                    return new OptimizeFragment(_pos, GAME);
                case Data.ProgrammType.FIREWALL:
                    return new FirewallFragment(_pos, GAME);
                case Data.ProgrammType.GOTO:
                    return new GoToFragment(_pos, GAME);
            }

            throw new Exception();
        }
    }
}