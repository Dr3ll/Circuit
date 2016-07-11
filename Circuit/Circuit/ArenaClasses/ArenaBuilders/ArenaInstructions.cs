using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.ArenaClasses.ArenaBuilders
{
    class ArenaInstructions
    {
        string mName;
        List<Vector3> mDPositionsBox;
        List<Vector3> mDOrientationsBox;
        List<Vector3> mDPositionsCylinder;
        List<Vector3> mDOrientationsCylinder;
        List<Vector3> mSpawnPoints;

        #region Interface

        public string           Name
        {
            get { return mName; }
        }
        public List<Vector3>[] Boxes
        {
            get { return new List<Vector3>[]{ mDPositionsBox, mDOrientationsBox }; }
        }
        public List<Vector3>[] Cylinders
        {
            get { return new List<Vector3>[] { mDPositionsCylinder, mDOrientationsCylinder }; }
        }
        public List<Vector3> Spawns
        {
            get { return mSpawnPoints; }
        }

        #endregion

        public ArenaInstructions(
            string _name, 
            List<Vector3> _dPosBOX, 
            List<Vector3> _dOriBOX,
            List<Vector3> _dPosCY,
            List<Vector3> _dOriCY,
            List<Vector3> _spawns)
        {
            this.mName = _name;
            this.mDPositionsBox = _dPosBOX;
            this.mDOrientationsBox = _dOriBOX;
            this.mDPositionsCylinder = _dPosCY;
            this.mDOrientationsCylinder = _dOriCY;
            this.mSpawnPoints = _spawns;
        }
    }
}
