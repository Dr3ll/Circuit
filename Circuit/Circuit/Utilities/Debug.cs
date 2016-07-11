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
    public class Debug : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static float Line1;
        public static float Line2;
        public static float Line3;
        public static float Line4;

        private static Color FontColor = Color.White;

        private SpriteBatch SB;
        private SpriteFont mFont;
        private string mFontDirectory;

        public Debug(Game GAME)
            : base(GAME)
        {
            GAME.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(OnComponentAdded);

            this.LoadContent();
        }

        public Debug(Game GAME, string _fontDirectory)
            : base(GAME)
        {
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

            if (mFontDirectory != null)
                mFont = Game.Content.Load<SpriteFont>(mFontDirectory + "/fps_font");
            else
                mFont = Game.Content.Load<SpriteFont>("fps_font");
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime GT)
        {

        }

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            SB.DrawString(mFont, Line1.ToString("00.00"), new Vector2(10, 70), FontColor);
            SB.DrawString(mFont, Line2.ToString("00.00"), new Vector2(10, 90), FontColor);
            SB.DrawString(mFont, Line3.ToString("00.00"), new Vector2(10, 110), FontColor);
            SB.DrawString(mFont, Line4.ToString("00.00"), new Vector2(10, 130), FontColor);
            SB.End();
        }

        private void OnComponentAdded(object sender, EventArgs e)
        {
            if (!this.Game.Components[this.Game.Components.Count() - 2].Equals(this))
            {
                this.Game.Components.Remove(this);
                this.Game.Components.Add(this);
            }
        }
    }
}
