using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuit.Settings
{
    class SettingsCollection
    {
        GraficSettings      mGraficSettings;
        GameplaySettings    mGameplaySettings;
        static SettingsCollection mSettings;
        static string mContentpath;

        public static string SetContentpath
        {
            set 
            {
                SettingsCollection.mContentpath = value + "/Settings";
                System.IO.Directory.CreateDirectory(mContentpath);
            }
        }

        private SettingsCollection()
        {
            mGraficSettings = new GraficSettings(mContentpath);
            mGameplaySettings = new GameplaySettings(mContentpath);
        }

        public static GraficSettings GraficSettings
        {
            get 
            { 
                return mSettings.mGraficSettings;
            }
        }

        public static GameplaySettings GameplaySettings
        {
            get
            {
                return mSettings.mGameplaySettings;
            }
        }

        public static void Setup()
        {
            if (mSettings == null)
                mSettings = new SettingsCollection();
        }
    }
}
