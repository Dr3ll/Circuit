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

namespace Circuit.ArenaClasses.Destructibles
{
    class DestructibleBoxCreator : DestructibleCreator
    {
        public DestructibleBoxCreator(Game _GAME)
            : base(_GAME)
        {
        }

        /// <summary>
        /// Creates a rectangular DestructibleObject with standard dimensions.
        /// </summary>
        /// <param name="_type">
        /// The ProgrammType of which the Fragments are.</param>
        /// <returns></returns>
        public override DestructibleObject CreateDestructible(Data.ProgrammType _type)
        {
            return CreateDestrucible(_type, Data.D_BOXWIDTH, Data.D_BOXHEIGHT, Data.D_BOXDEPTH);
        }

        public DestructibleObject CreateDestrucible(Data.ProgrammType _type, int _width, int _height, int _depth)
        {
            List<Fragment> tParts = new List<Fragment>();
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    for (int z = 0; z < _depth; z++)
                    {
                        Fragment tPart = BuildPart(new Vector3(x * Data.D_BOXPARTSIZE,
                                                               y * Data.D_BOXPARTSIZE,
                                                               z * Data.D_BOXPARTSIZE),
                                                               _type);
                        tPart.Entity.BecomeKinematic();
                        tParts.Add(tPart);
                    }

            return new DestructibleBox(tParts.ToArray());
        }
    }
}
