using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Circuit.Utilities.Materials;
using Circuit.Utilities.Cameras;

namespace Circuit.Utilities
{
    /// <summary>
    /// Component that draws a model.
    /// </summary>
    public class StaticModel : DrawableGameComponent, DrawableEntity3D
    {
        Model mModel;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;

        public Effect mGlowEffect;

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

        /// <summary>
        /// Creates a new StaticModel.
        /// </summary>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public StaticModel(Model model, Matrix transform, Game game)
            : base(game)
        {
            this.mModel = model;
            this.Transform = transform;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            mModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            generateTags();

            mGlowEffect = Game.Content.Load<Effect>("Effects/GlowEffect");
            this.GlowTex = GlowTex;
        }

        public void Draw(GameTime gameTime, Camera _cam)
        {
            
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            

            Matrix worldMatrix;
            worldMatrix = Transform;

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
                    }
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", _cam.View);
                        setEffectParameter(effect, "Projection", _cam.Projection);
                        setEffectParameter(effect, "CameraPosition", Vector3.Zero);

                        ((MeshTag)meshPart.Tag).Material.SetEffectParameters(effect);
                    }
                }

                mesh.Draw();
            }
        }

        // Sets the specified effect parameter to the given effect, if it
        // has that parameter
        void setEffectParameter(Effect effect, string paramName, object val)
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
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        setEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                }
            }
        }

        public void SetModelMaterial(Material material)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
                SetMeshMaterial(mesh.Name, material);
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

        private void generateTags()
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
}
