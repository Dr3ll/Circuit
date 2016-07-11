using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.Settings
{
    class GameplaySettings : Settings
    {

        public GameplaySettings(string _path)
            : base()
        {
            mFileName = "GraficSettings";
        }

        protected override void WriteValue(string _cat, string _value)
        { }

        protected override void WriteDefaults(string _path)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToFile(string _path)
        {
            throw new NotImplementedException();
        }
    }
}
