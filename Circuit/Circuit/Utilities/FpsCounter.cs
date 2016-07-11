using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Circuit.Utilities
{
    /// <summary>
    /// This draws an fps-counter in the upper left corner of the screen
    /// </summary>
    public class FpsCounter : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private static Color FontColor = Color.White;

        private float mElapsedFrame;
        private float mFrameRate;
        private float mFrames;
        private float mElapsedUpdate;
        private float mUpdateRate;
        private float mUpdates;
        private SpriteBatch SB;
        private SpriteFont mFont;
        private string mFontDirectory;

        public FpsCounter(Game GAME)
            : base(GAME)
        {
            mElapsedFrame = 0.0f;
            mFrameRate = 0.0f;
            mFrames = 0.0f;

            mElapsedUpdate = 0.0f;
            mUpdateRate = 0.0f;
            mUpdates = 0.0f;

            GAME.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(OnComponentAdded);

            this.LoadContent();
        }

        public FpsCounter(Game GAME, string _fontDirectory)
            : base(GAME)
        {
            mElapsedFrame = 0.0f;
            mFrameRate = 0.0f;
            mFrames = 0.0f;

            mElapsedUpdate = 0.0f;
            mUpdateRate = 0.0f;
            mUpdates = 0.0f;

            mFontDirectory = _fontDirectory;

            GAME.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(OnComponentAdded);

            this.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SB = new SpriteBatch(Game.GraphicsDevice);
            
            if(mFontDirectory != null)
                mFont = Game.Content.Load<SpriteFont>(mFontDirectory + "/fps_font");
            else
                mFont = Game.Content.Load<SpriteFont>("fps_font");
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime GT)
        {
            mElapsedUpdate += (float) GT.ElapsedGameTime.TotalSeconds;

            if (mElapsedUpdate > 1.0f)
            {
                mElapsedUpdate -= 1.0f;
                mUpdateRate = mUpdates;
                mUpdates = 0;
            }
            else
            {
                mUpdates += 1;
            }
        }

        public override void Draw(GameTime GT)
        {
            mElapsedFrame += (float) GT.ElapsedGameTime.TotalSeconds;

            if (mElapsedFrame > 1.0f)
            {
                mElapsedFrame -= 1.0f;
                mFrameRate = mFrames;
                mFrames = 0;
            }
            else
            {
                mFrames += 1;
            }

            SB.Begin();
            SB.DrawString(mFont, mFrameRate.ToString("0.00"), new Vector2(10, 10), FontColor);
            SB.DrawString(mFont, mUpdateRate.ToString("0.00"), new Vector2(10, 32), FontColor);
            SB.End();
        }

        private void OnComponentAdded(object sender, EventArgs e)
        {
            if (!this.Game.Components[this.Game.Components.Count()-1].Equals(this))
            {
                this.Game.Components.Remove(this);
                this.Game.Components.Add(this);
            }
        }
    }
}
