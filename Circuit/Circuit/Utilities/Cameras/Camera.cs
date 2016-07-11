using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.Utilities.Cameras
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;
        public Vector3 Position { get; protected set; }

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                GenerateFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                GenerateFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }
        
        public Camera(float _aspectRatio, GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;

            GeneratePerspectiveProjectionMatrix(_aspectRatio, MathHelper.PiOver4);
        }

        private void GeneratePerspectiveProjectionMatrix(float _aspectRatio, float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), _aspectRatio, 0.1f, 1000000.0f);
        }

        public virtual void Update(GameTime _GT)
        {
        }

        private void GenerateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
