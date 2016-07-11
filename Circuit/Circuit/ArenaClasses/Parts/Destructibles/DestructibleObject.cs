using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.S_Action.PlayerClasses.Programms;
using BEPUphysics;
using Circuit.Screens;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionRuleManagement;
using Circuit.Utilities;

namespace Circuit.ArenaClasses.Destructibles
{
    abstract class DestructibleObject
    {
        protected Vector3 mPosition;
        protected Vector3 mOrientation;
        protected Fragment[] mParts;

        protected DestructibleObject(Fragment[] _parts)
        {
            mParts = _parts;
        }

        public void Initialize(Vector3 _pos, Vector3 _ori)
        {
            mPosition = _pos;

            for (int i = 0; i < mParts.GetLength(0); i++)
            {
                mParts[i].Position += _pos;
                mParts[i].Transform = Matrix.CreateFromYawPitchRoll(_ori.X, _ori.Y, _ori.Z);
                mParts[i].Entity.CollisionInformation.Events.ContactCreated += OnCollision;
            }
        }

        private void OnCollision(object _sender, BEPUphysics.Collidables.Collidable _other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair, BEPUphysics.CollisionTests.ContactData _contact)
        {
            if (_other is EntityCollidable)
            {
                if ((_other as EntityCollidable).Tag != null &&
                    !(_other as EntityCollidable).Tag.Equals("_player_one") &&
                    !(_other as EntityCollidable).Tag.Equals("_player_two"))
                {
                    if ((_other as EntityCollidable).Tag is CollisionTag &&
                        ((_other as EntityCollidable).Tag as CollisionTag).RunType.Equals(Data.RunType.SHOT))
                    {
                        for (int i = 0; i < mParts.GetLength(0); i++)
                        {
                            Vector3 tRange = mParts[i].Position - (_sender as BEPUphysics.Collidables.MobileCollidables.ConvexCollidable).Entity.Position;
                            if (tRange.Length() < .01f)
                            {
                                mParts[i].Entity.BecomeDynamic(1);
                                mParts[i].MakeCollectable();
                            }
                        }
                    }
                }
            }
            else
            {
                //(_sender as BEPU.Collidables.MobileCollidables.ConvexCollidable).Entity.BecomeKinematic();
            }
        }

        public void AddToSpace(Space _space, CollisionGroup _group)
        {
            for (int i = 0; i < mParts.GetLength(0); i++)
            {
                mParts[i].Entity.CollisionInformation.CollisionRules.Group = _group;
                _space.Add(mParts[i].Entity);
            }
        }

        public void AddDrawables(GameScreen _SCREEN)
        {
            for (int i = 0; i < mParts.GetLength(0); i++)
                _SCREEN.Add(mParts[i]);
                    
        }

        public void AddFragments(List<Fragment> _frags)
        {
            for (int i = 0; i < mParts.GetLength(0); i++)
                _frags.Add(mParts[i]);
                    
        }
    }
}