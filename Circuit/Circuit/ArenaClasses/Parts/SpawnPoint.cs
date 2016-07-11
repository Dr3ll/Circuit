using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.ArenaClasses
{
    class SpawnPoint
    {
        Vector3 mPosition;

        public Vector3 Position
        {
            get { return mPosition; }
        }

        public SpawnPoint(Vector3 _pos)
        {
            mPosition = _pos;
        }
    }
}
