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
    class CreditsScreen : GameScreen
    {
        #region Events

        // ExitCredits Event wenn im CreditsMenu Escp gedrückt wurde
        public delegate void ExitCreditsHandler(CreditsScreen _creditsScreen, EventArgs e);
        public event ExitCreditsHandler ExitCredits;

        #endregion

        SpriteBatch SB;
        Texture2D mPageTexture;

        GamePadState mState;
        GamePadState mOldState;

        KeyboardState mKeyState;
        KeyboardState mOldKey;

        public CreditsScreen(Game GAME)
            : base(GAME)
        {
            SB = new SpriteBatch(GAME.GraphicsDevice);

            LoadContent(GAME.Content);
        }

        private void LoadContent(ContentManager CM)
        {
            mPageTexture = CM.Load<Texture2D>("CreditsScreen");
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
                { ExitCredits(this, EventArgs.Empty); }

                mOldState = mState;
                mOldKey = mKeyState;
                //base.Update(gameTime);
            }
        }

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            SB.Draw(mPageTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            SB.End();
            //base.Draw(GT);
        }


    }
}
