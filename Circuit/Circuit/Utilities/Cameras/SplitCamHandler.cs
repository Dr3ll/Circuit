using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Circuit.Utilities.Cameras
{
    class SplitCamHandler : CameraHandler
    {
        Camera mCam1;
        Camera mCam2;
        Viewport mDefaultViewport;
        Viewport mViewport1;
        Viewport mViewport2;
        GraphicsDevice mGraphics;
        Game mGame;
        SpriteBatch mSpriteBatch;

        RenderCapture mRenderCapture;
        RenderCapture mGlowCapture;

        GaussianBlur mBlur;

        public SplitCamHandler(Camera _cam1, Camera _cam2, GraphicsDevice _graphics, Game _Game)
        {
            mCam1 = _cam1;
            mCam2 = _cam2;

            mGame = _Game;
            mSpriteBatch = new SpriteBatch(_graphics);

            mGraphics = _graphics;
            mDefaultViewport = new Viewport(0, 0, Data.R_VIEWPORTWIDTH, Data.R_VIEWPORTHEIGHT);
            mViewport1 = mDefaultViewport;
            mViewport2 = mViewport1;
            mViewport1.Width = Data.R_VIEWPORTWIDTH;
            mViewport2.Width = Data.R_VIEWPORTWIDTH;
            mViewport1.Height = (int)(Data.R_VIEWPORTHEIGHT * .5f);
            mViewport2.Height = (int)(Data.R_VIEWPORTHEIGHT * .5f);
            mViewport2.Y = (int)(Data.R_VIEWPORTHEIGHT * .5f);

            mRenderCapture = new RenderCapture(_graphics);
            mGlowCapture = new RenderCapture(_graphics);

            mBlur = new GaussianBlur(_graphics, _Game.Content, Data.GLOW_BLUR_AMOUNT);
        }

        public void Draw(GameTime _GT, List<DrawableEntity3D> _comps)
        {
            mGraphics.Viewport = mDefaultViewport;
            mGraphics.Clear(Color.Black);

            mGlowCapture.Begin();

            mGraphics.Viewport = mViewport1;
            mGraphics.Clear(Color.Black);
            foreach (DrawableEntity3D _e in _comps)
            {
                //if (_e is Circuit.ArenaClasses.Skybox)
                //    (_e as Circuit.ArenaClasses.Skybox).Position = mCam1.Position;

                if (_e is Circuit.ArenaClasses.Skybox) continue;

                if (!(_e is Circuit.ArenaClasses.Skybox) && !(_e is Circuit.Utilities.StaticModel) && !((EntityModel)_e).IS_PLAYER)
                {
                    ((EntityModel)_e).CacheEffects();
                    ((EntityModel)_e).SetModelEffect(((EntityModel)_e).mGlowEffect, false);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((EntityModel)_e).GlowTex);
                    _e.Draw(_GT, mCam1);
                    ((EntityModel)_e).RestoreEffects();
                }

                if (!(_e is Circuit.Utilities.StaticModel) && ((EntityModel)_e).IS_PLAYER)
                {
                    ((EntityModel)_e).CacheEffects();
                    ((EntityModel)_e).SetModelEffect(((EntityModel)_e).mGlowEffect, false);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((EntityModel)_e).GlowTex);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowColor1"].SetValue(
                                                      ((EntityModel)_e).mGlowColor1);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowColor2"].SetValue(
                                                      ((EntityModel)_e).mGlowColor2);
                    _e.Draw(_GT, mCam1);
                    ((EntityModel)_e).RestoreEffects();
                }

                if (_e is Circuit.Utilities.StaticModel)
                {
                    ((Circuit.Utilities.StaticModel)_e).CacheEffects();
                    ((Circuit.Utilities.StaticModel)_e).SetModelEffect(((Circuit.Utilities.StaticModel)_e).mGlowEffect, false);
                    ((Circuit.Utilities.StaticModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((Circuit.Utilities.StaticModel)_e).GlowTex);
                    _e.Draw(_GT, mCam1);
                    ((Circuit.Utilities.StaticModel)_e).RestoreEffects();
                }
            }

            mGraphics.Viewport = mViewport2;
            //mGraphics.Clear(Color.Black);

            foreach (DrawableEntity3D _e in _comps)
            {
                //if (_e is Circuit.ArenaClasses.Skybox)
                //    (_e as Circuit.ArenaClasses.Skybox).Position = mCam2.Position;

                if (_e is Circuit.ArenaClasses.Skybox) continue;

                if (!(_e is Circuit.ArenaClasses.Skybox) && !(_e is Circuit.Utilities.StaticModel) && !((EntityModel)_e).IS_PLAYER)
                {
                    ((EntityModel)_e).CacheEffects();
                    ((EntityModel)_e).SetModelEffect(((EntityModel)_e).mGlowEffect, false);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((EntityModel)_e).GlowTex);
                    _e.Draw(_GT, mCam2);
                    ((EntityModel)_e).RestoreEffects();
                }

                if (!(_e is Circuit.Utilities.StaticModel) && ((EntityModel)_e).IS_PLAYER)
                {
                    ((EntityModel)_e).CacheEffects();
                    ((EntityModel)_e).SetModelEffect(((EntityModel)_e).mGlowEffect, false);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((EntityModel)_e).GlowTex);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowColor1"].SetValue(
                                                      ((EntityModel)_e).mGlowColor1);
                    ((EntityModel)_e).mGlowEffect.Parameters["GlowColor2"].SetValue(
                                                      ((EntityModel)_e).mGlowColor2);
                    _e.Draw(_GT, mCam2);
                    ((EntityModel)_e).RestoreEffects();
                }

                if (_e is Circuit.Utilities.StaticModel)
                {
                    ((Circuit.Utilities.StaticModel)_e).CacheEffects();
                    ((Circuit.Utilities.StaticModel)_e).SetModelEffect(((Circuit.Utilities.StaticModel)_e).mGlowEffect, false);
                    ((Circuit.Utilities.StaticModel)_e).mGlowEffect.Parameters["GlowTexture"].SetValue(
                                                       ((Circuit.Utilities.StaticModel)_e).GlowTex);
                    _e.Draw(_GT, mCam2);
                    ((Circuit.Utilities.StaticModel)_e).RestoreEffects();
                }

            }

            mGlowCapture.End();

            mGraphics.Clear(Color.Black);

            mRenderCapture.Begin();

            //mGraphics.Clear(Color.Black);

            mGraphics.Viewport = mViewport1;
            //mGraphics.Clear(Color.Black);

            foreach (DrawableEntity3D _e in _comps)
            {
                if (_e is Circuit.ArenaClasses.Skybox)
                    (_e as Circuit.ArenaClasses.Skybox).Position = mCam1.Position;

                _e.Draw(_GT, mCam1);
            }

            mGraphics.Viewport = mViewport2;
            //mGraphics.Clear(Color.Black);

            foreach (DrawableEntity3D _e in _comps)
            {
                if (_e is Circuit.ArenaClasses.Skybox)
                    (_e as Circuit.ArenaClasses.Skybox).Position = mCam2.Position;

                _e.Draw(_GT, mCam2);

            }

            mRenderCapture.End();


            // Blur the glow render back into the glow RenderCapture
            mBlur.Input = mGlowCapture.GetTexture();
            mBlur.ResultCapture = mGlowCapture;
            mBlur.Draw();

            mGraphics.Viewport = mDefaultViewport;
            mGraphics.Clear(Color.Black);

            // Draw the blurred glow render over the normal render additively
            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            mSpriteBatch.Draw(mRenderCapture.GetTexture(), Vector2.Zero, Color.White);
            if (Keyboard.GetState().IsKeyUp(Keys.G))
            {
                mSpriteBatch.Draw(mGlowCapture.GetTexture(), Vector2.Zero, Color.White);
            }
            mSpriteBatch.End();

            mGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            mGame.GraphicsDevice.BlendState = BlendState.Opaque;

        }
    }
}
