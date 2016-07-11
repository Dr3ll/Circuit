using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.Utilities.Cameras
{
    class StandartCamHandler : CameraHandler
    {
        private Camera mCam;
        private GraphicsDevice mGraphics;
        private Viewport mViewport;

        public StandartCamHandler(Camera _cam, GraphicsDevice _graphics)
        {
            mGraphics = _graphics;
            mViewport = new Viewport(0, 0, Data.R_VIEWPORTWIDTH, Data.R_VIEWPORTHEIGHT);
            mCam = _cam;

            mGraphics.Viewport = mViewport;
        }

        public void Draw(GameTime _GT, List<DrawableEntity3D> _comps)
        {
            foreach (DrawableEntity3D _c in _comps)
            {
                _c.Draw(_GT, mCam);
            }
        }
    }
}
