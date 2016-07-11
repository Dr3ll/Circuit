using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Circuit.Screens;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;

namespace Circuit.Utilities
{
    class DisposeHandler
    {
        // The DisposeHandler is used to handle the deletion of specific objects (Disposables) during runtime.
        // To clear the existence of an instance, it is removed from physical space (Space) aswell as visual space (GameScreen)
        // Currently its used to delete generic Shots after they collided.

        Space mSpace;
        GameScreen mScreen;
        List<S_Action.PlayerClasses.Programms.GoTo.GoToShot> mGoToShotsP_ONE;
        List<S_Action.PlayerClasses.Programms.GoTo.GoToShot> mGoToShotsP_TWO;
        EventHandler mDisposeHandler;
        List<Disposable> mDisposables;

        public Space Space
        {
            get { return mSpace; }
        }

        public DisposeHandler(
            Space _space,
            GameScreen _screen)
        {
            mDisposables = new List<Disposable>();
            mSpace = _space;
            mScreen = _screen;
            mDisposeHandler = new EventHandler(OnDispose);
        }

        public void SetGoToLists(
            List<S_Action.PlayerClasses.Programms.GoTo.GoToShot> _goToShotsP_ONE,
            List<S_Action.PlayerClasses.Programms.GoTo.GoToShot> _goToShotsP_TWO)
        {
            mGoToShotsP_ONE = _goToShotsP_ONE;
            mGoToShotsP_TWO = _goToShotsP_TWO;
        }

        public void AddForDraw(EntityModel _entity)
        {
            mScreen.Add(_entity);
        }

        public void Add(EntityModel _entity)
        {
            mSpace.Add(_entity.Entity);

            if (_entity is Disposable)
            {
                (_entity as Disposable).Dispose += mDisposeHandler;
                mDisposables.Add(_entity as Disposable);
            }
        }

        public void Add(BEPUphysics.Constraints.SingleEntity.SingleEntityConstraint _constraint)
        {
            mSpace.Add(_constraint);
        }

        public void Add(BEPUphysics.Constraints.TwoEntity.Joints.Joint _joint)
        {
            mSpace.Add(_joint);
        }

        public void Add(MotorizedGrabSpring _spring)
        {
            mSpace.Add(_spring);

            if (_spring is Disposable)
            {
                (_spring as Disposable).Dispose += mDisposeHandler;
                mDisposables.Add(_spring as Disposable);
            }
        }

        public void Remove(BEPUphysics.Entities.Entity _entity)
        {
            mSpace.Remove(_entity);
        }

        public void RemoveFromDraw(EntityModel _entity)
        {
            mScreen.Remove(_entity);
        }

        // The Update is needed because (some) Disposables are delete after a countdown
        public void Update(GameTime _GT)
        {
            try
            {
                foreach (Disposable _d in mDisposables)
                {
                    _d.Update(_GT);
                }
            }
            catch { }
        }

        private void OnDispose(object _sender, EventArgs _e)
        {
            if ((_sender as EntityModel) != null)
            {
                mSpace.Remove((_sender as EntityModel).Entity);
                mScreen.Remove(_sender as EntityModel);
            }
            else
            {
                mSpace.Remove(_sender as MotorizedGrabSpring);
            }

            if (_sender is S_Action.PlayerClasses.Programms.GoTo.GoToShot)
            {
                mGoToShotsP_ONE.Remove(_sender as S_Action.PlayerClasses.Programms.GoTo.GoToShot);
                mGoToShotsP_TWO.Remove(_sender as S_Action.PlayerClasses.Programms.GoTo.GoToShot);
            }
            
            this.mDisposables.Remove(_sender as Disposable);
        }
    }
}