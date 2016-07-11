using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses
{
    class PositionEventArgs : EventArgs
    {
        internal Vector3 Position;
        internal Vector2 Angles;
        internal Vector3 Direction;
        internal Vector3 ShotPos;
        internal Vector3 ShotRot;

        // Angles X - Azimut
        // Angles Y - Polar

        internal PositionEventArgs(Vector3 _pos, Vector3 _dir, Vector2 _ang, Vector3 _shotOff, Vector3 _shotRot)
            : base()
        {
            this.Position = _pos;
            this.Angles = _ang;
            this.ShotPos = _pos + _shotOff;
            this.Direction = _dir;
            this.ShotRot = new Vector3(_shotRot.Y, -_shotRot.X, _shotRot.Z);
        }
    }
}
