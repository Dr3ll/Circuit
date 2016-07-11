using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Circuit.Utilities 
{
    class OptionsSubmenu : Microsoft.Xna.Framework.DrawableGameComponent, DrawableEntity2D
    {
        Submenu[] menuItems;

        int selectedIndex;

        Color normal = Color.White;
        Color hilite = Color.Red;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GamePadState controllerState;
        GamePadState oldControllerState;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        SoundEffect mMenuHighlight;
        SoundEffect mMenuAccept;

        Vector2 position;
        float width = 0f;
        float height = 0f;

        Game mGame;

        //Events
        // StartGame Event wenn im Menü Start Game ausgesucht wurde
        public delegate void EnterMenuItemHandler(OptionsSubmenu _MenuComponent, EventArgs e);
        public event EnterMenuItemHandler EnterMenuItem;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = 0;
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
        }

        internal Submenu[] MenuItems
        {
            get { return menuItems; }
            set { menuItems = value; }
        }

        public int Count()
        {
            return menuItems.Count();
        }

        public OptionsSubmenu(Game game,
            SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Submenu[] menuItems)
            : base(game)
        {
            this.mGame = game;
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            mMenuAccept = mGame.Content.Load<SoundEffect>("Soundeffects/MenuAccept");
            mMenuHighlight = mGame.Content.Load<SoundEffect>("Soundeffects/Menu");
            this.menuItems = menuItems;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (Submenu item in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(item.Name);
                if (size.X > width) width = size.X;
                height += spriteFont.LineSpacing + 5;
            }

            position = new Vector2(100, 200);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {            
            base.Initialize();
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            #region Keyboard

            keyboardState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length) selectedIndex = 0;
                mMenuHighlight.Play(Data.S_MENUHIGHLIGHTVOL * Data.S_MASTERVOLUME, 0, 0);
            }

            if (CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
                mMenuHighlight.Play(Data.S_MENUHIGHLIGHTVOL * Data.S_MASTERVOLUME, 0, 0);
                
            }

            if (CheckKey(Keys.Left))
            {
                menuItems[selectedIndex].Subindex -= 1;
            }

            if (CheckKey(Keys.Right))
            {
                menuItems[selectedIndex].Subindex += 1;
            }

            if (CheckKey(Keys.Enter) || CheckKey(Keys.Space))
            {
                if (EnterMenuItem != null)
                {
                    mMenuAccept.Play(Data.S_MENUACCEPT * Data.S_MASTERVOLUME, 0, 0);
                    EnterMenuItem(this, new EventArgs());
                }
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;

            #endregion

            #region Controller

            controllerState = GamePad.GetState(PlayerIndex.One);

            if ((   controllerState.DPad.Down == ButtonState.Pressed
                 && oldControllerState.DPad.Down == ButtonState.Released)
              ||(   controllerState.ThumbSticks.Left.Y > -.5f
                 && oldControllerState.ThumbSticks.Left.Y < -.5f))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length) selectedIndex = 0;
                mMenuHighlight.Play(Data.S_MENUHIGHLIGHTVOL * Data.S_MASTERVOLUME, 0, 0);
            }
            
            if ((   controllerState.DPad.Up == ButtonState.Pressed
                 && oldControllerState.DPad.Up == ButtonState.Released)
              ||(   controllerState.ThumbSticks.Left.Y < .5f
                 && oldControllerState.ThumbSticks.Left.Y > .5f))
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
                mMenuHighlight.Play(Data.S_MENUHIGHLIGHTVOL * Data.S_MASTERVOLUME, 0, 0);
            }


            if ((   controllerState.DPad.Left == ButtonState.Pressed
                 && oldControllerState.DPad.Left == ButtonState.Released)
              ||(   controllerState.ThumbSticks.Left.X > -.5f
                 && oldControllerState.ThumbSticks.Left.X < -.5f))
            {
                menuItems[selectedIndex].Subindex -= 1;
            }
            
            if ((   controllerState.DPad.Right == ButtonState.Pressed
                 && oldControllerState.DPad.Right == ButtonState.Released)
              ||(   controllerState.ThumbSticks.Left.X < .5f
                 && oldControllerState.ThumbSticks.Left.X > .5f))
            {
                menuItems[selectedIndex].Subindex += 1;
            }
            //if ((   controllerState.DPad.Right == ButtonState.Pressed
            //     && oldControllerState.DPad.Right == ButtonState.Released)
            //  ||(   controllerState.ThumbSticks.Left.X > .5f
            //     && oldControllerState.ThumbSticks.Left.X < .5f))
            //{
            //    selectedIndex++;
            //    if (selectedIndex == menuItems.Length) selectedIndex = 0;
            //}

            //if ((   controllerState.DPad.Left == ButtonState.Pressed
            //     && oldControllerState.DPad.Left == ButtonState.Released)
            //  ||(   controllerState.ThumbSticks.Left.X < -.5f
            //     && oldControllerState.ThumbSticks.Left.X > -.5f))
            //{
            //    selectedIndex--;
            //    if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
            //}

            if (GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.A) && oldControllerState.IsButtonDown(Buttons.A))
            {
                if (EnterMenuItem != null && selectedIndex==menuItems.Count()-1)
                {
                    mMenuAccept.Play(Data.S_MENUACCEPT * Data.S_MASTERVOLUME, 0, 0);
                    EnterMenuItem(this, new EventArgs());
                }
            }

            oldControllerState = GamePad.GetState(PlayerIndex.One);

            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 location = position;
            Color tint;
            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex) tint = hilite;
                else tint = normal;

                spriteBatch.DrawString(spriteFont,
                                       menuItems[i].Name,
                                       location,
                                       tint);

                spriteBatch.DrawString(spriteFont, menuItems[i].SubmenuItems[menuItems[i].Subindex], location + new Vector2(Data.R_VIEWPORTWIDTH/2, 0), Color.White);

                location.Y += spriteFont.LineSpacing + 15;
            }

            spriteBatch.End();
        }
    }
}
