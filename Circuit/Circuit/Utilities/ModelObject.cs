using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities.Cameras;


namespace Circuit.Utilities
{
    /// <summary>
    /// Easy to use object with model from xna contentloader
    /// </summary>
    class ModelObject
    {
        public Model model;

        protected Color color;

        internal bool selected;

        /// <summary>
        /// creates a modelobject by loading a specified model from the content manager
        /// </summary>
        /// <param name="contentManager">contentmanager that will load the specified model</param>
        /// <param name="modelName">name of the model - no fileending!</param>
        public ModelObject(ContentManager contentManager, string modelName)
            : this(contentManager.Load<Model>(modelName))
        {

        }

        public Vector3 Scaling
        { get { return scaling; } set { scaling = value; } }
        private Vector3 scaling = new Vector3(1.0f);

        /// <summary>
        /// creates a modelobject by using a already loaded model
        /// </summary>
        /// <param name="model">instance of an already loaded model</param>
        public ModelObject(Model model)
        {
            this.model = model;

            // some settings to the material
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    BasicEffect basicEffect = (BasicEffect)effect;
                    basicEffect.AmbientLightColor = new Vector3(.5f, 0.5f, 0.5f);
                    basicEffect.DirectionalLight0.Enabled = true;
                    basicEffect.DirectionalLight1.Enabled = false;
                    basicEffect.DirectionalLight2.Enabled = false;
                    basicEffect.LightingEnabled = true;
                    basicEffect.PreferPerPixelLighting = true;
                }
            }
        }

        /// <summary>
        /// draws the object using a specified camera
        /// </summary>
        /// <param name="CAM">camera in that space the object will be drawn</param>
        public void Draw(Camera CAM)
        {
            Draw(CAM.View, CAM.Projection);
        }

        /// <summary>
        /// sets/uses the given view and projection matrices to draw the object
        /// </summary>
        /// <param name="_viewMatrix">view matrix used for the drawing</param>
        /// <param name="_projectionMatrix">projection matrix applied to the object drawing</param>
        public void Draw(Matrix _viewMatrix, Matrix _projectionMatrix)
        {

            Matrix worldMatrix;

            worldMatrix = Matrix.CreateScale(Scaling) * Matrix.CreateTranslation(Vector3.Zero);
            

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect basicEffect = (BasicEffect)effect;
                        basicEffect.World = worldMatrix;
                        basicEffect.View = _viewMatrix;
                        basicEffect.Projection = _projectionMatrix;
                    }
                    else
                    {
                        // unknown effect!
                    }
                }
                mesh.Draw();
            }
        }
    }
}
