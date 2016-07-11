using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.ArenaClasses
{
    class Collider : EntityModel
    {
        public Collider(Vector3 _position, Vector3 _scale, Vector3 _orientation, Game _GAME, Texture2D GlowTex)
            : base(new BEPUphysics.Entities.Prefabs.Box(_position, _scale.X, _scale.Y, _scale.Z), _GAME.Content.Load<Model>("cubef"), _GAME, GlowTex)
        {
            ModelScaling = _scale;
            Transform = Matrix.CreateFromYawPitchRoll(_orientation.X, _orientation.Y, _orientation.Z);
           
        }
    }
}
