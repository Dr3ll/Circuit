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
using Circuit;
using Circuit.Utilities;
using Circuit.ArenaClasses;
using Circuit.S_Action;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using Circuit.Utilities.Cameras;
using Circuit.Utilities.Materials;
using Circuit.S_Action.PlayerClasses;

namespace Circuit.Screens
{
    class ActionScreen : GameScreen, ArenaClasses.ArenaBuilders.BuildDirector
    {
        Arena mArena;
        PointManager mManager;
        IngamePlayerScreen mCrosses;
        IngamePlayerScreen mDeathScreen;
        IngamePlayerScreen mResultScreen;
        IngamePlayerScreen mScoreScreen;
        KeyboardState mOldKey;
        GamePadState mOldState;
        GamePadState mOldState2;
        bool mEndGame;
        float rotationLight = 0.75f;
        SoundEffect[] tSoundArray;

        double mCurrentTimer;
        int mTempPointLimit;
        string mMinutes;
        string mSeconds;

        #region Events

        //Events
        // ExitAction Event when the ActionScreen is left (via menu or due to the end of the partie)
        public delegate void ExitActionHandler(ActionScreen _actionScreen, EventArgs e);
        public event ExitActionHandler ExitAction;



        #endregion

        public ActionScreen(Game GAME)
            : base(GAME)
        {
            //base.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            if (this.Visible)
            {
                ArenaClasses.ArenaBuilders.ArenaBuilder tBuilder = new ArenaClasses.ArenaBuilders.NeutralArenaBuilder(Game, this);
                mArena = (Create(tBuilder));
                mEndGame = false;

                mManager = new PointManager();

                ControlClass tControlclass = null;

                foreach (GameComponent GC in this.Game.Components)
                {
                    if (GC is ControlClass) tControlclass = (ControlClass)GC;
                }

                foreach (GameComponent GC in this.Game.Components)
                {
                    if (GC is IngamePlayerScreen && (GC as IngamePlayerScreen).Name == "Crosshair") mCrosses = (IngamePlayerScreen)GC;
                }
                mCrosses.Show(PlayerIndex.One);
                mCrosses.Show(PlayerIndex.Two);

                foreach (GameComponent GC in this.Game.Components)
                {
                    if (GC is IngamePlayerScreen && (GC as IngamePlayerScreen).Name==Data.PS_DEATHNAME) mDeathScreen= (IngamePlayerScreen)GC;
                }

                foreach (GameComponent GC in this.Game.Components)
                {
                    if (GC is IngamePlayerScreen && (GC as IngamePlayerScreen).Name == Data.PS_SCORENAME) mScoreScreen = (IngamePlayerScreen)GC;
                }
                mScoreScreen.Show(PlayerIndex.One);
                mScoreScreen.Show(PlayerIndex.Two);

                foreach (GameComponent GC in this.Game.Components)
                {
                    if (GC is IngamePlayerScreen && (GC as IngamePlayerScreen).Name == Data.PS_GAMEENDNAME) mResultScreen= (IngamePlayerScreen)GC;
                }

                if (Data.USE_TIMELIMIT) mCurrentTimer = Data.TIMELIMIT * 60;
                mTempPointLimit = Data.POINTS_TO_WIN;
                NormalMapMat.LightPosition = new Vector3(300, 100, 0);

                tSoundArray = new SoundEffect[Data.S_NUMBER_OF_SOUNDS_IN_SOUND_ARRAY];
                tSoundArray[0] = Game.Content.Load<SoundEffect>("Soundeffects/Charge");
                tSoundArray[1]=Game.Content.Load<SoundEffect>("Soundeffects/Shot");
                tSoundArray[2]=Game.Content.Load<SoundEffect>("Soundeffects/Jump");
                tSoundArray[3]=Game.Content.Load<SoundEffect>("Soundeffects/Die");
                tSoundArray[4] = Game.Content.Load<SoundEffect>("Soundeffects/Respawn");
                tSoundArray[5] = Game.Content.Load<SoundEffect>("Soundeffects/Hit");
                tSoundArray[6] = Game.Content.Load<SoundEffect>("Soundeffects/Boost");

                Texture2D[] tTextures = new Texture2D[3];
                tTextures[0] = Game.Content.Load<Texture2D>("Bob/GlowTextures/full_health");
                tTextures[1] = Game.Content.Load<Texture2D>("Bob/GlowTextures/mid_health");
                tTextures[2] = Game.Content.Load<Texture2D>("Bob/GlowTextures/low_health");

                Circuit.S_Action.PlayerClasses.Player tPlayerONE = new Circuit.S_Action.PlayerClasses.Player(PlayerIndex.One, mArena, this, tControlclass, tSoundArray, tTextures);
                tPlayerONE.mModel.IS_PLAYER = true;
                Circuit.S_Action.PlayerClasses.Player tPlayerTWO = new Circuit.S_Action.PlayerClasses.Player(PlayerIndex.Two, mArena, this, tControlclass, tSoundArray, tTextures);
                tPlayerTWO.mModel.IS_PLAYER = true;

                mArena.Add(tPlayerONE, tPlayerTWO);

                RandomizeStartingSpawns();

                CamHandler = new SplitCamHandler(mArena.PlayerONE.CAM, mArena.PlayerTWO.CAM, this.GraphicsDevice, this.Game);
                //CAM = new FreeCamera(GraphicsDevice.Viewport.AspectRatio);

                mArena.PlayerONE.Death += OnPlayerOneDeath;
                mArena.PlayerONE.Suicide += OnPlayerOneSuicide;
                mArena.PlayerONE.Respawn += OnPlayerRespawn;

                mArena.PlayerTWO.Death += OnPlayerTwoDeath;
                mArena.PlayerTWO.Suicide += OnPlayerTwoSuicide;
                mArena.PlayerTWO.Respawn += OnPlayerRespawn;

                mManager.EndGame+= OnGameEnd;
            }

            if (!mLoaded)
                LoadContent(Game.Content);
        }

        private void RandomizeStartingSpawns()
        {
            Random tRand = new Random();
            int spawnONE = tRand.Next(4);
            mArena.PlayerONE.Spawn(mArena.Spawns.ElementAt(spawnONE).Position);
            int spawnTWO = tRand.Next(4);

            while (spawnTWO == spawnONE)
            {
                spawnTWO = tRand.Next(4);
            }
            mArena.PlayerTWO.Spawn(mArena.Spawns.ElementAt(spawnTWO).Position);
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
            _builder.BuildColliders();

            return _builder.GetArena();
        }

        #region Death related Events
        private void OnPlayerOneDeath(Circuit.S_Action.PlayerClasses.Player _player, EventArgs e)
        {
            mDeathScreen.Show(_player.Index);
            mManager.IncreaseScore(PlayerIndex.Two);
        }

        private void OnPlayerOneSuicide(Circuit.S_Action.PlayerClasses.Player _player, EventArgs e)
        {
            mDeathScreen.Show(_player.Index);
            mManager.DecreaseScore(PlayerIndex.One);
        }

        private void OnPlayerTwoDeath(Circuit.S_Action.PlayerClasses.Player _player, EventArgs e)
        {
            mDeathScreen.Show(_player.Index);
            mManager.IncreaseScore(PlayerIndex.One);
        }

        private void OnPlayerTwoSuicide(Circuit.S_Action.PlayerClasses.Player _player, EventArgs e)
        {
            mDeathScreen.Show(_player.Index);
            mManager.DecreaseScore(PlayerIndex.Two);
        }

        private void OnPlayerRespawn(Circuit.S_Action.PlayerClasses.Player _player, EventArgs e)
        {
            mDeathScreen.Hide(_player.Index);
        }

        public void OnGameEnd(object o, PlayerWinArgs e)
        {
            var x = e.Index;
            if ((o as PointManager).mPoints[1] - (o as PointManager).mPoints[0] == Data.POINTS_TO_WIN) mResultScreen.SwitchWindows();
            mResultScreen.Show(PlayerIndex.One);
            mResultScreen.Show(PlayerIndex.Two);
            mEndGame = true;
        }
        #endregion

        private void UpdateTime(GameTime _GT)
        {
            mCurrentTimer -= _GT.ElapsedGameTime.TotalSeconds;
            mMinutes = ((int)(mCurrentTimer / 60)).ToString();
            if(mCurrentTimer%60 >=10) mSeconds = ((int) mCurrentTimer % 60).ToString();
            else mSeconds = "0" + ((int)mCurrentTimer % 60).ToString();

            if (mCurrentTimer <= 0)
            {
                mCurrentTimer = 0;
                if (mManager.mPoints[0] == mManager.mPoints[1]) Data.POINTS_TO_WIN = Math.Abs(mManager.mPoints[0] - mManager.mPoints[1]) + 1;
                else if (mManager.mPoints[0] > mManager.mPoints[1]) { Math.Abs(mManager.mPoints[0] - mManager.mPoints[1]); OnGameEnd(mManager, new PlayerWinArgs(PlayerIndex.One)); }
                else
                {
                    Data.POINTS_TO_WIN = Math.Abs(mManager.mPoints[0] - mManager.mPoints[1]);
                    OnGameEnd(mManager, new PlayerWinArgs(PlayerIndex.Two));
                }
            }
        }

        public override void Update(GameTime _GT)
        {
            if (this.Visible && this.Enabled)
            {
                mArena.Update(_GT);
                if (Data.USE_TIMELIMIT && !mEndGame) UpdateTime(_GT);
                // Debug Cam
                //CAM.Update(_GT);
                Matrix rotation = Matrix.CreateRotationY(MathHelper.ToRadians(rotationLight));
                NormalMapMat.LightPosition = Vector3.Transform(NormalMapMat.LightPosition, rotation);


                //NormalMapMat.LightPosition = mArena.PlayerONE.Position;

                if (Keyboard.GetState().IsKeyUp(Keys.Escape) && mOldKey.IsKeyDown(Keys.Escape) || 
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back) || 
                    GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.Back) || 
                    (mEndGame && GamePad.GetState(PlayerIndex.Two).IsButtonUp(Buttons.A) && mOldState2.IsButtonDown(Buttons.A)) ||
                    (mEndGame && GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.A) && mOldState.IsButtonDown(Buttons.A)))
                {
                    mResultScreen.Hide(PlayerIndex.One);
                    mResultScreen.Hide(PlayerIndex.Two);
                    mDeathScreen.Hide(PlayerIndex.One);
                    mDeathScreen.Hide(PlayerIndex.Two);
                    mCrosses.Hide(PlayerIndex.One);
                    mCrosses.Hide(PlayerIndex.Two);
                    mScoreScreen.Hide(PlayerIndex.One);
                    mScoreScreen.Hide(PlayerIndex.Two);
                    mArena = null;
                    Data.POINTS_TO_WIN = mTempPointLimit;
                    ExitAction(this, EventArgs.Empty);
                }

                mOldState = GamePad.GetState(PlayerIndex.One);
                mOldState2 = GamePad.GetState(PlayerIndex.Two);
                mOldKey = Keyboard.GetState();
            }
            base.Update(_GT);
        }

        public override void Draw(GameTime _GT)
        {
            base.Draw(_GT);

            SpriteBatch sp = new SpriteBatch(Game.GraphicsDevice);
            sp.Begin();
            sp.DrawString(Game.Content.Load<SpriteFont>("Fonts/startMenuFont"),
                            mManager.mPoints[0].ToString(),
                            new Vector2(25, 25),
                            Color.White);
            if(Data.USE_TIMELIMIT)
                sp.DrawString(Game.Content.Load<SpriteFont>("Fonts/startMenuFont"),
                mMinutes+":"+mSeconds,
                new Vector2(450, 360),
                Color.White);
            sp.DrawString(Game.Content.Load<SpriteFont>("Fonts/startMenuFont"),
                            mManager.mPoints[1].ToString(),
                            new Vector2(25, 25 + Game.GraphicsDevice.Viewport.Height / 2),
                            Color.White);

            sp.End();
        }

        public override void Hide()
        {
            mArena = null;
            base.Hide();
        }

    }
}
