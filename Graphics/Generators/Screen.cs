using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Physics;

namespace Platformer.Graphics.Generators
{
    public sealed class Screen : IDisposable
    {
        public readonly static int MinDim = 64;
        public readonly static int MaxDim = 4096;

        private bool isDisposed;
        private Game game;

        public readonly int Width;
        public readonly int Height;

        private RenderTarget2D target;
        private bool isSet;

        public Screen(Game game, int width, int height)
        {
            isDisposed = false;
            this.game = game ?? throw new ArgumentNullException("game");

            Width = FlatMath.Clamp(width, MinDim, MaxDim);
            Height = FlatMath.Clamp(height, MinDim, MaxDim);

            target = new RenderTarget2D(this.game.GraphicsDevice, Width, Height);
            isSet = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                target?.Dispose();
                target = null;
            }

            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void Set()
        {
            if (isSet)
            {
                throw new Exception("The Screen is already set as the rendering target.");
            }

            game.GraphicsDevice.SetRenderTarget(target);
            isSet = true;
        }

        public void Unset()
        {
            if (!isSet)
            {
                throw new Exception("Function \"Set\" must be called before \"UnSet\" as pairs.");
            }

            game.GraphicsDevice.SetRenderTarget(null);
            isSet = false;
        }

        public void Present(Sprites sprites)
        {
            Present(sprites, Color.Black);
        }

        public void Present(Sprites sprites, Color backgroundColor, bool textureFiltering = true)
        {
            if (isSet)
            {
                throw new Exception("The \"Screen\" is currently set as the render target. \"UnSet\" the \"Screen\" before presenting.");
            }

            if (sprites is null)
            {
                throw new ArgumentNullException("sprites");
            }

            game.GraphicsDevice.Clear(backgroundColor);

            Rectangle destinationRectangle = CalculateDestinationRectangle();

            sprites.Begin(null, textureFiltering, SpriteBlendType.Alpha);
            sprites.Draw(target, destinationRectangle, Color.White);
            sprites.End();
        }

        internal Rectangle CalculateDestinationRectangle()
        {
            // TODO: Should I recalculate the destination rectangle everytime or just calculate it when the game window size changes?

            Rectangle backbufferRectangle = game.GraphicsDevice.PresentationParameters.Bounds;
            float backbuffer_aspect = backbufferRectangle.Width / (float)backbufferRectangle.Height;
            float screen_aspect = Width / (float)Height;

            float rx = 0;
            float ry = 0;
            float rw = backbufferRectangle.Width;
            float rh = backbufferRectangle.Height;

            if (screen_aspect > backbuffer_aspect)
            {
                rh = rw / screen_aspect;
                ry = (backbufferRectangle.Height - rh) / 2f;
            }
            else if (screen_aspect < backbuffer_aspect)
            {
                rw = rh * screen_aspect;
                rx = (backbufferRectangle.Width - rw) / 2f;
            }

            Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
            return result;
        }
    }
}
