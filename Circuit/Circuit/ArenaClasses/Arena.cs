using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Circuit.ArenaClasses.Destructibles;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Circuit.S_Action.PlayerClasses.Programms;
using BEPUphysics.CollisionRuleManagement;
using Circuit.S_Action.PlayerClasses;

namespace Circuit.ArenaClasses
{
    class Arena
    {
        // Parts
        List<DestructibleObject> mDestructibles;
        List<SpawnPoint> mSpawns;
        List<Fragment> mFragments;
        List<Trap> mTraps;
        string mName;
        Space mSpace;
        GenericPlane mPlane;

        public DisposeHandler mDisposeHandler;

        // CollisionGroups
        public BEPUphysics.CollisionRuleManagement.CollisionGroup PlayerOneCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup PlayerTwoCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup PlayerOneShotsCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup PlayerTwoShotsCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup TrapsCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup DestructiblesCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup BaseCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup CollidersCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup GoToSplinterCG;
        public BEPUphysics.CollisionRuleManagement.CollisionGroup FirewallChipCG;

        public static EntityModel bla;

        Player mPlayerONE;
        Player mPlayerTWO;

        Circuit.Screens.GameScreen SCREEN;


        public Circuit.Screens.GameScreen Screen
        {
            get { return SCREEN; }
        }
        public DisposeHandler Space
        {
            get { return mDisposeHandler; }
        }
        internal List<Fragment> Fragments
        {
            get { return mFragments; }
        }
        public List<SpawnPoint> Spawns
        {
            get { return mSpawns; }
        }
        public Player PlayerONE
        {
            get { return mPlayerONE; }
        }
        public Player PlayerTWO
        {
            get { return mPlayerTWO; }
        }

        public Arena(Circuit.Screens.GameScreen _SCREEN)
        {
            SCREEN = _SCREEN;

            mTraps = new List<Trap>();
        }

        #region Constructor

        public void Add(string _name)
        {
            mName = _name;
        }
        public void Add(Space _space)
        {
            mSpace = _space;
            mDisposeHandler = new DisposeHandler(mSpace, SCREEN);

            PlayerOneCG = new CollisionGroup();
            PlayerTwoCG = new CollisionGroup();
            PlayerOneShotsCG = new CollisionGroup();
            PlayerTwoShotsCG = new CollisionGroup();
            TrapsCG = new CollisionGroup();
            DestructiblesCG = new CollisionGroup();
            BaseCG = new CollisionGroup();
            CollidersCG = new CollisionGroup();
            GoToSplinterCG = new CollisionGroup();
            FirewallChipCG = new CollisionGroup();


            // Add rules to prevent collision between Players and DeleteTraps
            BEPUphysics.CollisionRuleManagement.CollisionGroupPair tGroupPair;

            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                TrapsCG, PlayerOneCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                TrapsCG, PlayerTwoCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                TrapsCG, GoToSplinterCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            // Add rules to prevent Collision between a Player and his own Shots
            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                PlayerOneShotsCG, PlayerOneCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                PlayerTwoShotsCG, PlayerTwoCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            // Add rules to prevent Collision between GoToSplinters aswell as between GoToSplinters and the base
            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                GoToSplinterCG, GoToSplinterCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            //tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
            //    GoToSplinterCG, BaseCG);
            //CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            //tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
            //    GoToSplinterCG, PlayerOneCG);
            //CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            // Add rule to prevent collision between the basemodel and colliders
            tGroupPair = new BEPUphysics.CollisionRuleManagement.CollisionGroupPair(
                BaseCG, CollidersCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            // Add rule so DestructibleObject parts cant collide with each other
            tGroupPair = new CollisionGroupPair(DestructiblesCG, DestructiblesCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            // Add rules for FirewallChips
            tGroupPair = new CollisionGroupPair(FirewallChipCG, PlayerOneCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new CollisionGroupPair(FirewallChipCG, PlayerTwoCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new CollisionGroupPair(FirewallChipCG, BaseCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new CollisionGroupPair(FirewallChipCG, CollidersCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new CollisionGroupPair(FirewallChipCG, DestructiblesCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            tGroupPair = new CollisionGroupPair(FirewallChipCG, FirewallChipCG);
            CollisionRules.CollisionGroupRules.Add(tGroupPair, CollisionRule.NoBroadPhase);

            if (Environment.ProcessorCount > 1)
            {
                // On windows, just throw a thread at every processor.  The thread scheduler will take care of where to put them.
                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    mSpace.ThreadManager.AddThread();
                }
            }

            mSpace.ForceUpdater.Gravity = new Vector3(0, -40.81f, 0);
        }
        public void Add(Skybox _skybox)
        {
            SCREEN.Add(_skybox);
        }
        public void Add(Model _baseModel)
        {
            Vector3 tPosition = new Vector3(0, -40, 0);

            Vector3[] tVertices;
            int[] tIndices;
            BEPUphysics.DataStructures.TriangleMesh.GetVerticesAndIndicesFromModel(_baseModel, out tVertices, out tIndices);
            var tMesh = new BEPUphysics.Collidables.StaticMesh(tVertices, tIndices, new BEPUphysics.MathExtensions.AffineTransform(tPosition));
            tMesh.CollisionRules.Group = BaseCG;
            mSpace.Add(tMesh);

            StaticModel _sModel = new StaticModel(_baseModel, tMesh.WorldTransform.Matrix, SCREEN.Game);
            _sModel.SetModelEffect(SCREEN.Game.Content.Load<Effect>("Effects/NormalMapEffect"), true);
            _sModel.SetModelMaterial(SCREEN.NormalMapMat);

            _sModel.GlowTex = SCREEN.Game.Content.Load<Texture2D>("brick_texture_map");

            SCREEN.Add(_sModel);
        }
        public void Add(List<EntityModel> _colliders)
        {
            // Invisible boxes are added to the world to ensure correct collisions in critical areas

            foreach (EntityModel _m in _colliders)
            {
                _m.Entity.CollisionInformation.CollisionRules.Group = CollidersCG;
                Space.Add(_m);

                // Add to visible space for debug purposes
                //SCREEN.Add(_m);
                bla = _m;
            }
        }
        public void Add(List<DestructibleObject> _destructibles)
        {
            mDestructibles = _destructibles;
            foreach (DestructibleObject _do in mDestructibles)
            {
                _do.AddToSpace(mSpace, DestructiblesCG);
                _do.AddDrawables(SCREEN);
                _do.AddFragments(mFragments);
            }
        }
        public void Add(GenericPlane _plane)
        {
            mPlane = _plane;
        }
        public void Add(List<SpawnPoint> _spawns)
        {
            mSpawns = _spawns;
        }
        public void Add(List<Fragment> _frags)
        {
            //mFragments = new List<Fragment>();
            mFragments = _frags;

            foreach (Fragment _f in mFragments)
            {
                mSpace.Add(_f.Entity);
                SCREEN.Add(_f);
            }
        }
        public void Add(Player _one, Player _two)
        {
            mPlayerONE = _one;
            mPlayerTWO = _two;

            mPlayerONE.Moved += mPlayerTWO.EMhandler;
            mPlayerTWO.Moved += mPlayerONE.EMhandler;

            mDisposeHandler.SetGoToLists(mPlayerONE.GoToShots, mPlayerTWO.GoToShots);
        }

        #endregion

        public void Update(GameTime _GT)
        {
            mSpace.Update();
            mDisposeHandler.Update(_GT);

            mPlayerONE.Update(_GT, Fragments);
            mPlayerTWO.Update(_GT, Fragments);

            foreach (Trap _t in mTraps)
            {
                _t.Update(_GT, mPlayerONE.Entity);
            }
        }

        public void PlaceTrap(Trap _trap)
        {
            //mTraps.Add(_trap);
        }

        public void Remove(Fragment _c)
        {
            SCREEN.Remove(_c);
        }

    }
}
