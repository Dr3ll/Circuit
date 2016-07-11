using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.Utilities.Cameras;

namespace Circuit.Utilities
{
    public interface DrawableEntity2D
    {
        void Draw(GameTime _GT);
        void Initialize();
    }
}
