using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities;
using Circuit.Utilities.Materials;
using Circuit.Utilities.Cameras;

namespace Circuit.Utilities
{
    /// <summary>
    /// Component that draws a model following the position and orientation of a BEPUphysics entity.
    /// </summary>
    public class EntityModel : DrawableGameComponent, DrawableEntity3D
    {
        /// <summary>
        /// Entity that this model follows.
        /// </summary>
        Entity mEntity;
        Model mModel;

        public Effect mGlowEffect;

        public Vector3 mGlowColor1 = Data.PG_STARTLIFE;
        public Vector3 mGlowColor2 = Data.PG_DEFAULT_BOOST;
        public bool IS_PLAYER = false;

        private Texture2D glowTex;
        public Texture2D GlowTex
        {
            get { return glowTex; }

            set
            {
                glowTex = value;
                mGlowEffect.Parameters["GlowTexture"].SetValue(glowTex);
            }
        }

        protected Vector3 Color;
        bool co;

        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;

        private Vector3 mModelScale = new Vector3(1.0f);
        private float mEntityScale = 1;


        private BoundingSphere mBoundingSphere;

        public Vector3 Position
        {
            get { return mEntity.Position; }
            set { mEntity.Position = value; }
        }
        public EntityModel Model
        {
            get { return this; }
        }
        public Entity Entity
        {
            get { return mEntity; }
            set { mEntity = value; }
        }
        public Vector3 ModelScaling
        {
            get { return mModelScale; }
            set { mModelScale = value; }
        }
        public float EntityScaling
        {
            get { return mEntityScale; }
            set { mEntityScale = value; }

        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(ModelScaling)
                    * Matrix.CreateTranslation(Position);

                BoundingSphere transformed = mBoundingSphere;
                transformed = transformed.Transform(worldTransform);

                return transformed;
            }
        }

        /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="_model">Graphical representation to use for the entity.</param>
        /// <param name="_transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="_game">Game to which this component will belong.</param>
        public EntityModel(Entity entity, Model model, Game game, Texture2D GlowTexture)
            : base(game)
        {
            this.mEntity = entity;
            this.mModel = model;

            Transform = Matrix.Identity;
            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            mModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            BuildBoundingSphere();
            GenerateTags();

            mGlowEffect = Game.Content.Load<Effect>("Effects/GlowEffect");
            GlowTex = GlowTexture;

        }

        public void Draw(GameTime gameTime, Camera _cam)
        {

            if (_cam.BoundingVolumeIsInView(this.BoundingSphere))
            {
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;


                //Notice that the entity's worldTransform property is being accessed here.
                //This property is returns a rigid transformation representing the orientation
                //and translation of the entity combined.
                //There are a variety of properties available in the entity, try looking around
                //in the list to familiarize yourself with it.
                Matrix worldMatrix;
                worldMatrix = Matrix.CreateScale(ModelScaling) * Transform * mEntity.WorldTransform;

                foreach (ModelMesh mesh in mModel.Meshes)
                {
                    Matrix localWorld = boneTransforms[mesh.ParentBone.Index]
                        * worldMatrix;

                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        Effect effect = meshPart.Effect;

                        if (effect is BasicEffect)
                        {
                            ((BasicEffect)effect).World = localWorld;
                            ((BasicEffect)effect).View = _cam.View;
                            ((BasicEffect)effect).Projection = _cam.Projection;
                            ((BasicEffect)effect).EnableDefaultLighting();

                            // Coloring was for debug purposes, may get deleted
                            //if(co) (effect as BasicEffect).AmbientLightColor = Color;
                        }
                        else
                        {
                            SetEffectParameter(effect, "World", localWorld);
                            SetEffectParameter(effect, "View", _cam.View);
                            SetEffectParameter(effect, "Projection", _cam.Projection);
                            SetEffectParameter(effect, "CameraPosition", Vector3.Zero);

                            ((MeshTag)meshPart.Tag).Material.SetEffectParameters(effect);
                        }
                    }

                    mesh.Draw();
                }

                base.Draw(gameTime);
            }
        }

        public void Coloring(Vector3 _color)
        {
            Color = _color;
            co = true;
        }

        private void BuildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                    boneTransforms[mesh.ParentBone.Index]);

                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }

            this.mBoundingSphere = sphere;
        }

        public void SetModelMaterial(Material material)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                SetMeshMaterial(mesh.Name, material);
        }

        // Sets the specified effect parameter to the given effect, if it
        // has that parameter
        void SetEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                SetMeshEffect(mesh.Name, effect, CopyEffect);
        }

        public void SetMeshEffect(string MeshName, Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                if (mesh.Name != MeshName)
                    continue;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;
                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();
                    MeshTag tag = ((MeshTag)part.Tag);
                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        SetEffectParameter(toSet, "BasicTexture", tag.Texture);
                        SetEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        SetEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    SetEffectParameter(toSet, "DiffuseColor", tag.Color);
                    SetEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                }
            }
        }

        public void SetMeshMaterial(string MeshName, Material material)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                if (mesh.Name != MeshName)
                    continue;
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    ((MeshTag)meshPart.Tag).Material = material;
            }
        }

        private void GenerateTags()
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor,
                            effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
        }

        // Store references to all of the model's current effects
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        // Restore the effects referenced by the model's cache
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }
    }


    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;
        public Material Material = new Material();

        public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }
}