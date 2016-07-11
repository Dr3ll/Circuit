using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.ArenaClasses.ArenaBuilders
{
    interface BuildDirector
    {
        Arena Create(ArenaBuilder _builder);
    }
}
