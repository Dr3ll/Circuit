using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.Settings
{
    abstract class Settings
    {
        protected string[] mCategories;
        protected bool mWriteState;
        protected string mFileName;

        protected Settings()
        {
            mWriteState = false;
        }

        protected void WriteCategories(string[] _cat)
        {
            mCategories = _cat;
        }

        protected bool CheckCategorie(string _cat)
        {
            return mCategories.Contains<string>(_cat);
        }

        protected bool ToBool(string _value)
        {
            if (_value.Equals("True") || _value.Equals("true") || _value.Equals("1"))
                return true;
            if (_value.Equals("False") || _value.Equals("false") || _value.Equals("0"))
                return false;

            throw new SettingsValueIsNotBoolean(_value);
        }

        public void BeginWriting()
        {
            mWriteState = true;
        }

        public void EndWriting()
        {
            mWriteState = false; ;
        }

        protected abstract void WriteValue(string _cat, string _value);

        protected abstract void WriteDefaults(string _path);

        protected abstract void WriteToFile(string _path);

        protected class SettingsCategorieNotRecognised : Exception
        {
            public SettingsCategorieNotRecognised(String _categorie)
                : base("The categorie " + _categorie + " is not recognised as a part of this settings collection.") { }
        }
        protected class SettingsCategorieValueMissing : Exception
        {
            public SettingsCategorieValueMissing(String _categorie)
                : base("The categorie " + _categorie + " is missing a value.") { }
        }
        protected class SettingsValueIsNotBoolean : Exception
        {
            public SettingsValueIsNotBoolean(String _value) 
                : base("The value " + _value + " cannot be translated to boolean.") { }
        }
        protected class SettingsWritingStateNotEnabled : Exception
        {
            public SettingsWritingStateNotEnabled()
                : base() { }
        }
        protected class SettingsFileDoesNotExist : Exception
        {
            public SettingsFileDoesNotExist()
                : base() { }
        }
    }
}
