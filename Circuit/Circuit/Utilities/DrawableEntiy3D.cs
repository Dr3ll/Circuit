using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities.Cameras;

namespace Circuit.Utilities
{
    public interface DrawableEntity3D
    {
        void Draw(GameTime _GT, Camera _cam);
        void Initialize();
    }
}
