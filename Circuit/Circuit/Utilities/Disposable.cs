using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities
{
    interface Disposable
    {
        event EventHandler Dispose;
        void Update(GameTime _GT);
    }
}
