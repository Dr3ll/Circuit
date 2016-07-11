using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Circuit.ArenaClasses.Destructibles;
using Microsoft.Xna.Framework.Graphics;
using Circuit.S_Action.PlayerClasses.Programms;
using Circuit.Utilities;

namespace Circuit.ArenaClasses.ArenaBuilders
{
    class NeutralArenaBuilder : ArenaBuilder
    {
        private Arena mArena;
        private Texture2D NonGlowTex;

        public NeutralArenaBuilder(Game _GAME, Circuit.Screens.GameScreen _SCREEN)
            : base(_GAME)
        {
            mDirectory = "Arenas/NeutralArena";
            base.ReadInstructions();

            mArena = new Arena(_SCREEN);
            NonGlowTex = GAME.Content.Load<Texture2D>("NonGlow");
        }

        public override void SetName()
        {
            mArena.Add(mInstructions.Name);
        }

        public override void BuildSpace()
        {
            mArena.Add(new BEPUphysics.Space());
        }

        public override void BuildBase()
        {
            Model tBase = GAME.Content.Load<Model>(mDirectory + "/Base/base");

            mArena.Add(tBase);
        }

        public override void BuildSky()
        {
            Skybox tSky = new Skybox(GAME, mDirectory + "/Skybox");

            mArena.Add(tSky);
        }

        public override void BuildFragments()
        {
            List<Fragment> tFragments = new List<Fragment>();

            for (int i = 0; i < 25; i++)
            {
                Circuit.S_Action.PlayerClasses.Programms.Delete.DeleteFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Delete.DeleteFragment(new Vector3(0, 60 + i * 10, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
            }

            for (int i = 0; i < 25; i++)
            {
                Circuit.S_Action.PlayerClasses.Programms.Optimize.OptimizeFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Optimize.OptimizeFragment(new Vector3(0, 65 + i * 10, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFragments.Add(tFrag);
            }

            for (int i = 0; i < 25; i++)
            {
                Circuit.S_Action.PlayerClasses.Programms.Firewall.FirewallFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.Firewall.FirewallFragment(new Vector3(0, 60 + i * 10, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFrag.Type = Data.ProgrammType.FIREWALL;
                tFragments.Add(tFrag);
            }

            for (int i = 0; i < 25; i++)
            {
                Circuit.S_Action.PlayerClasses.Programms.GoTo.GoToFragment tFrag = new Circuit.S_Action.PlayerClasses.Programms.GoTo.GoToFragment(new Vector3(0, 65 + i * 10, 0), mArena.Screen.Game);
                tFrag.MakeCollectable();
                tFrag.Type = Data.ProgrammType.GOTO;
                tFragments.Add(tFrag);
            }

            mArena.Add(tFragments);
        }

        public override void BuildColliders()
        {
            List<EntityModel> tColliders = new List<EntityModel>();

            EntityModel tColl;

            tColl = new Collider(
                        new Vector3(-3.2f, 6.3f, .5f),
                        new Vector3(30, 120.5f, 26f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(.52f, 23.45f, 105.59f),
                        new Vector3(29.28f, 123.8f, 25.81f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(-119.44f, 15.49f, 8.8f),
                        new Vector3(29.38f, 139.44f, 25.65f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(34.37f, 16.03f, -92.18f),
                        new Vector3(29.28f, 139.44f, 25.73f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(-5.04f, -43.14f, 2.18f),
                        new Vector3(363.7f, 11.9f, 258.07f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(-1.26f, .44f, 2.54f),
                        new Vector3(109.1f, 4.92f, 81.57f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            tColl = new Collider(
                        new Vector3(-3.46f, 65.66f, -1.22f),
                        new Vector3(45.5f, 1.78f, 45.97f),
                        new Vector3(0, 0, 0),
                        GAME, NonGlowTex);
            tColliders.Add(tColl);

            mArena.Add(tColliders);
        }

        private Data.ProgrammType GetType(int _value)
        {
            switch (_value)
            {
                case 0:
                    return Data.ProgrammType.DELETE;
                case 1:
                    return Data.ProgrammType.FIREWALL;
                case 2:
                    return Data.ProgrammType.OPTIMIZE;
                case 3:
                    return Data.ProgrammType.GOTO;
                default:
                    return Data.ProgrammType.DELETE;
            }
        }

        public override void BuildDestructibles()
        {
            List<DestructibleObject> tDestructibles = new List<DestructibleObject>();

            DestructibleBoxCreator tBoxCreator = new DestructibleBoxCreator(GAME);
            DestructibleCylinderCreator tCylinderCreator = new DestructibleCylinderCreator(GAME);

            Random tRand = new Random();

            foreach (Vector3 _v in mInstructions.Boxes[0])
            {
                DestructibleObject tCopy = tBoxCreator.CreateDestructible(GetType((int)(tRand.NextDouble() * 4)));
                tCopy.Initialize(_v, Vector3.Zero);
                tDestructibles.Add(tCopy);
            }

            foreach (Vector3 _v in mInstructions.Cylinders[0])
            {
                DestructibleObject tCopy = tCylinderCreator.CreateDestructible(GetType((int)(tRand.NextDouble() * 4)));
                tCopy.Initialize(_v, Vector3.Zero);
                tDestructibles.Add(tCopy);
            }

            mArena.Add(tDestructibles);
        }

        public override void BuildSpawnPoints()
        {
            List<SpawnPoint> tSpawns = new List<SpawnPoint>();
            foreach (Vector3 _v in mInstructions.Spawns)
                tSpawns.Add(new SpawnPoint(_v));

            mArena.Add(tSpawns);
        }

        public override Arena GetArena()
        {
            return mArena;
        }
    }
}
