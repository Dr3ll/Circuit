using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Circuit.Utilities.Cameras;
using BEPUphysics;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Screens;
using Circuit.ArenaClasses;
using Microsoft.Xna.Framework.Audio;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;
using Circuit.Utilities.Counter;

namespace Circuit.S_Action.PlayerClasses
{
    class Player
    {
        bool mIsAlive;
        bool swap;
        float mLifePoints;

        PlayerIndex mIndex;
        Camera mCamera;
        Arena mArena;
        MouseState mOldMouse;
        KeyboardState mOldKeys;
        Vector3 mModelDirection;
        Vector3 mModelShotDirection;
        public EntityModel mModel;
        Texture2D[] mTextureArray;
        bool mFirewallInvuln;
        CounterManager mVibrationCounter;

        SoundEffect[] mSounds;
        SoundEffectInstance[] mSoundInstances;

        Character.CharacterControllerInput mController;
        Matrix mModelTranslation = Matrix.CreateTranslation(new Vector3(0, -3.71f, 0));
        ParameterSet mParameter;

        GameScreen mScreen;

        float mRadius = 2;
        float mPolar = 0;
        float mAzimut = 1;
        float mCamInvertX;
        float mCamInvertY;
        float mCamSensitivity;
        float mCamPosAdjust = 1.26f;
        float mCamTargetAdjust = 1.637f;
        Vector3 mCamPosOffset = new Vector3(0.382f, 4.50f, 0.49f);
        Vector3 mCamTargetOffset = new Vector3(2.8f, .48f, -2.18f);

        float mShotOffsetRadius = 3f;
        Vector3 mShotPosOffset = new Vector3(-.88f, .8f, 1.56f);
        float mShotOffsetY = 1.4f;
        Offset mShotOffset;

        Quaternion mCamOriTwist = Quaternion.CreateFromYawPitchRoll(1f, 1.1f, .4f);

        Circuit.S_Action.PlayerClasses.Arsenal mArsenal;

        #region Events

        public delegate void PlayerDeathHandler(Player _player, EventArgs e);
        public event PlayerDeathHandler Death;
        public event PlayerDeathHandler Suicide;
        public event PlayerDeathHandler Respawn;

        public event EventHandler<PositionEventArgs> Moved;
        // These are interim events for the controls until the controls class is operatable

        #endregion

        #region Interface

        public enum SoundEffects { CHARGING, SHOOTING, JUMPING, DYING, RESPAWNING, HITTING, BOOSTING }

        public Vector3 Position
        {
            get { return mController.Position; }
            set
            {
                mController.Position = value;
            }
        }

        public PlayerIndex Index
        {
            get { return mIndex; }
            set { mIndex = value; }
        }

        public Vector3 Direction
        {
            get { return -mModelDirection; }
        }

        public Vector3 ShotDirection
        {
            get { return mModelShotDirection; }
        }

        public Vector3 RelShotPosition
        {
            get { return mShotOffset.RelPosition(); }
        }

        public BEPUphysics.Entities.Entity Entity
        {
            get { return mController.CharacterController.Body; }
        }

        public EventHandler<PositionEventArgs> EMhandler
        {
            get { return mArsenal.EMhandler; }
        }

        public List<Programms.GoTo.GoToShot> GoToShots
        {
            get { return mArsenal.GoToShots; }
        }
        public Camera CAM
        {
            get { return mCamera; }
        }

        #endregion

        public Player(PlayerIndex _index, ArenaClasses.Arena _arena, GameScreen _SCREEN, ControlClass _CONTROLCLASS, SoundEffect[] _SOUNDS, Texture2D[] _GLOWTEXTURES)
        {
            mIndex = _index;
            mArena = _arena;
            mScreen = _SCREEN;
            mSounds = _SOUNDS;
            mTextureArray = _GLOWTEXTURES;

            // Call Setups
            SetupSound();
            SetupCController();
            SetupWeaponSystems();
            SetupEventSubscriptions(_CONTROLCLASS);
            SetupModel();

            mCamera = new ChaseCamera(mCamPosOffset * mCamPosAdjust, mCamTargetOffset, Vector3.Zero, 2 * ((float)Data.R_VIEWPORTWIDTH / (float)Data.R_VIEWPORTHEIGHT), _SCREEN.GraphicsDevice);

            Initialize();
        }

        private void SetupSound()
        {
            /*      SoundEffekt Indizes
             *      0 - Schuss aufladen
             *      1 - Schießen
             *      2 - Springen
             *      3 - Sterben
             *      4 - Respawn
             *      5 - Hit
             *      6 - Boost
             */

            mSoundInstances = new SoundEffectInstance[mSounds.Length];
            for (int i = 0; i < mSoundInstances.Length; i++) mSoundInstances[i] = mSounds[i].CreateInstance();

            mSoundInstances[(int)SoundEffects.CHARGING].Volume = Data.S_LOADVOL * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.SHOOTING].Volume = Data.S_SHOOTVOL * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.JUMPING].Volume = Data.S_JUMPVOL * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.DYING].Volume = Data.S_DIEVOL * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.RESPAWNING].Volume = Data.S_RESPAWNVOL * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.HITTING].Volume = Data.S_HIT * Data.S_MASTERVOLUME;
            mSoundInstances[(int)SoundEffects.BOOSTING].Volume = Data.S_BOOST * Data.S_MASTERVOLUME;

            mVibrationCounter = new CounterManager();
            mVibrationCounter.Bang += new EventHandler(OnVibrationBang);
        }

        private void SetupCController()
        {
            mController = new Character.CharacterControllerInput(mArena.Space.Space);

            if (mIndex == PlayerIndex.One)
                mController.CollisionInformation.CollisionRules.Group = mArena.PlayerOneCG;
            if (mIndex == PlayerIndex.Two)
                mController.CollisionInformation.CollisionRules.Group = mArena.PlayerTwoCG;

            mController.CharacterController.HorizontalMotionConstraint.Speed = Data.P_BASESPEED;
            mController.CharacterController.JumpSpeed = Data.P_BASEJUMP;

            if (mIndex == PlayerIndex.One)
            {
                mController.CharacterController.Body.Tag = new CollisionTag(Data.RunType.PLAYER, Data.ProgrammType.DEFAULT, 0, mIndex);
                mCamSensitivity = Data.O_CAMERA_SENSITIVITY1;
                if (Data.O_INVERT_X1) mCamInvertX = -1; else mCamInvertX = 1;
                if (Data.O_INVERT_Y1) mCamInvertY = -1; else mCamInvertY = 1;
            }

            if (mIndex == PlayerIndex.Two)
            {
                mController.CharacterController.Body.Tag = new CollisionTag(Data.RunType.PLAYER, Data.ProgrammType.DEFAULT, 0, mIndex);
                mCamSensitivity = Data.O_CAMERA_SENSITIVITY2;
                if (Data.O_INVERT_X2) mCamInvertX = -1; else mCamInvertX = 1;
                if (Data.O_INVERT_Y2) mCamInvertY = -1; else mCamInvertY = 1;
            }
        }

        private void SetupEventSubscriptions(ControlClass _CONTROLCLASS)
        {
            #region Parameter Events
            mParameter = new ParameterSet(mArsenal.BoostManager);
            mParameter.SetDeleteCollision += new EventHandler(OnSetDeleteCollision);
            mParameter.DeSetDeleteCollision += new EventHandler(OnDeSetDeleteCollision);

            mParameter.SetOptimizeSpeedBuff += new EventHandler(OnSetSpeedBuff);
            mParameter.DeSetOptimizeSpeedBuff += new EventHandler(OnDeSetSpeedBuff);

            mParameter.SetOptimizeSpeedDeBuff += new EventHandler(OnSetSpeedDeBuff);
            mParameter.DeSetOptimizeSpeedDeBuff += new EventHandler(OnDeSetSpeedDeBuff);

            mParameter.SetGotoJumpBuff += new EventHandler(OnSetJumpBuff);
            mParameter.DeSetGotoJumpBuff += new EventHandler(OnDeSetJumpBuff);

            mParameter.SetFirewallInvulnBuff += new EventHandler(OnSetInvulnBuff);
            mParameter.DeSetFirewallInvulnBuff += new EventHandler(OnDeSetInvulnBuff);
            #endregion

            #region Collision Detection
            mController.CharacterController.Body.CollisionInformation.Events.ContactCreated += OnContactCreated;
            #endregion

            #region Controller Events
            _CONTROLCLASS.ePressDPadUp += new ControlClass.ControllerEventHandler(OnPressDPadUp);
            _CONTROLCLASS.ePressDPadLeft += new ControlClass.ControllerEventHandler(OnPressDPadLeft);
            _CONTROLCLASS.ePressDPadRight += new ControlClass.ControllerEventHandler(OnPressDPadRight);
            _CONTROLCLASS.ePressDPadDown += new ControlClass.ControllerEventHandler(OnPressDPadDown);

            _CONTROLCLASS.ePressAButton += new ControlClass.ControllerEventHandler(OnPressAButton);
            _CONTROLCLASS.ePressBButton += new ControlClass.ControllerEventHandler(OnPressBButton);
            _CONTROLCLASS.ePressXButton += new ControlClass.ControllerEventHandler(OnPressXButton);
            _CONTROLCLASS.ePressYButton += new ControlClass.ControllerEventHandler(OnPressYButton);

            _CONTROLCLASS.ePressLeftTrigger += new ControlClass.ControllerEventHandler(OnPressLeftTrigger);
            _CONTROLCLASS.ePressLeftButton += new ControlClass.ControllerEventHandler(OnPressLeftButton);
            _CONTROLCLASS.ePressRightTrigger += new ControlClass.ControllerEventHandler(OnPressRightTrigger);
            _CONTROLCLASS.ePressRightButton += new ControlClass.ControllerEventHandler(OnPressRightButton);

            _CONTROLCLASS.eMoveLeftStick += new ControlClass.ControllerEventHandler(OnMoveLeftStick);

            _CONTROLCLASS.eMoveRightStickUp += new ControlClass.ControllerEventHandler(OnMoveRightStickUp);
            _CONTROLCLASS.eMoveRightStickLeft += new ControlClass.ControllerEventHandler(OnMoveRightStickLeft);
            _CONTROLCLASS.eMoveRightStickRight += new ControlClass.ControllerEventHandler(OnMoveRightStickRight);
            _CONTROLCLASS.eMoveRightStickDown += new ControlClass.ControllerEventHandler(OnMoveRightStickDown);

            _CONTROLCLASS.ePressStart += new ControlClass.ControllerEventHandler(OnPressStart);
            _CONTROLCLASS.ePressBack += new ControlClass.ControllerEventHandler(OnPressBack);
            #endregion
        }

        private void SetupModel()
        {
            mModel = new EntityModel(new BEPUphysics.Entities.Prefabs.Sphere(Vector3.Zero, 2, 10), mArena.Screen.Game.Content.Load<Model>("Bob/Bob"), mArena.Screen.Game, mTextureArray[0]);
            mModel.ModelScaling = new Vector3(Data.P_MODELSCALE);
            mModel.Entity.LinearDamping = .7f;
            mModel.Entity.AngularDamping = .9f;
            mModel.Transform += mModelTranslation;
            mModel.mGlowEffect = mArena.Screen.Game.Content.Load<Effect>("Effects/ExtraGlowEffect");
            mArena.Screen.Add(mModel);
        }

        private void SetupWeaponSystems()
        {
            mArsenal = new Circuit.S_Action.PlayerClasses.Arsenal(mArena, this);

            mArena.Space.Add(mArsenal.Grabber);

            mShotOffset = new Offset(mShotPosOffset, mShotOffsetRadius, mShotOffsetY);
        }

        public void Initialize()
        {
            mIsAlive = true;
            mLifePoints = Data.P_LIFEPOINT_BASE;
        }

        public void Spawn(Vector3 _pos)
        {
            mController.Position = _pos;
            mController.CharacterController.TeleportToPosition(_pos, 1);
        }

        public void InflictDamage(float _damage)
        {
            if (!mIsAlive)
                GamePad.SetVibration(mIndex, 0, 0);
            else
            {
                mLifePoints -= _damage;
                Vibrate();

                if (!mFirewallInvuln)
                {
                    if (mLifePoints <= 0 && mIsAlive)   //Player is dead, Inform ActionScreen and stop non-wanted player Input via mIsAlive
                    {
                        mIsAlive = false;
                        if (Death != null)
                        {
                            mSoundInstances[(int)SoundEffects.DYING].Play();
                            Death(this, EventArgs.Empty);
                        }
                    }
                }
                else { Vibrate(); }
            }
        }

        private void Vibrate()
        {
            float tVib = .5f;
            GamePad.SetVibration(mIndex, tVib, tVib);
            mVibrationCounter.StartCounter(500);
        }

        #region Update

        public void UpdateLifeGlow()
        {
            float tUpperFactor = 2 * (mLifePoints / Data.P_LIFEPOINT_BASE) - 1;
            float tLowerFactor = mLifePoints / (Data.P_LIFEPOINT_BASE / 2);

            if (mLifePoints > Data.P_LIFEPOINT_BASE / 2) mModel.mGlowColor1 = tUpperFactor * Data.PG_STARTLIFE + (1 - tUpperFactor) * Data.PG_MIDDLELIFE;
            else mModel.mGlowColor1 = tLowerFactor * Data.PG_MIDDLELIFE + (1 - tLowerFactor) * Data.PG_ENDLIFE;
        }

        public void Update(GameTime _GT, List<Fragment> _frags)
        {
            mController.Update(_GT);

            DebugControls();

            TransformModel(new Vector3(mAzimut, mPolar, 0));

            mVibrationCounter.Update(_GT);

            mShotOffset.Update(new Vector3(mAzimut, mPolar, 0));

            CalcModelDirection(mAzimut, mPolar);

            (mCamera as ChaseCamera).Move(Position, new Vector3(mAzimut, mPolar, 0));
            mCamera.Update(_GT);

            CalcShotDirection();

            mArsenal.Update(_GT);

            CheckPickup(_frags);

            mModel.Position = mController.Position;

            UpdateLifeGlow();

            if (Position.Y < -100 && mIsAlive)
            {
                mIsAlive = false;
                mSoundInstances[(int)SoundEffects.DYING].Play();
                Suicide(this, EventArgs.Empty);
            }

            // Debug Move call if only one controller is available
            //if (mIndex == PlayerIndex.Two && Moved != null)
            //    Moved(null, new PositionEventArgs(this.Position, mModelShotDirection, new Vector2(mAzimut, mPolar), RelShotPosition));
        }

        private void TransformModel(Vector3 _rot)
        {
            Vector3 tRot = _rot;
            tRot.Y += 2.7f;

            mModel.Transform = Matrix.CreateFromYawPitchRoll(tRot.Y, 0, tRot.Z) * mModelTranslation;
            //mModel.Transform += mModelTranslation;
        }

        private void CalcModelDirection(float azimut, float polar)
        {
            mModelDirection = new Vector3((float)Math.Sin(mPolar) * mRadius,    // für Bewegung.
                                          0,                                    // Für Shot.
                                          (float)Math.Cos(mPolar) * mRadius);   // für Bewegung.

        }

        private void CalcShotDirection()
        {
            mModelShotDirection = Vector3.Normalize((mCamera as ChaseCamera).Target - (mCamera as ChaseCamera).Position) * 1.5f;
        }

        private void CheckPickup(List<Fragment> _frags)
        {
            foreach (Fragment _f in _frags)
            {
                if ((this.Position - _f.Position).Length() <= Data.P_PICKUP_RANGE && !_f.Picked)
                    mArsenal.Pickup(_f.CheckPickup());
            }
        }

        public void DebugControls()
        {
            if (mIndex == PlayerIndex.One)
            {
                // Spawn ontop
                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    this.Spawn(new Vector3(0, 85, 0));

                if (Keyboard.GetState().IsKeyDown(Keys.M) && mOldKeys.IsKeyUp(Keys.M))
                    swap = swap ? false : true;

                Vector3 tChange;
                float tIntChange;

                tChange = Vector3.Zero;
                tIntChange = mCamTargetAdjust;

                float tInc = .02f;
                float tInc2 = 1.001f;
                float tInc3 = .999f;

                #region Change
                // Rotate Cam
                if (Keyboard.GetState().IsKeyDown(Keys.U))
                    tChange.X += tInc;
                if (Keyboard.GetState().IsKeyDown(Keys.I))
                    tChange.Y += tInc;
                if (Keyboard.GetState().IsKeyDown(Keys.O))
                    tChange.Z += tInc;
                if (Keyboard.GetState().IsKeyDown(Keys.J))
                    tChange.X -= tInc;
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                    tChange.Y -= tInc;
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                    tChange.Z -= tInc;
                #endregion



                mCamTargetAdjust = tIntChange;
                //Shot.mBaseRot = tChange;
            }
        }

        /*
        private void UpdateCAM()
        {
            mCamera.Position = new Vector3(
               (float)(mRadius * Math.Cos(mPolar) * Math.Cos(mAzimut)),
               (float)(mRadius * Math.Cos(mPolar) * Math.Sin(mAzimut)),
               (float)(mRadius * Math.Sin(mPolar)))
             + mController.Position;

            Vector3 tDir = (mController.Position - mCamera.Position);
            //mCAM.Direction = new Vector3(tDir.X + .8f, tDir.Y + .9f, tDir.Z);
            mCamera.Direction = new Vector3(tDir.X, tDir.Y, tDir.Z);
            mCamera.Direction.Normalize();

            mOffsetShot = mCamera.Direction * Data.P_SHOTOFFSET;
        }
        */

        #endregion

        #region Subscribed Events

        #region Gamepad Controls
        #region Dpad
        public void OnPressDPadUp(object o, ControllerArgs e)
        {
            //TODO: Implement weapon change once more weapons are implemented
        }

        public void OnPressDPadLeft(object o, ControllerArgs e)
        {
            //TODO: Implement weapon change once more weapons are implemented
        }

        public void OnPressDPadRight(object o, ControllerArgs e)
        {
            //TODO: Implement weapon change once more weapons are implemented
        }

        public void OnPressDPadDown(object o, ControllerArgs e)
        {
            //TODO: Implement weapon change once more weapons are implemented
        }
        #endregion

        #region Left Thumb Stick
        public void OnMoveLeftStick(object _sender, ControllerArgs _e)
        {

            Vector2 tTotalMovement = Vector2.Zero;

            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive)
            {
                Vector3 tForward = Direction;
                tForward.Y = 0;
                tForward.Normalize();
                Vector3 tRight = -Vector3.Cross(Vector3.Up, Direction);
                tTotalMovement += ((Vector2)_e.ControllerState).Y * new Vector2(tForward.X, tForward.Z);
                tTotalMovement += ((Vector2)_e.ControllerState).X * new Vector2(tRight.X, tRight.Z);
                mController.Move(tTotalMovement);

                if (Moved != null)
                    Moved(null, new PositionEventArgs(this.Position, mModelShotDirection, new Vector2(mAzimut, mPolar), RelShotPosition, new Vector3(mAzimut, mPolar, 0)));
            }
        }
        #endregion

        #region Right Thumb Stick
        public void OnMoveRightStickUp(object _sender, ControllerArgs _e)
        {

            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive)
            {
                mAzimut -= -.01f * mCamInvertY;
            }
            if (mAzimut > 1.5f) mAzimut = 1.5f;
        }

        public void OnMoveRightStickLeft(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive)
            {
                mPolar -= -.02f * mCamInvertX;
            }
        }

        public void OnMoveRightStickRight(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive)
            {
                mPolar += -.02f * mCamInvertX;
            }
        }

        public void OnMoveRightStickDown(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive)
            {
                mAzimut += -.01f * mCamInvertY;
            }
            if (mAzimut < 0.25f) mAzimut = 0.25f;
        }
        #endregion

        #region Face Buttons
        public void OnPressAButton(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == mIndex && mIsAlive && mScreen.Visible)
            {
                if (mController.CharacterController.SupportFinder.HasSupport)
                    mSoundInstances[(int)SoundEffects.JUMPING].Play();
                mController.CharacterController.Jump();
            }

            if (!mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex && mScreen.Visible)
            {
                mSoundInstances[3].Stop();
                mSoundInstances[(int)SoundEffects.RESPAWNING].Play();
                mArsenal.InitializeAmmunition();
                Respawn(this, EventArgs.Empty);
                Spawn(mArena.Spawns.ElementAt(0).Position);
                this.Initialize();
            }
        }

        public void OnPressBButton(object _sender, ControllerArgs _e)
        {
            if (!mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex && mScreen.Visible)
            {
                mSoundInstances[3].Stop();
                mSoundInstances[(int)SoundEffects.RESPAWNING].Play();
                mArsenal.InitializeAmmunition();
                Respawn(this, EventArgs.Empty);
                Spawn(mArena.Spawns.ElementAt(1).Position);
                this.Initialize();
            }
        }

        public void OnPressXButton(object _sender, ControllerArgs _e)
        {
            // Boost-Function
            if (_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive) this.mArsenal.TriggerBoost();
            if (!_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive) this.mArsenal.ReleaseBoost();

            // Respawn point X
            if (!mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex && mScreen.Visible)
            {
                mSoundInstances[3].Stop();
                mSoundInstances[(int)SoundEffects.RESPAWNING].Play();
                mArsenal.InitializeAmmunition();
                Respawn(this, EventArgs.Empty);
                Spawn(mArena.Spawns.ElementAt(2).Position);
                this.Initialize();
            }
        }

        public void OnPressYButton(object _sender, ControllerArgs _e)
        {
            //TODO: Respawn point Y
            if (!mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex && mScreen.Visible)
            {
                mSoundInstances[3].Stop();
                mSoundInstances[(int)SoundEffects.RESPAWNING].Play();
                mArsenal.InitializeAmmunition();
                Respawn(this, EventArgs.Empty);
                Spawn(mArena.Spawns.ElementAt(3).Position);
                this.Initialize();
            }
        }
        #endregion

        #region Shoulder Buttons
        public void OnPressLeftButton(object _sender, ControllerArgs _e)
        {
            //TODO: Cycle weapons
            //      Rotate cloud
            if (mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex) mArsenal.CycleLeft();
        }

        public void OnPressRightButton(object _sender, ControllerArgs _e)
        {
            //TODO: Cycle weapons
            //      Rotate cloud
            if (mIsAlive && _e.PressedButton && _e.ControllerIndex == mIndex) mArsenal.CycleRight();
        }

        public void OnPressLeftTrigger(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive) this.mArsenal.TriggerTrap();
            if (!_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive) this.mArsenal.ReleaseTrap();
        }

        public void OnPressRightTrigger(object _sender, ControllerArgs _e)
        {
            if (_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive && mScreen.Visible)
            {
                if (this.mArsenal.GetAmmo() > 0) mSoundInstances[(int)SoundEffects.CHARGING].Play();
                this.mArsenal.TriggerShot();
            }


            if (!_e.PressedButton && _e.ControllerIndex == this.mIndex && mIsAlive)
            {
                if (this.mArsenal.GetAmmo() > 0 && mArsenal.CurProgramm != Data.ProgrammType.GOTO && mScreen.Visible)
                {
                    mSoundInstances[0].Stop();
                    SoundEffectInstance tInstance = mSounds[(int)SoundEffects.SHOOTING].CreateInstance();
                    tInstance.Volume = mSoundInstances[(int)SoundEffects.SHOOTING].Volume;
                    tInstance.Play();
                }
                this.mArsenal.ReleaseShot();
            }
        }
        #endregion

        #region Other
        public void OnPressStart(object o, ControllerArgs e)
        {

        }

        public void OnPressBack(object o, ControllerArgs e)
        {

        }
        #endregion

        #endregion

        #region Collisions

        private void OnContactCreated(
            object _sender,
            BEPUphysics.Collidables.Collidable _other,
            BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler _pair,
            BEPUphysics.CollisionTests.ContactData _contact)
        {
            if (!(_other is BEPUphysics.Collidables.StaticMesh)
                && _other.Tag != null && !(_other.Tag is string))
            {
                // Test for projectile
                if (_other.Tag != null && _other.Tag is CollisionTag && (_other.Tag as CollisionTag).Player != mIndex)
                {
                    CollisionTag tTag = (CollisionTag)_other.Tag;
                    int tRun = (int)tTag.RunType;

                    // 0 : Trap
                    // 1 : Shot


                    switch (tRun)
                    {
                        case 0:
                            HandleTrapCollision(tTag.ProgramType);
                            break;
                        case 1:
                            HandleShotCollision(tTag.ProgramType, tTag.ScaleFactor, Vector3.Normalize(this.Position - _pair.Contacts.First().Contact.Position));
                            break;
                    }
                }
                else
                {

                    // Test for other player
                    if (_other is BEPUphysics.Collidables.MobileCollidables.ConvexCollidable &&
                        (_other as BEPUphysics.Collidables.MobileCollidables.ConvexCollidable).Entity.Tag != null)
                    {
                        if (((_other as BEPUphysics.Collidables.MobileCollidables.ConvexCollidable).Entity.Tag
                            as CollisionTag).RunType == Data.RunType.BOOST)
                            HandleBoostCollision(Data.ProgrammType.DELETE);
                    }
                }
            }
        }

        private void HandleTrapCollision(Data.ProgrammType _programType)
        {
            if (_programType == Data.ProgrammType.DELETE)
                this.InflictDamage(Data.DEL_TRAPDAMAGE);
            if (_programType == Data.ProgrammType.OPTIMIZE)
                mArsenal.TriggerNegativeBoost();
            if (_programType == Data.ProgrammType.GOTO)
                this.InflictDamage(Data.OPT_TRAPDAMAGE);
        }

        private void HandleShotCollision(Data.ProgrammType _programType, float _scaling, Vector3 _dir)
        {
            if (_programType == Data.ProgrammType.DELETE)
            {
                mSoundInstances[(int)SoundEffects.HITTING].Play();
                this.InflictDamage(20 * _scaling * Data.DEL_SHOTBASEMASS);
            }
            if (_programType == Data.ProgrammType.OPTIMIZE)
                mArsenal.TriggerNegativeBoost();
            if (_programType == Data.ProgrammType.GOTO)
            {
                mSoundInstances[(int)SoundEffects.HITTING].Play();
                this.InflictDamage(10);
            }
            if (_programType == Data.ProgrammType.FIREWALL)
            {
                _dir.Y = 0;
                mSoundInstances[(int)SoundEffects.HITTING].Play();
                if (!mFirewallInvuln)
                    this.mController.CharacterController.Body.ApplyImpulse(Vector3.Zero, _dir * 150 + new Vector3(0, 50, 0));

                Vibrate();
            }
        }

        private void HandleBoostCollision(Data.ProgrammType _programType)
        {
            if (_programType == Data.ProgrammType.DELETE)
                this.InflictDamage(Data.DEL_BOOSTDAMAGE);
        }
        #endregion

        #region Set and Deset Boosts
        private void OnSetDeleteCollision(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 += Data.PG_DELETE;
            mSoundInstances[(int)SoundEffects.BOOSTING].Play();
            (mController.CharacterController.Body.Tag as CollisionTag).MakeDeleteBoost();
        }

        private void OnDeSetDeleteCollision(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 -= Data.PG_DELETE;
            (mController.CharacterController.Body.Tag as CollisionTag).MakePlayer();
        }

        private void OnSetInvulnBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 += Data.PG_FIREWALL;
            mFirewallInvuln = true;
        }

        private void OnDeSetInvulnBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 -= Data.PG_FIREWALL;
            mFirewallInvuln = false;
        }

        private void OnSetSpeedBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 += Data.PG_OPTIMIZE;
            mSoundInstances[(int)SoundEffects.BOOSTING].Play();
            mController.CharacterController.HorizontalMotionConstraint.Speed *= Data.OPT_BOOSTFACTOR;
        }

        private void OnDeSetSpeedBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 -= Data.PG_OPTIMIZE;
            mController.CharacterController.HorizontalMotionConstraint.Speed /= Data.OPT_BOOSTFACTOR;
        }

        private void OnSetSpeedDeBuff(object _sender, EventArgs _e)
        {
            mController.CharacterController.HorizontalMotionConstraint.Speed *= Data.OPT_NEGBOOSTFACTOR;
        }

        private void OnDeSetSpeedDeBuff(object _sender, EventArgs _e)
        {
            mController.CharacterController.HorizontalMotionConstraint.Speed /= Data.OPT_NEGBOOSTFACTOR;
        }

        private void OnSetJumpBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 += Data.PG_GOTO;
            mSoundInstances[(int)SoundEffects.BOOSTING].Play();
            mController.CharacterController.JumpSpeed *= Data.GOTO_BOOSTFACTOR;
        }

        private void OnDeSetJumpBuff(object _sender, EventArgs _e)
        {
            mModel.mGlowColor2 -= Data.PG_GOTO;
            mController.CharacterController.JumpSpeed /= Data.GOTO_BOOSTFACTOR;
        }
        #endregion

        private void OnVibrationBang(object _sender, EventArgs _e)
        {
            GamePad.SetVibration(mIndex, 0, 0);
        }

        #endregion

    }
}
