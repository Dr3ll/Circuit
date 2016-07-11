using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Circuit.Utilities
{

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ControlClass : Microsoft.Xna.Framework.GameComponent
    {

        private bool[] mConnectedControllers;
        private GamePadState mControllerOne;
        private GamePadState mControllerTwo;

        private GamePadState mOldStateOne;
        private GamePadState mOldStateTwo;

        private int mLastTotalMS;

        public delegate void ControllerEventHandler(ControlClass _CC, ControllerArgs e);
        #region Events
        #region D-Pad
        public event ControllerEventHandler ePressDPadUp;
        public event ControllerEventHandler ePressDPadLeft;
        public event ControllerEventHandler ePressDPadRight;
        public event ControllerEventHandler ePressDPadDown;
        #endregion
        #region Face Buttons
        public event ControllerEventHandler ePressAButton;
        public event ControllerEventHandler ePressBButton;
        public event ControllerEventHandler ePressXButton;
        public event ControllerEventHandler ePressYButton;
        #endregion
        #region Shoulder Buttons
        public event ControllerEventHandler ePressLeftTrigger;
        public event ControllerEventHandler ePressLeftButton;
        public event ControllerEventHandler ePressRightTrigger;
        public event ControllerEventHandler ePressRightButton;
        #endregion
        #region Thumbsticks
        public event ControllerEventHandler eMoveLeftStick;
        public event ControllerEventHandler eMoveLeftStickUp;
        public event ControllerEventHandler eMoveLeftStickLeft;
        public event ControllerEventHandler eMoveLeftStickRight;
        public event ControllerEventHandler eMoveLeftStickDown;
        public event ControllerEventHandler eMoveRightStick;
        public event ControllerEventHandler eMoveRightStickUp;
        public event ControllerEventHandler eMoveRightStickLeft;
        public event ControllerEventHandler eMoveRightStickRight;
        public event ControllerEventHandler eMoveRightStickDown;
        #endregion
        #region Other
        public event ControllerEventHandler ePressStart;
        public event ControllerEventHandler ePressBack;
        #endregion
        #endregion


        public ControlClass(Game GAME)
            : base(GAME)
        {
            mConnectedControllers = new bool[] { false, false };
            mControllerOne = GamePad.GetState(PlayerIndex.One);
            mControllerTwo = GamePad.GetState(PlayerIndex.Two);

        }

        public override void Initialize()
        {
            if (mControllerOne.IsConnected) mConnectedControllers[0] = true;

            if (mControllerTwo.IsConnected) mConnectedControllers[1] = true;

            base.Initialize();
        }

        public void CheckConnection()
        {
            if (mControllerOne.IsConnected) mConnectedControllers[0] = true;
            else mConnectedControllers[0] = false;

            if (mControllerTwo.IsConnected) mConnectedControllers[1] = true;
            else mConnectedControllers[1] = false;
        }

        private void ReadControllerOne(int _ms)
        {
            #region D-Pad
            if (mControllerOne.IsButtonDown(Buttons.DPadUp) && ePressDPadUp != null) ePressDPadUp(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.DPadUp) && ePressDPadUp != null) ePressDPadUp(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.DPadLeft) && ePressDPadLeft != null) ePressDPadLeft(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.DPadLeft) && ePressDPadLeft != null) ePressDPadLeft(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.DPadRight) && ePressDPadRight != null) ePressDPadRight(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.DPadRight) && ePressDPadRight != null) ePressDPadRight(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.DPadDown) && ePressDPadDown != null) ePressDPadDown(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.DPadDown) && ePressDPadDown != null) ePressDPadDown(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            #endregion

            #region Face Buttons
            if (mControllerOne.IsButtonDown(Buttons.A) && mOldStateOne.IsButtonUp(Buttons.A) && ePressAButton != null) ePressAButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.A) && ePressAButton != null) ePressAButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.B) && mOldStateOne.IsButtonUp(Buttons.B) && ePressBButton != null) ePressBButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.B) && ePressBButton != null) ePressBButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.X) && mOldStateOne.IsButtonUp(Buttons.X) && ePressXButton != null) ePressXButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.X) && ePressXButton != null) ePressXButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.Y) && mOldStateOne.IsButtonUp(Buttons.Y) && ePressYButton != null) ePressYButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.Y) && ePressYButton != null) ePressYButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            #endregion

            #region Shoulder Buttons
            if (mControllerOne.IsButtonDown(Buttons.LeftTrigger) && ePressLeftTrigger != null) ePressLeftTrigger(this, new ControllerArgs(PlayerIndex.One, mControllerOne.Triggers.Left, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.LeftTrigger) && ePressLeftTrigger != null) ePressLeftTrigger(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.LeftShoulder)
                && ePressLeftButton != null
                && mOldStateOne.IsButtonUp(Buttons.LeftShoulder)) ePressLeftButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.LeftShoulder) && ePressLeftButton != null) ePressLeftButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.RightTrigger) && ePressRightTrigger != null) ePressRightTrigger(this, new ControllerArgs(PlayerIndex.One, mControllerOne.Triggers.Right, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightTrigger) && ePressRightTrigger != null) ePressRightTrigger(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.RightShoulder)
                && mOldStateOne.IsButtonUp(Buttons.RightShoulder)
                && ePressRightButton != null) ePressRightButton(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightShoulder) && ePressRightButton != null) ePressRightButton(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            #endregion

            float f = mControllerOne.ThumbSticks.Left.LengthSquared();

            #region Left Stick
            if (eMoveLeftStick != null) eMoveLeftStick(this, new ControllerArgs(PlayerIndex.One, mControllerOne.ThumbSticks.Left, true, _ms));
            #endregion

            #region Right Stick
            if (eMoveRightStick != null) eMoveRightStick(this, new ControllerArgs(PlayerIndex.One, mControllerOne.ThumbSticks.Right, true, _ms));

            if (mControllerOne.IsButtonDown(Buttons.RightThumbstickUp) && eMoveRightStickUp != null) eMoveRightStickUp(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightThumbstickUp) && eMoveRightStickUp != null) eMoveRightStickUp(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.RightThumbstickLeft) && eMoveRightStickLeft != null) eMoveRightStickLeft(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightThumbstickLeft) && eMoveRightStickLeft != null) eMoveRightStickLeft(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.RightThumbstickRight) && eMoveRightStickRight != null) eMoveRightStickRight(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightThumbstickRight) && eMoveRightStickRight != null) eMoveRightStickRight(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.RightThumbstickDown) && eMoveRightStickDown != null) eMoveRightStickDown(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.RightThumbstickDown) && eMoveRightStickDown != null) eMoveRightStickDown(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            #endregion

            #region Other
            if (mControllerOne.IsButtonDown(Buttons.Start) && ePressStart != null) ePressStart(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.Start) && ePressStart != null) ePressStart(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            
            if (mControllerOne.IsButtonDown(Buttons.Back) && ePressBack != null) ePressBack(this, new ControllerArgs(PlayerIndex.One, true, _ms));
            else if (mOldStateOne.IsButtonDown(Buttons.Back) && ePressBack != null) ePressBack(this, new ControllerArgs(PlayerIndex.One, false, _ms));
            #endregion
            mOldStateOne = mControllerOne;

        }

        private void ReadControllerTwo(int _ms)
        {
            #region D-Pad
            if (mControllerTwo.IsButtonDown(Buttons.DPadUp) && ePressDPadUp != null) ePressDPadUp(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.DPadUp) && ePressDPadUp != null) ePressDPadUp(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.DPadLeft) && ePressDPadLeft != null) ePressDPadLeft(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.DPadLeft) && ePressDPadLeft != null) ePressDPadLeft(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.DPadRight) && ePressDPadRight != null) ePressDPadRight(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.DPadRight) && ePressDPadRight != null) ePressDPadRight(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.DPadDown) && ePressDPadDown != null) ePressDPadDown(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.DPadDown) && ePressDPadDown != null) ePressDPadDown(this, new ControllerArgs(PlayerIndex.Two, false, _ms));
            #endregion

            #region Face Buttons
            if (mControllerTwo.IsButtonDown(Buttons.A) && mOldStateTwo.IsButtonUp(Buttons.A) && ePressAButton != null) ePressAButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.A) && ePressAButton != null) ePressAButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.B) && mOldStateTwo.IsButtonUp(Buttons.B) && ePressBButton != null) ePressBButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.B) && ePressBButton != null) ePressBButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.X) && mOldStateTwo.IsButtonUp(Buttons.X) && ePressXButton != null) ePressXButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.X) && ePressXButton != null) ePressXButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.Y) && mOldStateTwo.IsButtonUp(Buttons.Y) && ePressYButton != null) ePressYButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.Y) && ePressYButton != null) ePressYButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));
            #endregion

            #region Shoulder Buttons
            if (mControllerTwo.IsButtonDown(Buttons.LeftTrigger) && ePressLeftTrigger != null) ePressLeftTrigger(this, new ControllerArgs(PlayerIndex.Two, mControllerTwo.Triggers.Left, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.LeftTrigger) && ePressLeftTrigger != null) ePressLeftTrigger(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.LeftShoulder)
                && ePressLeftButton != null
                && mOldStateTwo.IsButtonUp(Buttons.LeftShoulder)) ePressLeftButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.LeftShoulder) && ePressLeftButton != null) ePressLeftButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightTrigger) && ePressRightTrigger != null) ePressRightTrigger(this, new ControllerArgs(PlayerIndex.Two, mControllerTwo.Triggers.Right, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightTrigger) && ePressRightTrigger != null) ePressRightTrigger(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightShoulder)
                && mOldStateTwo.IsButtonUp(Buttons.RightShoulder)
                && ePressRightButton != null) ePressRightButton(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightShoulder) && ePressRightButton != null) ePressRightButton(this, new ControllerArgs(PlayerIndex.Two, false, _ms));
            #endregion

            float f = mControllerTwo.ThumbSticks.Left.LengthSquared();

            #region Left Stick
            if (eMoveLeftStick != null) eMoveLeftStick(this, new ControllerArgs(PlayerIndex.Two, mControllerTwo.ThumbSticks.Left, true, _ms));
            #endregion

            #region Right Stick
            if (eMoveRightStick != null) eMoveRightStick(this, new ControllerArgs(PlayerIndex.Two, mControllerOne.ThumbSticks.Right, true, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightThumbstickUp) && eMoveRightStickUp != null) eMoveRightStickUp(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightThumbstickUp) && eMoveRightStickUp != null) eMoveRightStickUp(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightThumbstickLeft) && eMoveRightStickLeft != null) eMoveRightStickLeft(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightThumbstickLeft) && eMoveRightStickLeft != null) eMoveRightStickLeft(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightThumbstickRight) && eMoveRightStickRight != null) eMoveRightStickRight(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightThumbstickRight) && eMoveRightStickRight != null) eMoveRightStickRight(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.RightThumbstickDown) && eMoveRightStickDown != null) eMoveRightStickDown(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.RightThumbstickDown) && eMoveRightStickDown != null) eMoveRightStickDown(this, new ControllerArgs(PlayerIndex.Two, false, _ms));
            #endregion

            #region Other
            if (mControllerTwo.IsButtonDown(Buttons.Start) && ePressStart != null) ePressStart(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.Start) && ePressStart != null) ePressStart(this, new ControllerArgs(PlayerIndex.Two, false, _ms));

            if (mControllerTwo.IsButtonDown(Buttons.Back) && ePressBack != null) ePressBack(this, new ControllerArgs(PlayerIndex.Two, true, _ms));
            else if (mOldStateTwo.IsButtonDown(Buttons.Back) && ePressBack != null) ePressBack(this, new ControllerArgs(PlayerIndex.Two, false, _ms));
            #endregion
            mOldStateTwo = mControllerTwo;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            int tUpdateDuration = (int)gameTime.ElapsedGameTime.TotalMilliseconds - mLastTotalMS;

            mControllerOne = GamePad.GetState(PlayerIndex.One);
            mControllerTwo = GamePad.GetState(PlayerIndex.Two);


            CheckConnection();

            if (mControllerOne.IsConnected) ReadControllerOne(tUpdateDuration);

            if (mControllerTwo.IsConnected) ReadControllerTwo(tUpdateDuration);

            base.Update(gameTime);

            mLastTotalMS = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

    }
}
