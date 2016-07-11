using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Circuit.ArenaClasses.ArenaBuilders
{
    abstract class ArenaBuilder
    {
        protected Game GAME;
        protected string mDirectory;
        protected string mInsFile = "/Instructions.txt";
        protected ArenaInstructions mInstructions;

        public ArenaBuilder(Game _GAME)
        {
            GAME = _GAME;
        }

        public abstract void SetName();
        public abstract void BuildSpace();
        public abstract void BuildBase();
        public abstract void BuildSky();
        public abstract void BuildDestructibles();
        public abstract void BuildSpawnPoints();
        public abstract void BuildFragments();
        public abstract void BuildColliders();

        public abstract Arena GetArena();

        protected void ReadInstructions()
        {
            StreamReader tReader;
            List<string> tLines;
            tLines = new List<string>();

            try {
                tReader = new StreamReader(GAME.Content.RootDirectory + '/' + mDirectory + mInsFile);
            }
            catch (FileNotFoundException e)
            {
                throw new ArenaBuilderInstructionsFileMissing(mDirectory);
            }

            using (tReader)
            {
                string tLine = tReader.ReadLine();
                while (tLine != null)
                {
                    tLines.Add(tLine);
                    tLine = tReader.ReadLine();
                }
            }

            // Decrypt read lines
            string tArenaName;
            List<Vector3> tDPosBOX = new List<Vector3>();
            List<Vector3> tDOriBOX = new List<Vector3>();
            List<Vector3> tDPosCY = new List<Vector3>();
            List<Vector3> tDOriCY = new List<Vector3>();
            List<Vector3> tSpawns = new List<Vector3>();

            #region Decrypt file

            // First line contains ArenaName
            tArenaName = tLines[0];
            tLines.Remove(tLines[0]);

            // Next lines is empty
            tLines.Remove(tLines[0]);

            // Next line says 'DPositionsBox'
            tLines.Remove(tLines[0]);

            // The following lines until empty line contain DestructiblesPositions
            while(tLines[0] != "")
            {
                tDPosBOX.Add(DecryptVector3(tLines[0]));
                tLines.Remove(tLines[0]);
            }

            // Next line is empty
            tLines.Remove(tLines[0]);

            // Next line says 'DOrientationsBox'
            tLines.Remove(tLines[0]);

            // The following lines until empty line contain DestructiblesPositions
            while (tLines[0] != "")
            {
                tDOriBOX.Add(DecryptVector3(tLines[0]));
                tLines.Remove(tLines[0]);
            }

            // Next lines is empty
            tLines.Remove(tLines[0]);

            // Next line says 'DPositionsCylinder'
            tLines.Remove(tLines[0]);

            // The following lines until empty line contain DestructiblesPositions
            while (tLines[0] != "")
            {
                tDPosCY.Add(DecryptVector3(tLines[0]));
                tLines.Remove(tLines[0]);
            }

            // Next line is empty
            tLines.Remove(tLines[0]);

            // Next line says 'DOrientationsCylinder'
            tLines.Remove(tLines[0]);

            // The following lines until empty line contain DestructiblesPositions
            while(tLines[0] != "")
            {
                tDOriCY.Add(DecryptVector3(tLines[0]));
                tLines.Remove(tLines[0]);
            }

            // Next line is empty
            tLines.Remove(tLines[0]);

            // Next line says 'SpawnPoints'
            tLines.Remove(tLines[0]);

            // The following lines until empty line contain SpawnPoints
            while (tLines[0] != "")
            {
                tSpawns.Add(DecryptVector3(tLines[0]));
                tLines.Remove(tLines[0]);
            }

            // Next line says 'End of file'
            tLines.Remove(tLines[0]);

            #endregion

            mInstructions = new ArenaInstructions(
                tArenaName, 
                tDPosBOX, 
                tDOriBOX, 
                tDPosCY,
                tDOriCY,
                tSpawns);
        }

        private Vector3 DecryptVector3(string _in)
        {
            string anus = _in.Substring(_in.IndexOf('Z') + 1, _in.IndexOf(')') - 2 - _in.IndexOf('Z'));

            return new Vector3(Convert.ToInt16(_in.Substring(_in.IndexOf('X') + 1, _in.IndexOf('Y') - 3)),
                               Convert.ToInt16(_in.Substring(_in.IndexOf('Y') + 1, _in.IndexOf('Z') - 2 - _in.IndexOf('Y'))),
                               Convert.ToInt16(_in.Substring(_in.IndexOf('Z') + 1, _in.IndexOf(')') - 1 - _in.IndexOf('Z'))));
        }
    }

    class ArenaBuilderInstructionsFileMissing : Exception
    {
        public ArenaBuilderInstructionsFileMissing(string _arenaType) : base(_arenaType + " is missing an instructions file.") { }
    }
}
