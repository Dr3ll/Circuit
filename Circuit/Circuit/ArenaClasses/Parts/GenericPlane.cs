using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities.Cameras;

namespace Circuit.ArenaClasses
{
    class GenericPlane
    {
        private BasicEffect effect = null;
        public BasicEffect Effect
        {
            get { return effect; }
        }

        private VertexBuffer vertexBuffer = null;
        private GraphicsDevice graphics = null;

        /// <summary>
        /// Generates a new plane
        /// </summary>
        /// <param name="graphics">reference to game's graphics device</param>
        /// <param name="texture">reference to a texture to use for this groundplane (use Content.Load)</param>
        /// <param name="size">size of the plane in x and z direction</param>
        /// <param name="textureRepeatFactor">how often the texture repeats over the ground per unit</param>
        public GenericPlane(GraphicsDevice graphics, Texture2D texture, float size, float textureRepeatFactor = 0.25f) : base()
        {
            this.graphics = graphics;

            // create a basic effect
            effect = new BasicEffect(graphics);
            effect.World = Matrix.Identity;
            effect.LightingEnabled = false;
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = false;
            effect.Texture = texture;

            // primitive color
            effect.AmbientLightColor = new Vector3(0.6f, 0.6f, 0.6f);
            effect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 5.0f;
            effect.Alpha = 1.0f;

            // create vertices
            float tex = textureRepeatFactor * size * 2;
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(-size, 0, -size);
            vertices[0].TextureCoordinate = new Vector2(0.0f, 0.0f);
            vertices[1].Position = new Vector3(size, 0, -size);
            vertices[1].TextureCoordinate = new Vector2(tex, 0.0f);
            vertices[2].Position = new Vector3(-size, 0, size);
            vertices[2].TextureCoordinate = new Vector2(0.0f, tex);
            vertices[3].Position = new Vector3(size, 0, size);
            vertices[3].TextureCoordinate = new Vector2(tex, tex);
            //vertices[0].Normal = vertices[1].Normal = vertices[2].Normal = vertices[3].Normal = new Vector3(0, 1, 0);
          
            // create vertexbuffer
            vertexBuffer = new VertexBuffer(graphics, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        /// <summary>
        /// draws the plane using a specified camera
        /// </summary>
        /// <param name="usedCamera">camera in that space the plane will be drawn</param>
        public void Draw(Camera usedCamera)
        {
            Draw(usedCamera.View, usedCamera.Projection);
        }

        /// <summary>
        /// sets/uses the given view and projection matrices to draw the object
        /// </summary>
        /// <param name="viewMatrix">view matrix used for the drawing</param>
        /// <param name="projectionMatrix">projection matrix applied to the object drawing</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            // use matrices
            Effect.View = viewMatrix;
            Effect.Projection = projectionMatrix;

            // set renderstate
            RasterizerState rasterizerState1 = new RasterizerState();
            rasterizerState1.CullMode = CullMode.None;
            graphics.RasterizerState = rasterizerState1;

            // set vertexbuffer
            graphics.SetVertexBuffer(vertexBuffer);

            // render every pass of the 
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }
    }
}
