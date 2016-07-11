using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.Utilities
{
    class IngamePlayerScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        bool mIsVisibleUp;
        bool mIsVisibleDown;
        Texture2D mScreen;
        Texture2D mAltScreen;
        SpriteBatch SB;
        private String mName;
        Vector2 mOffset;
        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public IngamePlayerScreen(Game _GAME, String _name, Texture2D _screen, Texture2D _secondScreen=null, int _offsetX=0, int _offsetY=0):base(_GAME)
        {
            SB = new SpriteBatch(_GAME.GraphicsDevice);
            mName = _name;
            mScreen = _screen;

            mOffset = new Vector2(_offsetX, _offsetY);

            if(_secondScreen !=null)mAltScreen=_secondScreen;
            else mAltScreen=mScreen;

            mIsVisibleUp = false;
            mIsVisibleUp = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SwitchWindows()
        {
            Texture2D tTemp = mScreen;
            mScreen = mAltScreen;
            mAltScreen = tTemp;
        }

        public void Show(PlayerIndex _index)
        {
            if (_index == PlayerIndex.One) mIsVisibleUp = true;
            else mIsVisibleDown = true;
        }

        public void Hide(PlayerIndex _index)
        {
            if (_index == PlayerIndex.One) mIsVisibleUp = false;
            else mIsVisibleDown = false;
        }

        public override void Draw(GameTime GT)
        {
            SB.Begin();
            if(mIsVisibleUp)
                SB.Draw(mScreen,new Rectangle(Data.R_VIEWPORTWIDTH/2-mScreen.Width/2+(int)mOffset.X, Data.R_VIEWPORTHEIGHT/4-mScreen.Height/2+(int)mOffset.Y,mScreen.Width, mScreen.Height),Color.White);
            if(mIsVisibleDown)
                SB.Draw(mAltScreen, new Rectangle(Data.R_VIEWPORTWIDTH/2-mScreen.Width/2+(int)mOffset.X, 3*Data.R_VIEWPORTHEIGHT/4-mScreen.Height/2+(int)mOffset.Y, mScreen.Width, mScreen.Height), Color.White);
            SB.End();
        }
    }
}
