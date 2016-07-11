using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Circuit.Utilities
{
    class RenderCapture
    {
        RenderTarget2D renderTarget;
        GraphicsDevice graphicsDevice;

        public RenderCapture(GraphicsDevice GraphicsDevice)
        {
            this.graphicsDevice = GraphicsDevice;
            renderTarget = new RenderTarget2D(GraphicsDevice,
            GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
            false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        // Begins capturing from the graphics device
        public void Begin()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
        }

        // Stop capturing
        public void End()
        {
            graphicsDevice.SetRenderTarget(null);
        }

        // Returns what was captured
        public Texture2D GetTexture()
        {
            return renderTarget;
        }
    }
}
