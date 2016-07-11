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
    class DestructibleCylinderCreator : DestructibleCreator
    {
        public DestructibleCylinderCreator(Game _GAME)
            : base(_GAME)
        {
        }

        /// <summary>
        /// Creates a cylindric DestructibleObject with standard dimensions.
        /// </summary>
        /// <param name="_type">
        /// The ProgrammType of which the Fragments are.</param>
        /// <returns></returns>
        public override DestructibleObject CreateDestructible(Data.ProgrammType _type)
        {
            return CreateDestrucible(_type, Data.D_CYLINDERRADIUS, Data.D_CYLINDERHEIGHT);
        }

        public DestructibleObject CreateDestrucible(Data.ProgrammType _type, float _radius, int _height)
        {
            List<Fragment> tParts = new List<Fragment>();

            int tNumberofRings = 3;
            int tFirstRingCount = 28;
            int tDegration = 0;
            float tRadiusDegration = .26f;
            float tHeightIncrement = .3f;

            float tAngleBase = (float)((Math.PI * 2) / tFirstRingCount);

            for (int h = 0; h < _height; ++h)
                for (int r = 0; r < tNumberofRings - 1; ++r)
                    for (int i = 0; i < tFirstRingCount - r * tDegration; ++i)
                    {
                        float tAngle = tAngleBase * i;
                        float tRadius = _radius - r * tRadiusDegration;
                        Vector3 tPos = new Vector3(tRadius * (float)Math.Cos(tAngle),
                                                   tHeightIncrement * h,
                                                   tRadius * (float)Math.Sin(tAngle));

                        Fragment tPart = BuildPart(tPos, _type);
                        tPart.Entity.BecomeKinematic();
                        tParts.Add(tPart);
                    }

            return new DestructibleCylinder(tParts.ToArray());
        }
    }
}
