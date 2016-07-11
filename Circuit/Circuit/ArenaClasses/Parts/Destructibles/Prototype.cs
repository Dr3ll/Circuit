using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.ArenaClasses.Destructibles
{
    interface Prototype
    {
        Circuit.ArenaClasses.Destructibles.DestructibleObject Clone();
    }
}
