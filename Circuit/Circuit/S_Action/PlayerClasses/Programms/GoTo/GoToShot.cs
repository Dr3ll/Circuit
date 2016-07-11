using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Collidables.MobileCollidables;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToShot : Shot
    {
        Vector3 force = Vector3.Zero;
        float mAngle = 0;

        public GoToShot(string _path, Game _GAME, Vector3 _pos, Quaternion _ori, PlayerIndex _index)
            : base(GoToFactory.GetPhysicalEntity(Data.GOTO_SHOTENTITYSCALE),
            GoToFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreen")
            )
        {
            Entity.Position = _pos;
            Entity.Orientation = _ori;

            ModelScaling = new Vector3(Data.GOTO_SHOTMODELSCALE);
            EntityScaling = Data.GOTO_SHOTENTITYSCALE;

            base.Coloring(new Vector3(0, 0, 255));

            this.Entity.CollisionInformation.Tag = new CollisionTag(Data.RunType.SHOT, Data.ProgrammType.GOTO, this.EntityScaling, _index);
        }

        public void ProcessHoming(Vector3 _target)
        {
            float tAdjustAmount = .01f;

            Vector3 tToTarget = _target - this.Position;
            Vector3 tThisDirection = Entity.LinearVelocity;

            float tDistance = tToTarget.LengthSquared();
            tDistance = 400;

            tToTarget.Normalize();
            tThisDirection.Normalize();

            float tMaxAngle = Data.GOTO_SHOTHOMINGMINANGLE;

            float tAngle = (float)(Vector3.Dot(tToTarget, tThisDirection));
            Debug.Line1 = mAngle;
            
            if (mAngle == 0)
                mAngle = tAngle;

          //if (TestDistance(_target))
            if (tAngle >= tMaxAngle && tAngle > 0)
            {
                Vector3 tAdjustDirection = Vector3.Cross(tThisDirection, Vector3.Cross(tToTarget, tThisDirection));

                force = tAdjustDirection * (tAdjustAmount * tDistance * mAngle);

                Entity.ApplyImpulse(Vector3.Zero, force);
            }
        }

        private bool TestDistance(Vector3 _target)
        {
            return true;
        }

        private bool TestAngle(Vector3 _toTarget, Vector3 _thisDirection)
        {
            float tMaxAngle = 100.5f;

            float tAngle = (float)Math.Abs(Vector3.Dot(_toTarget, _thisDirection));

            return tAngle <= tMaxAngle;
        }
    }
}