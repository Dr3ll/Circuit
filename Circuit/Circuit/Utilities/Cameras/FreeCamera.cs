using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circuit.Utilities.Cameras
{
    public class FreeCamera : Camera 
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }

        private Vector3 translation;

        public FreeCamera(Vector3 _position, float _yaw, float _pitch, float _aspectRatio,
            GraphicsDevice _graphics)
            : base(_aspectRatio, _graphics)
        {
            this.Position = _position;
            this.Yaw = _yaw;
            this.Pitch = _pitch;

            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        public override void Update(GameTime _GT)
        {
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
