using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Circuit.Utilities.Cameras;
using Circuit.Utilities;

namespace Circuit.ArenaClasses
{
    public class Skybox : Microsoft.Xna.Framework.DrawableGameComponent, DrawableEntity3D
    {
        // containers
        private Texture2D[] skyboxTextures;
        private VertexBuffer skyboxBuffer;
        private BasicEffect effect;

        // const skybox size, may be changed if convenient
        private const float size = 420.0f;

        // sampler states for texture clamping
        private SamplerState clampState;
        private SamplerState backupState;

        private Vector3 mPosition;

        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        // constructor
        public Skybox(Game GAME, string _path) 
            : base(GAME)
        {
            string textureName = _path + "/Skybox";

            // load Skybox textures
            skyboxTextures = new Texture2D[6];
            skyboxTextures[0] = Game.Content.Load<Texture2D>(textureName + "_TOP");
            skyboxTextures[1] = Game.Content.Load<Texture2D>(textureName + "_BOTTOM");
            skyboxTextures[2] = Game.Content.Load<Texture2D>(textureName + "_LEFT");
            skyboxTextures[3] = Game.Content.Load<Texture2D>(textureName + "_RIGHT");
            skyboxTextures[4] = Game.Content.Load<Texture2D>(textureName + "_FRONT");
            skyboxTextures[5] = Game.Content.Load<Texture2D>(textureName + "_BACK");
        }

        public override void Initialize()
        {
            base.Initialize();

            // create basic effect
            effect = new BasicEffect(GraphicsDevice);

            // initailize effect
            effect.World = Matrix.CreateTranslation(mPosition);
            effect.LightingEnabled = false;
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = false;

            effect.AmbientLightColor = new Vector3(0.6f, 0.6f, 0.6f);
            effect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 5.0f;
            effect.Alpha = 1.0f;

            // define skybox vertices
            Vector3[] vertices = new Vector3[8];
            vertices[0] = new Vector3(-size, -size, -size);
            vertices[1] = new Vector3(-size, -size, size);
            vertices[2] = new Vector3(size, -size, -size);
            vertices[3] = new Vector3(size, -size, size);
            vertices[4] = new Vector3(-size, size, -size);
            vertices[5] = new Vector3(-size, size, size);
            vertices[6] = new Vector3(size, size, -size);
            vertices[7] = new Vector3(size, size, size);

            // create SamplerState that clamps textures at borders
            clampState = new SamplerState();
            clampState.AddressU = TextureAddressMode.Clamp;
            clampState.AddressV = TextureAddressMode.Clamp;

            // define skybox faces as triangle fans
            // TODO: if texture winding or orientation is wrong, adjust vertex order or texture coordinates here
            VertexPositionTexture[] skyboxModel = new VertexPositionTexture[24];
            // Top
            skyboxModel[0] = new VertexPositionTexture(vertices[4], new Vector2(0.0f, 0.0f));
            skyboxModel[1] = new VertexPositionTexture(vertices[6], new Vector2(0.0f, 1.0f));
            skyboxModel[2] = new VertexPositionTexture(vertices[5], new Vector2(1.0f, 0.0f));
            skyboxModel[3] = new VertexPositionTexture(vertices[7], new Vector2(1.0f, 1.0f));
            // Bottom
            skyboxModel[4] = new VertexPositionTexture(vertices[2], new Vector2(0.0f, 0.0f));
            skyboxModel[5] = new VertexPositionTexture(vertices[0], new Vector2(0.0f, 1.0f));
            skyboxModel[6] = new VertexPositionTexture(vertices[3], new Vector2(1.0f, 0.0f));
            skyboxModel[7] = new VertexPositionTexture(vertices[1], new Vector2(1.0f, 1.0f));
            // Left
            skyboxModel[8] = new VertexPositionTexture(vertices[4], new Vector2(0.0f, 0.0f));
            skyboxModel[9] = new VertexPositionTexture(vertices[0], new Vector2(0.0f, 1.0f));
            skyboxModel[10] = new VertexPositionTexture(vertices[6], new Vector2(1.0f, 0.0f));
            skyboxModel[11] = new VertexPositionTexture(vertices[2], new Vector2(1.0f, 1.0f));
            // Right
            skyboxModel[12] = new VertexPositionTexture(vertices[7], new Vector2(0.0f, 0.0f));
            skyboxModel[13] = new VertexPositionTexture(vertices[3], new Vector2(0.0f, 1.0f));
            skyboxModel[14] = new VertexPositionTexture(vertices[5], new Vector2(1.0f, 0.0f));
            skyboxModel[15] = new VertexPositionTexture(vertices[1], new Vector2(1.0f, 1.0f));
            // Front
            skyboxModel[16] = new VertexPositionTexture(vertices[6], new Vector2(0.0f, 0.0f));
            skyboxModel[17] = new VertexPositionTexture(vertices[2], new Vector2(0.0f, 1.0f));
            skyboxModel[18] = new VertexPositionTexture(vertices[7], new Vector2(1.0f, 0.0f));
            skyboxModel[19] = new VertexPositionTexture(vertices[3], new Vector2(1.0f, 1.0f));
            // Back
            skyboxModel[20] = new VertexPositionTexture(vertices[5], new Vector2(0.0f, 0.0f));
            skyboxModel[21] = new VertexPositionTexture(vertices[1], new Vector2(0.0f, 1.0f));
            skyboxModel[22] = new VertexPositionTexture(vertices[4], new Vector2(1.0f, 0.0f));
            skyboxModel[23] = new VertexPositionTexture(vertices[0], new Vector2(1.0f, 1.0f));

            // create vertexbuffer
            skyboxBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            skyboxBuffer.SetData(skyboxModel);
        }

        public void Draw(GameTime GT, Camera _cam)
        {
           
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            Draw(GraphicsDevice,
                _cam.View,
                _cam.Projection,
                Matrix.CreateTranslation(Vector3.Zero));
        }

        private void Draw(GraphicsDevice graphics, Matrix viewMatrix, Matrix projectionMatrix, Matrix WorldMatrix)
        {
            // disable depth buffer
            graphics.DepthStencilState = DepthStencilState.None;

            // clamp texture
            backupState = graphics.SamplerStates[0];
            graphics.SamplerStates[0] = clampState;

            // set vertexbuffer
            graphics.SetVertexBuffer(skyboxBuffer);

            // world matrix should contain camera translation, not orientation
            // this results in the skybox moving with the viewer e.g. being "infinitely far away"
            effect.World = Matrix.CreateTranslation(mPosition);
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            // render skybox
            effect.Texture = skyboxTextures[0];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
            effect.Texture = skyboxTextures[1];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 4, 2);
            }
             effect.Texture = skyboxTextures[2];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 8, 2);
             }
             effect.Texture = skyboxTextures[3];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 12, 2);
             }
             effect.Texture = skyboxTextures[4];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 16, 2);
             }
             effect.Texture = skyboxTextures[5];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 20, 2);
             }

            // return to default
             graphics.SamplerStates[0] = backupState;

            // enable depth buffer again
            graphics.DepthStencilState = DepthStencilState.Default;
        }
    }
}
