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

namespace Circuit.Screens
{
    class IntroScreen : GameScreen
    {
        Video mVideo;
        VideoPlayer mVidPlayer;
        Texture2D mVidTexture;
        SpriteBatch SB;
        KeyboardState mOldKeys;
        GamePadState mOldGamePad;

        Rectangle mScreen;

        #region Events

        public delegate void StartMenuHandler(IntroScreen _IntroScreen, EventArgs e);
        public event StartMenuHandler StartMenu;

        #endregion

        public IntroScreen(Game GAME)
            : base(GAME)
        {
            mVideo = GAME.Content.Load<Video>("Videos/Logo_720p");
            mVidPlayer = new VideoPlayer();
            mVidPlayer.IsLooped = true;

            int width = GAME.Window.ClientBounds.Width;
            int height = width * 9 / 16;
            mScreen = new Rectangle(0, (GAME.Window.ClientBounds.Height - height) / 2, width, height);

            mOldKeys = Keyboard.GetState();
            mOldGamePad = GamePad.GetState(PlayerIndex.One);
        }

        public override void Update(GameTime GT)
        {
            Mouse.SetPosition(512, 384);


            if (mVidPlayer.PlayPosition.TotalMilliseconds >= mVideo.Duration.TotalMilliseconds - 500 ||
                (mOldKeys.IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Escape)) ||
                (mOldGamePad.IsConnected && (mOldGamePad.Buttons.Back == ButtonState.Pressed && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released)) ||
                (mOldGamePad.IsConnected && (mOldGamePad.Buttons.Start == ButtonState.Pressed && GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Released)) ||
                Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                mVidPlayer.Stop();
                StartMenu(this, new EventArgs());
            }
            if (mVidPlayer.State == MediaState.Stopped && mVidPlayer.IsLooped)
            {
                mVidPlayer.IsLooped = false;
                mVidPlayer.Play(mVideo);
            }

            SB = new SpriteBatch(base.Game.GraphicsDevice);

            base.Update(GT);

            mOldKeys = Keyboard.GetState();
            mOldGamePad = GamePad.GetState(PlayerIndex.One);
        }

        public override void Draw(GameTime GT)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(242, 242, 242, 255));

            // Only call GetTexture if a video is playing or paused
            if (mVidPlayer.State != MediaState.Stopped)
                mVidTexture = mVidPlayer.GetTexture();

            // Draw the video, if we have a texture to draw.
            if (mVidTexture != null)
            {
                SB.Begin();
                SB.Draw(mVidTexture, mScreen, Color.White);
                SB.End();
            }

            base.Draw(GT);
        }

    }
}
