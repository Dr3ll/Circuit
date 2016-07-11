using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.S_Action.PlayerClasses
{
    struct Offset
    {
        public Vector3 mBaseRotation;
        public Vector3 mRotation;
        public Vector3 mRadius;

        public Offset(Vector3 _baseRotation, float _radius, float _height)
        {
            mBaseRotation = _baseRotation;
            mBaseRotation.Y = _height;
            mRadius = new Vector3(0, _radius, 0);
            mRotation = Vector3.Zero;
        }

        public Offset(Vector3 _baseRotation, float _dist,  float _radius, float _height)
        {
            mBaseRotation = _baseRotation * _dist;
            mBaseRotation.Y = _height;
            mRadius = new Vector3(0, _radius, 0);
            mRotation = Vector3.Zero;
        }

        public void Update(Vector3 _rot)
        {
            mRotation = _rot;
        }

        public static Vector3 CalcRotFromPolar(float _polar)
        {
            float tPolar = _polar - 2.08f;
            return new Vector3((float)Math.Sin(tPolar), 0, (float)Math.Cos(tPolar));
        }

        public Vector3 RelPosition()
        {
            Vector3 tRot = mRotation + mRadius;
            return Vector3.Transform(mBaseRotation, Matrix.CreateFromYawPitchRoll(tRot.Y, 0, tRot.Z));
        }

    }
}
