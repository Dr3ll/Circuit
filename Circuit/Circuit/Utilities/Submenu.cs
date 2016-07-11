using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.Utilities
{
    class Submenu
    {
        string mName;
        string[] mSubmenuItems;
        int mIndex;

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        public string[] SubmenuItems
        {
            get { return mSubmenuItems; }
            set { mSubmenuItems = value; }
        }

        public int Subindex
        {
            get { return mIndex; }
            set {
                mIndex = value;
                if (mIndex < 0) mIndex = mSubmenuItems.Count()-1;
                if (mIndex == mSubmenuItems.Count()) mIndex = 0;
            }
        }

        public Submenu(string _name, string[] _menuItems, int _index=0)
        {
            mName = _name;
            mSubmenuItems = _menuItems;
            mIndex = _index;
            
            if (mSubmenuItems[0] == null) mSubmenuItems[0] = " ";

        }
    }
}
