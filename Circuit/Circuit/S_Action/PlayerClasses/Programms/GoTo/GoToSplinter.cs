using System;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Circuit.Utilities.Counter;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Constraints.SingleEntity;

namespace Circuit.S_Action.PlayerClasses.Programms.GoTo
{
    class GoToSplinter : EntityModel, Disposable
    {
        private CounterManager mCM = new CounterManager();
        public new event EventHandler Dispose;
        string mNormCounter = "norm";
        public SingleEntityLinearMotor mLinearMotor;

        public GoToSplinter(Game _GAME)
            : base(
            GoToFactory.GetPhysicalEntity(Data.GOTO_SPLINTERENTITYSCALE),
            GoToFactory.GetModelCopy(),
            _GAME,
            _GAME.Content.Load<Texture2D>("Programms/GoTo/GlowGreen"))
        {
            mCM.AddCounter(mNormCounter, Data.A_SHOTDELETIONTIME);
            ModelScaling = new Vector3(Data.GOTO_SPLINTERENTITYSCALE);
            Entity.IsAffectedByGravity = true;

            mLinearMotor = new SingleEntityLinearMotor(Entity, this.Entity.Position);
            mLinearMotor.IsActive = true;
            mLinearMotor.Settings.Servo.SpringSettings.StiffnessConstant = 6000000 * mLinearMotor.Entity.Mass;
            mLinearMotor.Settings.Servo.SpringSettings.DampingConstant = 90000 * mLinearMotor.Entity.Mass;
            mLinearMotor.Settings.Servo.Goal = this.Entity.Position;
            mLinearMotor.Settings.MaximumForce = 100000 * mLinearMotor.Entity.Mass;
            mLinearMotor.Settings.Mode = BEPUphysics.Constraints.TwoEntity.Motors.MotorMode.Servomechanism;

            Entity.CollisionInformation.Events.ContactCreated += OnCollision;
        }

        public void UpdatePos(Vector3 _pos)
        {
            this.Position = _pos;
        }

        public void Initialize(DisposeHandler _space)
        {
            mCM.StartCounter(mNormCounter, false);

            _space.Add(this);
            _space.AddForDraw(this);
            _space.Add(mLinearMotor);
        }

        private void OnCollision(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            if(this.Entity.LinearVelocity.LengthSquared() < .001f)
                (Entity.CollisionInformation.Tag as CollisionTag).ProgramType = Data.ProgrammType.DEFAULT;
        }

        public void DisposeThis()
        {
            if (Dispose != null)
                Dispose(this, EventArgs.Empty);
        }
    }
}
