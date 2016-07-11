using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionRuleManagement;
using Microsoft.Xna.Framework.Input;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallChip : EntityModel, Disposable
    {
        public new event EventHandler Dispose;

        public Offset mOffset;
        MotorizedGrabSpring mHolder;
        float mRotDitch = 0;

        public FirewallChip(Offset _off, Entity _entity, Model _model, Game _GAME, Texture2D _glowT, CollisionGroup _cg)
            : base(_entity, _model, _GAME, _glowT)
        {
            mOffset = _off;
            mHolder = new MotorizedGrabSpring();
            this.Entity.CollisionInformation.CollisionRules.Group = _cg;

            mHolder.Setup(this.Entity, this.Position);
        }

        public void AddToSpace(DisposeHandler _space)
        {
            _space.Add(this);
            _space.Add(mHolder);
        }

        private Vector3 CalcTrapOrientation(Vector3 _shotRot)
        {
            float tRotCorrection = -.465f;

            return new Vector3(_shotRot.X + tRotCorrection, 0, _shotRot.Z);
        }

        public void Update(GameTime gameTime, Vector3 _pos, float _polar, Vector3 _rot, Vector3 _dir)
        {
            base.Update(gameTime);

            //Vector3 tRot = _dir + mBaseRot;
            //Transform = Matrix.CreateFromYawPitchRoll(tRot.X, tRot.Y, tRot.Z);

            Vector3 tToPlayer = this.Position - _pos;
            tToPlayer.Y = 0;
            mRotDitch = (float)Vector3.Dot(new Vector3(_dir.X, 0, _dir.Z), tToPlayer);

            mOffset.Update(new Vector3(0, _polar, 0));
            mHolder.GoalPosition = _pos + mOffset.RelPosition();
            mHolder.GoalOrientation = CalcTrapOrientation(_rot);
        }

        public void DisposeThis()
        {
            mHolder.Release();
            mHolder.DisposeThis();

            if (Dispose != null)
                Dispose(this, EventArgs.Empty);
        }
    }
}
