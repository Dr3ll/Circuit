using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    abstract class Fragment : EntityModel
    {
        private enum FragState { ON_GROUND, IN_DO, TO_AWAY, IN_CLOUD, TO_CLOUD, DEFAULT }

        public Data.ProgrammType Type;
        public event EventHandler Used;

        private FragState STATE = FragState.DEFAULT;
        protected const float BASESIZE = .5f;
        protected const float BASEMASS = 1;
        RotationData mRotData;
        float mAwayPercenage;
        Vector3 mRelPos;

        public bool Detached
        {
            get { return STATE == FragState.TO_AWAY; }
        }
        public bool Picked
        {
            get { return STATE != FragState.ON_GROUND; }
        }
        public void SetupRotation(float _rand)
        {
            //Scaling = new Vector3(Data.FC_BASE_FRAGSCALE
            //        * _rand + Data.FC_BASE_FRAGSCALE);

            mRotData = new RotationData(_rand);
        }

        public abstract void GlowLight();
        public abstract void GlowDark();

        public Fragment(BEPUphysics.Entities.Entity _entity, Model _model, Game _GAME, Texture2D GlowTex)
            : base(_entity, _model, _GAME, GlowTex)
        {
            ModelScaling = new Vector3(BASESIZE);
            STATE = FragState.IN_DO;
            _entity.Tag = "frag";
        }

        public Fragment CheckPickup()
        {
            if (STATE == FragState.ON_GROUND)
            {
                return this;
            }

            return null;
        }

        public void PickUp()
        {
            STATE = FragState.TO_CLOUD;
        }

        public void Detach()
        {
            STATE = FragState.TO_AWAY;
        }

        public void MakeCollectable()
        {
            STATE = FragState.ON_GROUND;
        }

        public void Update(Vector3 _cpos, GameTime _GT, Vector3 _awayPos)
        {
            if (STATE == FragState.DEFAULT && STATE == FragState.ON_GROUND)
                throw new Exception();

            switch (STATE)
            {
                // Fragment is in a FragmentCloud and therefore is rotating 
                case FragState.IN_CLOUD:
                    mRelPos = mRotData.GetRelPosition(_GT);
                    this.Position = _cpos + mRelPos;
                    this.Transform = mRotData.GetOrientation(_GT);
                    break;
            
                // Fragment is flying 'into' the shot that is beeing empowered
                case FragState.TO_AWAY:
                    Vector3 tAway = (mRelPos - _awayPos);
                    tAway.Normalize();
                    tAway *= -Data.FC_AWAYRANGE;
                    mAwayPercenage += _GT.ElapsedGameTime.Milliseconds * Data.FC_AWAYSPEED;
                    this.Position = _cpos + mAwayPercenage * tAway;

                    if (mAwayPercenage >= Data.FC_AWAYPERCENTAGE)
                        if (Used != null)
                            Used(this, EventArgs.Empty);
                    break;

                // Fragment got picked up and is flying to the cloud
                case FragState.TO_CLOUD:
                    Vector3 tToCloud = _cpos - this.Position;

                    if (tToCloud.LengthSquared() <= Data.FC_TOCLOUDSPEED)
                        STATE = FragState.IN_CLOUD;
                    else
                    {
                        tToCloud.Normalize();
                        tToCloud *= Data.FC_TOCLOUDSPEED;
                        this.Position += tToCloud;
                    }
                    break;
            }
        }
    }

    public class RotationData
    {
        float   mHeight = 0;
        float   mHMult = 1;
        Vector2 mRotator  = Vector2.Zero;
        Vector3 mOrientor = Vector3.Zero;
        int     mRand;
        float   mRotSpeed;
        float   mYSpeed;
        float   mYSpan;
        float   mRadius;

        public RotationData(float _rand)
        {
            mRand = (int)(_rand * 200);

            mRotSpeed = Data.FC_BASE_ROTATIONSPEED * _rand + Data.FC_BASE_ROTATIONSPEED * Data.FC_BASE_MINMANIPULATOR;
            mYSpeed = Data.FC_BASE_YSPEED * (1 - _rand) + Data.FC_BASE_YSPEED * Data.FC_BASE_MINMANIPULATOR;
            mYSpan = Data.FC_BASE_YSPAN * (1 - _rand) + Data.FC_BASE_YSPAN * Data.FC_BASE_MINMANIPULATOR;
            mRadius = Data.FC_BASE_RADIUS * (1 - _rand) + Data.FC_BASE_RADIUS * Data.FC_BASE_MINMANIPULATOR;

        }

        public Vector3 GetRelPosition(GameTime _GT)
        {
            mRotator.X += mRotSpeed * _GT.ElapsedGameTime.Milliseconds;
            mRotator.Y += mRotSpeed * _GT.ElapsedGameTime.Milliseconds;

            mHeight += mHMult * (_GT.ElapsedGameTime.Milliseconds) * .0167f * mYSpan * mYSpeed;
            if (mHeight > mYSpan || mHeight < 0)
                mHMult *= -1;

            return new Vector3(
                       (float)System.Math.Sin(mRotator.X + mRand) * mRadius,
                       mHeight,
                       (float)System.Math.Cos(mRotator.Y + mRand) * mRadius);
        }

        public Matrix GetOrientation(GameTime _GT)
        {
            mOrientor.X += mRotSpeed * _GT.ElapsedGameTime.Milliseconds;
            mOrientor.Y += mRotSpeed * _GT.ElapsedGameTime.Milliseconds;
            mOrientor.Z += mRotSpeed * _GT.ElapsedGameTime.Milliseconds;
            return Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(mOrientor.X + mRand, mOrientor.Y + mRand, mOrientor.Z + mRand));
        }
    }
}