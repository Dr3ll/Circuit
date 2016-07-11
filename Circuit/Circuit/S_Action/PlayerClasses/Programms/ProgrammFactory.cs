using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.ArenaClasses;
using Circuit.Screens;
using Circuit.Utilities;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    abstract class ProgrammFactory
    {
        protected Game GAME;
        protected Vector3 mShotPos;
        protected Vector3 mShotDir;
        protected Vector3 mShotRot;
        protected Run mCurRun;

        public ProgrammFactory(Game _GAME, Player _player)
        {
            mShotPos = _player.RelShotPosition + _player.Position;
            mShotDir = _player.ShotDirection;
            GAME = _GAME;
            _player.Moved += new EventHandler<PositionEventArgs>(OnPlayerMoved);
        }

        /// <summary>
        /// This constructor is for DummieFactory only.
        /// </summary>>
        public ProgrammFactory()
        { }

        public abstract float CreateShot(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index);
        public abstract float CreateTrap(DisposeHandler _space, GameScreen _SCREEN, PlayerIndex _index);
        public abstract float CreateBoost(DisposeHandler _space, GameScreen _SCREEN);

        public abstract void ReleaseShot(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space);
        public abstract void ReleaseTrap(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space);
        public abstract Boost ReleaseBoost(BEPUphysics.CollisionRuleManagement.CollisionGroup _group, DisposeHandler _space);

        protected void UpdateRunPos()
        {
            if (mCurRun != null)
                mCurRun.UpdatePos(mShotPos, mShotRot);
        }

        #region Subscribed events

        protected virtual void OnPlayerMoved(object _sender, PositionEventArgs _e)
        {
            mShotPos = _e.ShotPos;
            mShotDir = _e.Direction;
            mShotRot = _e.ShotRot;

            this.UpdateRunPos();
        }

        #endregion
    }
}
