using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities.Counter;
using Circuit.Utilities;
using Circuit.ArenaClasses;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Screens;
using BEPUphysics.CollisionRuleManagement;
using Microsoft.Xna.Framework.Input;

namespace Circuit.S_Action.PlayerClasses.Programms.Firewall
{
    class FirewallBoost : Boost
    {
        private DisposeHandler mSpace;
        private GameScreen mScreen;
        private List<FirewallChip> mChips;
        private Vector3 mPlayerPos = Vector3.Zero;
        private Vector3 mPlayerDir = Vector3.Zero;
        private Vector3 mChipRot = Vector3.Zero;
        private float mPolar;
        private float mChipMass = 50;
        private CollisionGroup mChipsCG;

        float cake = -.28f;
        float hohe = 1.335f;
        float mult = 1;

        public FirewallBoost(GameScreen _screen, DisposeHandler _space, Player _player)
        {
            base.mPower = Data.FIRE_BOOSTPOWER;
            mSpace = _space;
            mScreen = _screen;
            mChips = new List<FirewallChip>();

            _player.Moved += new EventHandler<PositionEventArgs>(OnPlayerMoved);
        }

        private void CreateChips()
        {
            float tLength = .18f;

            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-.505f), 1.0f, 5, 2.355f), .043f, .053f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.29f), 1.22f, 5, 2.135f), .036f, .034f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.685f), 1.68f, 5, 2.855f), .068f, .031f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-.28f), .81f, 5, 1.335f), .027f, .058f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-4.75f), -1.13f, 5, 1.475f), .049f, .038f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(.87f), 1.09f, 5, 2.385f), .085f, .051f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(.88f), .88f, 5, 3.065f), .02f, .02f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(.92f), .82f, 5, 1.225f), .044f, .045f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-.99f), 1.0f, 5, .955f), .024f, .027f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.71f), 1.5f, 5, .925f), .058f, .012f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.795f), 1.84f, 5, 1.855f), .025f, .046f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(1.155f), 1.14f, 5, .475f), .024f, .024f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(.045f), .68f, 5, .495f), .062f, .042f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.695f), 1.64f, 5, .345f), .049f, .048f, tLength));
            mChips.Add(Chip(new Offset(Offset.CalcRotFromPolar(-1.695f), 1.64f, 5, .345f), .049f, .048f, tLength));
        }

        private FirewallChip Chip(Offset _off, float _width, float _height, float _length)
        {
            Entity tEntity = new BEPUphysics.Entities.Prefabs.Box(
                        mPlayerPos + _off.RelPosition(),
                        _width,
                        _height,
                        _length,
                        mChipMass);

            FirewallChip tChip = new FirewallChip(_off, tEntity, FirewallFactory.GetModelCopy(), mScreen.Game, null, mChipsCG);

            tChip.ModelScaling = new Vector3(_width ,
                                             _height,
                                             _length * Data.FIRE_RATIOKEY);

            return tChip;
        }

        public void SetCollisionGroup(CollisionGroup _cg)
        {
            mChipsCG = _cg;
        }

        public override void Start()
        {
            base.Start();

            CreateChips();

            foreach (FirewallChip _c in mChips)
            {
                mScreen.Add(_c);
                _c.AddToSpace(mSpace);
            }

            int cake;
        }


        public override void Update(GameTime _GT)
        {
            base.Update(_GT);
            FirewallChip tChip = null;
            foreach (FirewallChip _c in mChips)
            {
                _c.Update(_GT, mPlayerPos, mPolar, mChipRot, mPlayerDir);

                tChip = _c;
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.M))
            //    cake += .005f;
            //if (Keyboard.GetState().IsKeyDown(Keys.N))
            //    cake -= .005f;

            //if (Keyboard.GetState().IsKeyDown(Keys.I))
            //    hohe += .01f;
            //if (Keyboard.GetState().IsKeyDown(Keys.K))
            //    hohe -= .01f;

            //if (Keyboard.GetState().IsKeyDown(Keys.D))
            //    tChip.ModelScaling = new Vector3(tChip.ModelScaling.X + .001f, tChip.ModelScaling.Y, tChip.ModelScaling.Z);
            //if (Keyboard.GetState().IsKeyDown(Keys.A))
            //    tChip.ModelScaling = new Vector3(tChip.ModelScaling.X - .001f, tChip.ModelScaling.Y, tChip.ModelScaling.Z);

            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    tChip.ModelScaling = new Vector3(tChip.ModelScaling.X, tChip.ModelScaling.Y + .001f, tChip.ModelScaling.Z);
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    tChip.ModelScaling = new Vector3(tChip.ModelScaling.X, tChip.ModelScaling.Y - .001f, tChip.ModelScaling.Z);

            //if (Keyboard.GetState().IsKeyDown(Keys.R))
            //    mult += .01f;
            //if (Keyboard.GetState().IsKeyDown(Keys.F))
            //    mult  -= .01f;


            //tChip.mOffset.mBaseRotation = Offset.CalcRotFromPolar(cake);
            //tChip.mOffset.mBaseRotation *= mult;
            //tChip.mOffset.mBaseRotation.Y = hohe;
        }

        #region Subscribed Events

        protected virtual void OnPlayerMoved(object _sender, PositionEventArgs _e)
        {
            mPlayerPos = _e.Position;
            mPolar = _e.Angles.Y;
            mChipRot = _e.ShotRot;
            mPlayerDir = _e.Direction;
        }

        protected override void OnCounterUsedUp(object _sender, EventArgs _e)
        {
            foreach (FirewallChip _c in mChips)
            {
                _c.DisposeThis();
            }
            mChips.Clear();

            base.OnCounterUsedUp(_sender, _e);
        }

        #endregion
    }
}