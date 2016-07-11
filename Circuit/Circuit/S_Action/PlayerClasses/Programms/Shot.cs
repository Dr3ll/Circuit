using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities;
using Circuit.Utilities.Counter;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    abstract class Shot : EntityModel, Run, Disposable
    {
        private CounterManager mCM = new CounterManager();
        public new event EventHandler Dispose;
        string mColCounter = "col";
        string mNormCounter = "norm";
        static Vector3 mBaseRot = new Vector3(2.62f, 2.62f, .02f);

        public Shot(
            BEPUphysics.Entities.Entity _entity,
            Model _model,
            Game _GAME, Texture2D GlowTex)
            : base(_entity, _model, _GAME, GlowTex)
        {
            mCM.AddCounter(mColCounter, Data.A_SHOTDELETIONTIME_ONCOL);
            mCM.AddCounter(mNormCounter, Data.A_SHOTDELETIONTIME);

            RefreshSubscription();
            mCM.Bang += OnBang;

            mCM.StartCounter(mNormCounter, false);
        }

        protected void RefreshSubscription()
        {
            Entity.CollisionInformation.Events.ContactCreated += OnCollision;
        }

        public override void Update(GameTime _GT)
        {
            mCM.Update(_GT);
        }

        public virtual void UpdatePos(Vector3 _pos, Vector3 _dir)
        {
            this.Position = _pos;
            Vector3 tRot = _dir + mBaseRot;
            Transform = Matrix.CreateFromYawPitchRoll(tRot.X, tRot.Y, tRot.Z);
        }

        private void OnCollision(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            mCM.StartCounter(mColCounter, false);
            mCM.CancelCounter(mNormCounter);
        }

        private void OnBang(object _sender, EventArgs _e)
        {
            if (Dispose != null)
                Dispose(this, EventArgs.Empty);
        }
    }
}
