using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.Utilities;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using BEPUphysics;
using Circuit.ArenaClasses.Parts.Destructibles;

namespace Circuit.ArenaClasses.Destructibles
{
    class DestructibleBallCreator : DestructibleCreator
    {
        public DestructibleBallCreator(Game _GAME)
            : base(_GAME)
        {
        }

        /// <summary>
        /// Creates a globular DestructibleObject with standard radius.
        /// </summary>
        /// <param name="_type">
        /// The ProgrammType of which the Fragments are.</param>
        /// <returns></returns>
        public override DestructibleObject CreateDestructible(Data.ProgrammType _type)
        {
            throw new NotImplementedException();

            return CreateDestrucible(_type, Data.D_BALLRADIUS);
        }

        public DestructibleObject CreateDestrucible(Data.ProgrammType _type, int _radius)
        {
            List<Fragment> tParts = new List<Fragment>();

            throw new NotImplementedException();

            return new DestructibleBall(tParts.ToArray());
        }
    }
}
