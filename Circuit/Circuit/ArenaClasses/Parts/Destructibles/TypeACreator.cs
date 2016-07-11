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
    class TypeACreator : DestructiblesCreator
    {
        public override DestructibleObject CreateDestructible(Game _GAME, List<Fragment> _frags)
        {
            Fragment[, ,] tParts = new Fragment[Data.D_TYPEAWIDTH, Data.D_TYPEAHEIGHT, Data.D_TYPEADEPTH];
            for (int x = 0; x < tParts.GetLength(0); x++)
                for (int y = 0; y < tParts.GetLength(1); y++)
                    for (int z = 0; z < tParts.GetLength(2); z++)
                    {
                        Fragment tPart = new DeleteFragment(new Vector3(x * Data.D_TYPEAPARTSIZE,
                                                                        y * Data.D_TYPEAPARTSIZE,
                                                                        z * Data.D_TYPEAPARTSIZE),
                                                                        _GAME);
                        tPart.Entity.BecomeKinematic();
                        tParts[x, y, z] = tPart;
                        _frags.Add(tPart);
                    }

            return new DestructibleTypeA(tParts);
        }
    }
}
