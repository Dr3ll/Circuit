using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities.Counter;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    abstract class Trap : EntityModel, Run, Disposable
    {
        private int mLastCheck;
        protected bool mCheckingEnabled;
        protected CounterManager mCM = new CounterManager();
        protected string mColCounter = "col";
        protected string mNormCounter = "norm";
        public new event EventHandler Dispose;
        Vector3 mBaseRot = new Vector3(3.78f, 2.78f, .06f);
        private bool mDisposed = false;

        public Trap(
            BEPUphysics.Entities.Entity _entity,
            Microsoft.Xna.Framework.Graphics.Model _model,
            Game _GAME, Texture2D GlowTex)
            : base(_entity, _model, _GAME, GlowTex)
        {

        }

        public void Update(GameTime _GT, BEPUphysics.Entities.Entity _player)
        {
            if (mCheckingEnabled)
            {
                mLastCheck += _GT.ElapsedGameTime.Milliseconds;

                if (mLastCheck >= Data.TRAPCHECKTIME)
                {
                    Check(_player);
                }
            }
        }

        public void UpdatePos(Vector3 _pos, Vector3 _dir)
        {
            this.Position = _pos;
            Vector3 tRot = _dir + mBaseRot;
            //Transform = Matrix.CreateFromYawPitchRoll(tRot.Y, -tRot.X, tRot.Z);
        }

        protected virtual void Check(BEPUphysics.Entities.Entity _player)
        { }

        protected virtual void Trigger()
        { }

        public void DisposeThis()
        {
            if (!mDisposed)
                if (Dispose != null)
                {
                    Dispose(this, EventArgs.Empty);
                    mDisposed = true;
                }
        }
    }
}
