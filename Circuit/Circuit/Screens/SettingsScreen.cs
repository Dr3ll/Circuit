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
using System.Xml;

namespace Circuit.Screens
{
    class SettingsScreen : GameScreen
    {
        #region Events

        // ExitCredits Event wenn im CreditsMenu Escp gedrückt wurde
        public delegate void ExitSettingsHandler(SettingsScreen _settingsScreen, EventArgs e);
        public event ExitSettingsHandler ExitSettings;

        public delegate void ChangeResolutionHandler(SettingsScreen _settingsScreen, EventArgs e);
        public event ChangeResolutionHandler ChangeResolution;

        #endregion

        Game mGame;

        SpriteBatch SB;
        SpriteFont mFont;

        Texture2D mBackground;

        OptionsSubmenu mOptions;

        GamePadState mState;
        GamePadState mOldState;

        KeyboardState mKeyState;
        KeyboardState mOldKey;

        XmlTextWriter settingsXML;

        public SettingsScreen(Game GAME)
            : base(GAME)
        {
            mGame = GAME;
        }

        public override void Initialize()
        {
            base.Initialize();

                SB = new SpriteBatch(GraphicsDevice);
                if (!mLoaded)
                {
                    LoadContent(Game.Content);
                    GenerateSubmenuEntries(Game);
                    this.mLoaded = true;
                }
            
        }

        private void GenerateSubmenuEntries(Game GAME)
        {
            Submenu tFullscreen = new Submenu("Fullscreen", Data.SM_FULLSCREEN, 1);
            //Submenu tResolution = new Submenu("Resolution", Data.SM_RESOLUTION);
            Submenu tVolume = new Submenu("Soundeffect Volume", Data.SM_SOUND, 10);
            Submenu tUseTime = new Submenu("Use Time Limit", Data.SM_USETIMELIMIT,1);
            Submenu tTime = new Submenu("Time Limit", Data.SM_TIMELIMIT);
            Submenu tPoint = new Submenu("Point Limit", Data.SM_POINTLIMIT, 4);
            Submenu tSensitivity1 = new Submenu("Camera 1 Sensitivity", Data.SM_SENSITIVITY,5);
            Submenu tAxisX1 = new Submenu("Invert Camera 1 X-axis", Data.SM_INVERTX,1);
            Submenu tAxisY1 = new Submenu("Invert Camera 1 Y-axis", Data.SM_INVERTY,1);
            Submenu tSensitivity2 = new Submenu("Camera 2 Sensitivity", Data.SM_SENSITIVITY,5);
            Submenu tAxisX2 = new Submenu("Invert Camera 2 X-axis", Data.SM_INVERTX,1);
            Submenu tAxisY2 = new Submenu("Invert Camera 2 Y-axis", Data.SM_INVERTY,1);
            
            Submenu tApply = new Submenu("Apply", new string[1]);

            Submenu[] tSubmenu = { tFullscreen, tVolume, tUseTime, tTime, tPoint, tSensitivity1, tAxisX1, tAxisY1, tSensitivity2, tAxisX2, tAxisY2, tApply };

            mOptions = new OptionsSubmenu(GAME, SB, mFont, tSubmenu);
            this.Add(mOptions);
            mOptions.EnterMenuItem += new OptionsSubmenu.EnterMenuItemHandler(OnEnterMenuItem);
        }

        private void LoadContent(ContentManager CM)
        {
            mFont = CM.Load<SpriteFont>("Fonts/OptionsMenu");
            mBackground = CM.Load<Texture2D>("OptionsScreen");
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Visible && this.Enabled)
            {
                mState = GamePad.GetState(PlayerIndex.One);
                mKeyState = Keyboard.GetState();

                if (mState.IsButtonUp(Buttons.Back) && mOldState.IsButtonDown(Buttons.Back)) ExitSettings(this, EventArgs.Empty);
                if (mKeyState.IsKeyUp(Keys.Escape) && mOldKey.IsKeyDown(Keys.Escape)) 
                    ExitSettings(this, EventArgs.Empty);

                mOptions.Update(gameTime);

                mOldState = mState;
                mOldKey = mKeyState;
                //base.Update(gameTime);
            }
        }

        private void ApplyChanges()
        {
            #region Display
            if (mOptions.MenuItems[0].Subindex == 0) Data.R_FULLSCREEN = true;
            else Data.R_FULLSCREEN = false;

            #endregion

            Data.S_MASTERVOLUME = (float)float.Parse(mOptions.MenuItems[1].SubmenuItems[mOptions.MenuItems[1].Subindex])/10;
            #region Rules
            if (mOptions.MenuItems[2].Subindex == 0) Data.USE_TIMELIMIT = true;
            else Data.USE_TIMELIMIT = false;

            Data.TIMELIMIT = Int16.Parse(mOptions.MenuItems[3].SubmenuItems[mOptions.MenuItems[3].Subindex]);
            Data.POINTS_TO_WIN = Int16.Parse(mOptions.MenuItems[4].SubmenuItems[mOptions.MenuItems[4].Subindex]);
            #endregion

            #region Player 1
            Data.O_CAMERA_SENSITIVITY1 = float.Parse(mOptions.MenuItems[5].SubmenuItems[mOptions.MenuItems[5].Subindex]);
            
            if (mOptions.MenuItems[6].Subindex == 0) Data.O_INVERT_X1= true;
            else Data.O_INVERT_X1= false;

            if (mOptions.MenuItems[7].Subindex == 0) Data.O_INVERT_Y1= true;
            else Data.O_INVERT_Y1 = false;
            #endregion

            #region Player 2
            Data.O_CAMERA_SENSITIVITY2 = float.Parse(mOptions.MenuItems[8].SubmenuItems[mOptions.MenuItems[8].Subindex]);

            if (mOptions.MenuItems[9].Subindex == 0) Data.O_INVERT_X2 = true;
            else Data.O_INVERT_X2 = false;

            if (mOptions.MenuItems[10].Subindex == 0) Data.O_INVERT_Y2 = true;
            else Data.O_INVERT_Y2 = false;
            #endregion

            if(ChangeResolution!=null) ChangeResolution(this, EventArgs.Empty);
        }

        private void GenerateSettingsXML()
        {
            settingsXML = new XmlTextWriter("settings.xml", System.Text.Encoding.UTF8);
            settingsXML.WriteStartDocument();
            settingsXML.WriteStartElement("Settings");

            #region Display
            settingsXML.WriteStartElement("Display");
            settingsXML.WriteStartElement("Fullscreen");
            settingsXML.WriteValue(Data.R_FULLSCREEN);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("ResolutionX");
            settingsXML.WriteValue(Data.R_VIEWPORTWIDTH);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("ResolutionY");
            settingsXML.WriteValue(Data.R_VIEWPORTHEIGHT);
            settingsXML.WriteEndElement();
            settingsXML.WriteEndElement();
            #endregion

            #region Sound
            settingsXML.WriteStartElement("Sound");
            settingsXML.WriteStartElement("MasterVolume");
            settingsXML.WriteValue(Data.S_MASTERVOLUME);
            settingsXML.WriteEndElement();
            settingsXML.WriteEndElement();
            #endregion

            #region GameRules
            settingsXML.WriteStartElement("GameRules");
            settingsXML.WriteStartElement("UseTimeLimit");
            settingsXML.WriteValue(Data.USE_TIMELIMIT);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("TimeLimit");
            settingsXML.WriteValue(Data.TIMELIMIT);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("PointLimit");
            settingsXML.WriteValue(Data.POINTS_TO_WIN);
            settingsXML.WriteEndElement();
            settingsXML.WriteEndElement();
            #endregion

            #region Player1
            settingsXML.WriteStartElement("Player1");
            settingsXML.WriteStartElement("Sensitivity");
            settingsXML.WriteValue(Data.O_CAMERA_SENSITIVITY1);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("InvertX");
            settingsXML.WriteValue(Data.O_INVERT_X1);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("InvertY");
            settingsXML.WriteValue(Data.O_INVERT_Y1);
            settingsXML.WriteEndElement();
            settingsXML.WriteEndElement();
            #endregion

            #region Player2
            settingsXML.WriteStartElement("Player2");
            settingsXML.WriteStartElement("Sensitivity");
            settingsXML.WriteValue(Data.O_CAMERA_SENSITIVITY2);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("InvertX");
            settingsXML.WriteValue(Data.O_INVERT_X2);
            settingsXML.WriteEndElement();
            settingsXML.WriteStartElement("InvertY");
            settingsXML.WriteValue(Data.O_INVERT_Y2);
            settingsXML.WriteEndElement();
            settingsXML.WriteEndElement();
            #endregion

            settingsXML.WriteEndElement();
            settingsXML.WriteEndDocument();
            settingsXML.Close();
        }

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            SB.Draw(mBackground,new Rectangle(0,0, Data.R_VIEWPORTWIDTH,Data.R_VIEWPORTHEIGHT),Color.White);

            SB.End();
            mOptions.Draw(GT);
            //base.Draw(GT);
        }

        private void OnEnterMenuItem(OptionsSubmenu _optionsSubmenu, EventArgs e)
        {
            if (mOptions.SelectedIndex == mOptions.Count()-1)
            {
                mOptions.SelectedIndex = 0;
                ApplyChanges();
                GenerateSettingsXML();
                ExitSettings(this, EventArgs.Empty);
            }
        }
    }
}
