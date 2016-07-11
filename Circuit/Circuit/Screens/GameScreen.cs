using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Circuit.Utilities.Materials;
using Circuit.Utilities;
using Circuit.Utilities.Cameras;

namespace Circuit.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        CameraHandler mCamHandler;
        List<DrawableEntity3D> COMPONENTS_3D = new List<DrawableEntity3D>();
        List<DrawableEntity2D> COMPONENTS_2D = new List<DrawableEntity2D>();
        protected bool mLoaded;

        public NormalMapMaterial NormalMapMat { get; set; }

        protected CameraHandler CamHandler
        {
            get { return mCamHandler; }
            set { mCamHandler = value; }
        }

        protected List<DrawableEntity3D> Components3D
        {
            get { return COMPONENTS_3D; }
        }

        protected List<DrawableEntity2D> Components2D
        {
            get { return COMPONENTS_2D; }
        }

        public GameScreen(Game GAME)
            : base(GAME)
        {
        }

        public override void Initialize()
        {
            NormalMapMat = new NormalMapMaterial(Game.Content.Load<Texture2D>("Arenas/NeutralArena/brick_normal_map"));
            NormalMapMat.LightColor = Color.White.ToVector3();
            //NormalMapMaterial.LightDirection = new Vector3(0.5f, 0.5f, 0.5f);

            base.Initialize();


        }

        public override void Update(GameTime GT)
        {
            base.Update(GT);
        }

        public override void Draw(GameTime _GT)
        {
            //SetupGraphicsDevice();
            base.Draw(_GT);

            if (mCamHandler != null)
                mCamHandler.Draw(_GT, COMPONENTS_3D);

            foreach (DrawableGameComponent _c in Components2D)
                _c.Draw(_GT);

            //foreach (GameComponent component in COMPONENTS_3D)
            //    if (component is DrawableGameComponent &&
            //    ((DrawableGameComponent)component).Visible)
            //    {
            //        ((DrawableGameComponent)component).Draw(_GT);
            //    }
        }

        protected void SetupGraphicsDevice()
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }

        public void Add(DrawableEntity3D _comp)
        {
            this.Components3D.Add(_comp);
            _comp.Initialize();
        }

        public void Add(DrawableEntity2D _comp)
        {
            this.Components2D.Add(_comp);
            _comp.Initialize();
        }

        public void Remove(DrawableEntity3D _comp)
        {
            this.Components3D.Remove(_comp);
        }


        public virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;

            this.Initialize();
            /*
            foreach (GameComponent _c in COMPONENTS)
            {
                _c.Enabled = true;
                if (_c is DrawableGameComponent)
                    ((DrawableGameComponent)_c).Visible = true;
            }
            */
        }

        public virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;

            COMPONENTS_3D.Clear();
            COMPONENTS_2D.Clear();
        }
    }
}
