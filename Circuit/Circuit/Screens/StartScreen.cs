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
using Circuit.ArenaClasses;
using Circuit.Utilities.Cameras;

namespace Circuit.Screens
{
    class StartScreen : GameScreen, Circuit.ArenaClasses.ArenaBuilders.BuildDirector
    {
        SpriteBatch SB;
        MenuComponent mMenu;
        Arena mArena;
        Camera CAM;
        Vector2 mCamRotation = Vector2.Zero;
        Texture2D mStartScreen;
        #region Events

        // StartGame Event wenn im Menü Start Game ausgesucht wurde
        public delegate void StartGameHandler(StartScreen _startScreen, EventArgs e);
        public event StartGameHandler StartGame;

        // EnterCredits Event wenn im Menü Credits ausgesucht wurde
        public delegate void EnterCreditsHandler(StartScreen _startScreen, EventArgs e);
        public event EnterCreditsHandler EnterCredits;

        // ExitCredits Event wenn im CreditsMenu Escp gedrückt wurde
        public delegate void ExitCreditsHandler(CreditsScreen _creditsScreen, EventArgs e);
        public event ExitCreditsHandler ExitCredits;

        // EnterSettings Event wenn im Menü Settings ausgesucht wurde
        public delegate void EnterSettingsHandler(StartScreen _startScreen, EventArgs e);
        public event EnterSettingsHandler EnterSettings;

        public delegate void EnterInstructionsHandler(StartScreen _startScreen, EventArgs e);
        public event EnterInstructionsHandler EnterInstructions;

        #endregion

        public StartScreen(Game GAME)
            : base(GAME)
        {
            


        }

        public override void Initialize()
        {
            base.Initialize();
            if (this.Visible)
            {
                // Create an Arena and a Camera
                ArenaClasses.ArenaBuilders.ArenaBuilder tBuilder = new ArenaClasses.ArenaBuilders.NeutralArenaBuilder(Game, this);
                //mArena = (Create(tBuilder));

                CAM = new Circuit.Utilities.Cameras.FreeCamera(Vector3.Zero, .5f, .5f, (float)Data.R_VIEWPORTWIDTH / (float)Data.R_VIEWPORTHEIGHT, this.GraphicsDevice);
                CamHandler = new StandartCamHandler(CAM, this.GraphicsDevice);

                SB = new SpriteBatch(GraphicsDevice);

                mStartScreen = Game.Content.Load<Texture2D>("TitleScreen");

                // Create menu
                string[] tMenuItems = { "Start Game", "Instructions", "Options", "Credits", "End Game" };
                mMenu = new MenuComponent(Game,
                                          SB,
                                          Game.Content.Load<SpriteFont>("Fonts/startMenuFont"),
                                          tMenuItems);
                this.Add(mMenu);
                mMenu.EnterMenuItem += new MenuComponent.EnterMenuItemHandler(OnEnterMenuItem);
            }

            if (!mLoaded)
                LoadContent(Game.Content);
        }

        private void LoadContent(ContentManager CM)
        {
        }

        public Arena Create(Circuit.ArenaClasses.ArenaBuilders.ArenaBuilder _builder)
        {
            _builder.SetName();
            _builder.BuildSpace();
            _builder.BuildSky();
            _builder.BuildBase();
            _builder.BuildFragments();
            _builder.BuildDestructibles();
            _builder.BuildSpawnPoints();

            return _builder.GetArena();
        }

        public override void Update(GameTime _GT)
        {
            if (this.Visible && this.Enabled)
            {
                UpdateRotation();
                ((FreeCamera)CAM).Rotate(mCamRotation.X, mCamRotation.Y);
                CAM.Update(_GT);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Game.Exit();

                mMenu.Update(_GT);

                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                GamePad.SetVibration(PlayerIndex.Two, 0, 0);
            }
            base.Update(_GT);
        }

        private void UpdateRotation()
        {

            mCamRotation.X = -Mouse.GetState().X * 0.02f;
            mCamRotation.Y = -Mouse.GetState().Y * 0.02f;

            Mouse.SetPosition(0, 0);
        } 

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            SB.Draw(mStartScreen, new Rectangle(0,0,Data.R_VIEWPORTWIDTH,Data.R_VIEWPORTHEIGHT),Color.White);
            SB.End();
            //base.Draw(GT);
            mMenu.Draw(GT);
        }

        public override void Hide()
        {
            base.Hide();

            mArena = null;
        }

        #region Subscribed Events

        private void OnEnterMenuItem(MenuComponent _MenuComponent, EventArgs e)
        {
            // If Start Game is selected, StartGame gets fired       
            if (mMenu.SelectedIndex == 0)
            {
                if (StartGame != null)
                {
                    StartGame(this, new EventArgs());
                }
            }

            // If Instructions is selected, EnterInstructions gets fired         
            if (mMenu.SelectedIndex == 1)
            {
                if (EnterInstructions != null)
                {
                    EnterInstructions(this, new EventArgs());
                }
            }

            if (mMenu.SelectedIndex == 2)
            {
                if (EnterSettings != null)
                {
                    EnterSettings(this, new EventArgs());
                }
            }

            // If Credits is selected, EnterCredits gets fired         
            if (mMenu.SelectedIndex == 3)
            {
                if (EnterCredits != null)
                {
                    EnterCredits(this, new EventArgs());
                }
            }

            // If  Exit Game is selected the game ends
            if (mMenu.SelectedIndex == 4)
            {
                if (StartGame != null)
                {
                    Game.Exit();
                }
            }
        }

        #endregion

    }
}
