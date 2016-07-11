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
using Circuit.Utilities;
using Circuit;

namespace Circuit.Screens
{
    class InstructionsScreen : GameScreen
    {
        #region Events

        // ExitCredits Event wenn im CreditsMenu Escp gedrückt wurde
        public delegate void ExitInstructionsHandler(InstructionsScreen _creditsScreen, EventArgs e);
        public event ExitInstructionsHandler ExitInstructions;

        #endregion

        int mPageIndex;
        const int NUMBER_OF_PAGES=8;

        SpriteBatch SB;
        Texture2D mPageTexture;
        Texture2D[] mPageArray;

        GamePadState mState;
        GamePadState mOldState;

        KeyboardState mKeyState;
        KeyboardState mOldKey;

        public InstructionsScreen(Game GAME)
            : base(GAME)
        {
            SB = new SpriteBatch(GAME.GraphicsDevice);
            mPageIndex = 0;
            mPageArray = new Texture2D[NUMBER_OF_PAGES];

            LoadContent(GAME.Content);
        }

        private void LoadContent(ContentManager CM)
        {
            mPageArray[0] = CM.Load<Texture2D>("InstructionsMenu/Instructions0");
            mPageArray[1] = CM.Load<Texture2D>("InstructionsMenu/Instructions1");
            mPageArray[2] = CM.Load<Texture2D>("InstructionsMenu/Instructions2");
            mPageArray[3] = CM.Load<Texture2D>("InstructionsMenu/Instructions3");
            mPageArray[4] = CM.Load<Texture2D>("InstructionsMenu/Instructions4");
            mPageArray[5] = CM.Load<Texture2D>("InstructionsMenu/Instructions5");
            mPageArray[6] = CM.Load<Texture2D>("InstructionsMenu/Instructions6");
            mPageArray[7] = CM.Load<Texture2D>("InstructionsMenu/Instructions7");
            mPageTexture = mPageArray[mPageIndex];
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Visible && this.Enabled)
            {
                mState = GamePad.GetState(PlayerIndex.One);
                mKeyState = Keyboard.GetState();

                if (Keyboard.GetState().IsKeyUp(Keys.Escape) && mOldKey.IsKeyDown(Keys.Escape) || 
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back) || 
                    GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.Back))
                { mPageIndex = 0; ExitInstructions(this, EventArgs.Empty); }

                if(mState.IsButtonDown(Buttons.LeftThumbstickRight) && mOldState.IsButtonUp(Buttons.LeftThumbstickRight)) mPageIndex++;
                if (mState.IsButtonDown(Buttons.LeftThumbstickLeft) && mOldState.IsButtonUp(Buttons.LeftThumbstickLeft)) mPageIndex--;

                if (mKeyState.IsKeyDown(Keys.Right) && mOldKey.IsKeyUp(Keys.Right)) mPageIndex++;
                if (mKeyState.IsKeyDown(Keys.Left) && mOldKey.IsKeyUp(Keys.Left)) mPageIndex--;

                if (mPageIndex < 0) mPageIndex = 0;
                if (mPageIndex == NUMBER_OF_PAGES) mPageIndex--;

                mPageTexture = mPageArray[mPageIndex];

                mOldState = mState;
                mOldKey = mKeyState;
                //base.Update(gameTime);
            }
        }

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            SB.Draw(mPageTexture, new Rectangle(0,0,GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height), Color.White);
            SB.End();
            //base.Draw(GT);
        }


    }
}
