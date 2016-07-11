using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities.Cameras
{
    public interface CameraHandler
    {
        void Draw(GameTime _GT, List<DrawableEntity3D> _comps);
    }
}
