using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;


namespace Circuit.Settings
{
    class GraficSettings : Settings
    {
        // To add a new settings categorie change at the following positions:
        // Members list (+ Getter/Setter)
        // Constructor string list
        // WriteValue() method switch
        Point mResolution;
        bool mFullscreen;
        bool mShader1;
        bool mShader2;
        bool mShader3;

        public Point Resolution
        {
            get { return mResolution; }
            set { if (base.mWriteState) mResolution = value; }
        }
        public bool Fullscreen
        {
            get { return mFullscreen; }
            set { if(base.mWriteState) mFullscreen = value; }
        }
        public bool Shader1
        {
            get { return mShader1; }
            set { if (base.mWriteState) mShader1 = value; }
        }
        public bool Shader2
        {
            get { return mShader2; }
            set { if (base.mWriteState) mShader2 = value; }
        }
        public bool Shader3
        {
            get { return mShader3; }
            set { if (base.mWriteState) mShader3 = value; }
        }

        public GraficSettings(string _path) 
            : base ()
        {
            mFileName = "GraficSettings.txt";

            base.WriteCategories( new string[]
            {
                   "Resolution",
                   "Fullscreen",
                   "Shader1",
                   "Shader2",
                   "Shader3"
            });

            ReadFile(_path);
        }

        public void ReadFile(String _path)
        {
            List<string> lines = new List<string>();

            StreamReader tReader;

            try {
                tReader = new StreamReader(_path + mFileName);
            }
            catch (FileNotFoundException e)
            {
                StreamWriter tSW = File.CreateText(_path + '/' + mFileName);
                tSW.Close();
                WriteDefaults(_path);
                tReader = new StreamReader(_path + '/' + mFileName);
            }

            base.BeginWriting();

            using (tReader)
            {
                string line = tReader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = tReader.ReadLine();
                }
            }

            for (int y = 0; y < lines.Count; ++y)
            {
                string tCategorie = "";
                string tValue = "";
                int x = 0;

                while (!lines[y][x].Equals(' '))
                {
                    tCategorie += lines[y][x];
                    ++x;
                }
                x = x + 1;
                while (x < lines[y].Length)
                {
                    tValue += lines[y][x];
                    ++x;
                }

                WriteValue(tCategorie, tValue);

                base.EndWriting();
            }

            WriteToFile(_path);
        }

        protected override void WriteDefaults(string _path)
        {
            base.BeginWriting();

            mResolution = new Point(1280,720);
            mFullscreen = false;
            mShader1    = true;
            mShader2    = true;
            mShader3    = true;

            base.EndWriting();

            WriteToFile(_path);
        }

        protected override void WriteToFile(string _path)
        {
            StringBuilder tSB = new StringBuilder();

            tSB.AppendLine("Resolution " + mResolution.X + 'x' + mResolution.Y);
            tSB.AppendLine("Fullscreen " + mFullscreen);
            tSB.AppendLine("Shader1 " + mShader1);
            tSB.AppendLine("Shader2 " + mShader2);
            tSB.AppendLine("Shader3 " + mShader3);


            using (StreamWriter tWriter =
                new StreamWriter(_path + '/' + mFileName))
            {
                tWriter.Write(tSB.ToString());
            }
        }

        protected override void WriteValue(string _cat, string _value)
        {
            if(!base.CheckCategorie(_cat))
                throw new SettingsCategorieNotRecognised(_cat);

            switch (_cat)
            {
                case "Resolution":
                    this.Resolution = ToResolution(_value);
                    break;
                case "Fullscreen":
                    this.Fullscreen = ToBool(_value);
                    break;
                case "Shader1":
                    this.Shader1 = ToBool(_value);
                    break;
                case "Shader2":
                    this.Shader2 = ToBool(_value);
                    break;
                case "Shader3":
                    this.Shader3 = ToBool(_value);
                    break;
            }
        }

        private Point ToResolution(string _value)
        {
            return new Point(Convert.ToInt16(_value.Substring(0, _value.IndexOf('x'))),
                             Convert.ToInt16(_value.Substring(_value.IndexOf('x') + 1)));
        }
    }
}
