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
using Circuit.Screens;
using Circuit.Utilities;
using Circuit.ArenaClasses;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using Circuit.S_Action.PlayerClasses.Programms.Optimize;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;
using Circuit.S_Action.PlayerClasses.Programms.GoTo;
using System.Xml;

namespace Circuit
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager GRAPHICS;
        SpriteBatch SB;

        IntroScreen INTROSCREEN;
        StartScreen STARTSCREEN;
        ActionScreen ACTIONSCREEN;
        CreditsScreen CREDITSSCREEN;
        SettingsScreen SETTINGSSCREEN;
        InstructionsScreen INSTRUCTIONSSCREEN;

        XmlTextWriter settingsXML;

        public Game1()
        {
            Components.Add(new ControlClass(this));
            GRAPHICS = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);
            //this.IsFixedTimeStep = false;
            //GRAPHICS.SynchronizeWithVerticalRetrace = false;

            ReadAndApplyXMLConfig();

            this.TargetElapsedTime = new TimeSpan(120000);
        }

        #region Constructor

        private void ReadAndApplyXMLConfig()
        {
            // "Display" Variables
            bool fullscreen = Data.R_FULLSCREEN;
            int resolutionX = Data.R_VIEWPORTWIDTH;
            int resolutionY = Data.R_VIEWPORTHEIGHT;

            // "Sound" Variables
            float masterVolume = Data.S_MASTERVOLUME;

            // "Game Rules" Variables
            bool timeOn = Data.USE_TIMELIMIT;
            int timeLimit = Data.TIMELIMIT;
            int points = Data.POINTS_TO_WIN;

            // "Player 1" Variables
            float camSensitivity1 = Data.O_CAMERA_SENSITIVITY1;
            bool camX1 = Data.O_INVERT_X1;
            bool camY1 = Data.O_INVERT_Y1;

            // "Player 2" Variables
            float camSensitivity2 = Data.O_CAMERA_SENSITIVITY2;
            bool camX2 = Data.O_INVERT_X2;
            bool camY2 = Data.O_INVERT_Y2;

            try
            {
                System.Xml.XmlTextReader xmlConfigReader = new System.Xml.XmlTextReader("settings.xml");

                #region Read Display
                xmlConfigReader.ReadToDescendant("Display");

                //Fullscreen
                xmlConfigReader.ReadToDescendant("Fullscreen");
                fullscreen = xmlConfigReader.ReadElementContentAsBoolean();

                //ResolutionX
                resolutionX = xmlConfigReader.ReadElementContentAsInt();

                //ResolutionY
                resolutionY = xmlConfigReader.ReadElementContentAsInt();
                #endregion

                #region Read Sound
                xmlConfigReader.ReadToNextSibling("Sound");

                xmlConfigReader.ReadToDescendant("MasterVolume");
                masterVolume = xmlConfigReader.ReadElementContentAsFloat();
                #endregion

                #region Read Game Rules
                xmlConfigReader.ReadToNextSibling("GameRules");

                xmlConfigReader.ReadToDescendant("UseTimeLimit");
                timeOn = xmlConfigReader.ReadElementContentAsBoolean();

                //TimeLimit
                timeLimit = xmlConfigReader.ReadElementContentAsInt();

                //Point Limit
                points = xmlConfigReader.ReadElementContentAsInt();
                #endregion

                #region Read Player1
                xmlConfigReader.ReadToNextSibling("Player1");

                xmlConfigReader.ReadToDescendant("Sensitivity");
                camSensitivity1 = xmlConfigReader.ReadElementContentAsFloat();

                //InvertX
                camX1 = xmlConfigReader.ReadElementContentAsBoolean();

                //InvertY
                camY1 = xmlConfigReader.ReadElementContentAsBoolean();
                #endregion

                #region Read Player2
                xmlConfigReader.ReadToNextSibling("Player2");

                xmlConfigReader.ReadToDescendant("Sensitivity");
                camSensitivity2 = xmlConfigReader.ReadElementContentAsFloat();

                //InvertX
                camX2 = xmlConfigReader.ReadElementContentAsBoolean();

                //InvertY
                camY2 = xmlConfigReader.ReadElementContentAsBoolean();
                #endregion



                fullscreen = false;

                xmlConfigReader.Close();
            }
            catch
            {
                // error in xml document - write a new one with standard values
                try
                {
                    settingsXML = new XmlTextWriter("settings.xml", System.Text.Encoding.UTF8);
                    settingsXML.WriteStartDocument();
                    settingsXML.WriteStartElement("Settings");

                    #region Display
                    settingsXML.WriteStartElement("Display");
                    settingsXML.WriteStartElement("Fullscreen");
                    settingsXML.WriteValue(fullscreen);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("ResolutionX");
                    settingsXML.WriteValue(resolutionX);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("ResolutionY");
                    settingsXML.WriteValue(resolutionY);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndElement();
                    #endregion

                    #region Sound
                    settingsXML.WriteStartElement("Sound");
                    settingsXML.WriteStartElement("MasterVolume");
                    settingsXML.WriteValue(masterVolume);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndElement();
                    #endregion

                    #region GameRules
                    settingsXML.WriteStartElement("GameRules");
                    settingsXML.WriteStartElement("UseTimeLimit");
                    settingsXML.WriteValue(timeOn);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("TimeLimit");
                    settingsXML.WriteValue(timeLimit);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("PointLimit");
                    settingsXML.WriteValue(points);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndElement();
                    #endregion

                    #region Player1
                    settingsXML.WriteStartElement("Player1");
                    settingsXML.WriteStartElement("Sensitivity");
                    settingsXML.WriteValue(camSensitivity1);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("InvertX");
                    settingsXML.WriteValue(camX1);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("InvertY");
                    settingsXML.WriteValue(camY1);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndElement();
                    #endregion

                    #region Player2
                    settingsXML.WriteStartElement("Player2");
                    settingsXML.WriteStartElement("Sensitivity");
                    settingsXML.WriteValue(camSensitivity1);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("InvertX");
                    settingsXML.WriteValue(camX2);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteStartElement("InvertY");
                    settingsXML.WriteValue(camY2);
                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndElement();
                    #endregion

                    settingsXML.WriteEndElement();
                    settingsXML.WriteEndDocument();
                    settingsXML.Close();
                }
                catch { }
            }

            Data.R_VIEWPORTHEIGHT = resolutionY;
            Data.R_VIEWPORTWIDTH = resolutionX;
            // apply settings
            if (GRAPHICS.IsFullScreen != fullscreen)
                GRAPHICS.ToggleFullScreen();
            GRAPHICS.PreferredBackBufferWidth = resolutionX;
            GRAPHICS.PreferredBackBufferHeight = resolutionY;

            Data.S_MASTERVOLUME = masterVolume;

            // "Game Rules" Variables
            Data.USE_TIMELIMIT = timeOn;
            Data.TIMELIMIT = timeLimit;
            Data.POINTS_TO_WIN = points;

            // "Player 1" Variables
            Data.O_CAMERA_SENSITIVITY1 = camSensitivity1;
            Data.O_INVERT_X1 = camX1;
            Data.O_INVERT_Y1 = camY1;

            // "Player 2" Variables
            Data.O_CAMERA_SENSITIVITY2 = camSensitivity1;
            Data.O_INVERT_X2 = camX2;
            Data.O_INVERT_Y2 = camY2;
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;


            base.Initialize();

            #region Create screens


            INTROSCREEN = new IntroScreen(this);
            INTROSCREEN.Show();
            Components.Add(INTROSCREEN);
            INTROSCREEN.StartMenu += new IntroScreen.StartMenuHandler(OnStartMenu);

            STARTSCREEN = new StartScreen(this);
            Components.Add(STARTSCREEN);
            STARTSCREEN.Hide();

            STARTSCREEN.StartGame += new StartScreen.StartGameHandler(OnStartGame);
            STARTSCREEN.EnterCredits += new StartScreen.EnterCreditsHandler(OnEnterCredits);
            STARTSCREEN.EnterSettings += new StartScreen.EnterSettingsHandler(OnEnterSettings);
            STARTSCREEN.EnterInstructions += new StartScreen.EnterInstructionsHandler(OnEnterInstructions);

            ACTIONSCREEN = new ActionScreen(this);
            Components.Add(ACTIONSCREEN);
            ACTIONSCREEN.Hide();
            ACTIONSCREEN.ExitAction += new ActionScreen.ExitActionHandler(OnExitAction);

            CREDITSSCREEN = new CreditsScreen(this);
            Components.Add(CREDITSSCREEN);
            CREDITSSCREEN.Hide();
            CREDITSSCREEN.ExitCredits += new CreditsScreen.ExitCreditsHandler(OnExitCredits);

            SETTINGSSCREEN = new SettingsScreen(this);
            Components.Add(SETTINGSSCREEN);
            SETTINGSSCREEN.Hide();
            SETTINGSSCREEN.ExitSettings += new SettingsScreen.ExitSettingsHandler(OnExitSettings);

            INSTRUCTIONSSCREEN = new InstructionsScreen(this);
            Components.Add(INSTRUCTIONSSCREEN);
            INSTRUCTIONSSCREEN.Hide();
            INSTRUCTIONSSCREEN.ExitInstructions += new InstructionsScreen.ExitInstructionsHandler(OnExitInstructions);

            #endregion

            //Components.Add(new FpsCounter(this, "Fonts"));
            Components.Add(new IngamePlayerScreen(this, "Crosshair", this.Content.Load<Texture2D>("crosshair1"), null, -50, 10));
            Components.Add(new IngamePlayerScreen(this, Data.PS_SCORENAME, this.Content.Load<Texture2D>("PlayerScreens/Score-Kasten"), null, -(Data.R_VIEWPORTWIDTH / 2 - 50), -(Data.R_VIEWPORTHEIGHT / 4 - 50)));
            Components.Add(new IngamePlayerScreen(this, Data.PS_DEATHNAME, this.Content.Load<Texture2D>("PlayerScreens/Prompt")));
            Components.Add(new IngamePlayerScreen(this, Data.PS_GAMEENDNAME, this.Content.Load<Texture2D>("PlayerScreens/YouWin"), this.Content.Load<Texture2D>("PlayerScreens/YouLose")));
            //Components.Add(new Debug(this, "Fonts"));
        }

        #endregion

        #region LoadContent

        protected override void LoadContent()
        {
            SB = new SpriteBatch(GraphicsDevice);

            // Startup Prgogramm factotries
            DeleteFactory.Setup(this.Content);
            OptimizeFactory.Setup(this.Content);
            FirewallFactory.Setup(this.Content);
            GoToFactory.Setup(this.Content);
        }

        #endregion

        #region Update

        protected override void Update(GameTime _GT)
        {
            if (this.IsActive)
            {
                base.Update(_GT);
            }
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {





            base.Draw(gameTime);
        }

        #endregion

        #region Subscribed Events

        private void OnStartGame(StartScreen _startScreen, EventArgs e)
        {
            STARTSCREEN.Hide();
            ACTIONSCREEN.Show();
        }

        private void OnExitAction(ActionScreen _actionScreen, EventArgs e)
        {
            ACTIONSCREEN.Hide();
            STARTSCREEN.Show();
        }

        private void OnStartMenu(IntroScreen _introScreen, EventArgs e)
        {
            INTROSCREEN.Hide();
            STARTSCREEN.Show();

        }

        private void OnEnterCredits(StartScreen _startScreen, EventArgs e)
        {
            STARTSCREEN.Hide();
            CREDITSSCREEN.Show();
        }

        private void OnExitCredits(CreditsScreen _creditsScreen, EventArgs e)
        {
            CREDITSSCREEN.Hide();
            STARTSCREEN.Show();
        }

        private void OnEnterSettings(StartScreen _startScreen, EventArgs e)
        {
            STARTSCREEN.Hide();
            SETTINGSSCREEN.Show();
        }

        private void OnExitSettings(SettingsScreen _settingsScreen, EventArgs e)
        {
            SETTINGSSCREEN.Hide();
            STARTSCREEN.Show();
        }

        private void OnEnterInstructions(StartScreen _startScreen, EventArgs e)
        {
            STARTSCREEN.Hide();
            INSTRUCTIONSSCREEN.Show();
        }

        private void OnExitInstructions(InstructionsScreen _instructionsScreen, EventArgs e)
        {
            INSTRUCTIONSSCREEN.Hide();
            STARTSCREEN.Show();
        }

        #endregion
    }
}
