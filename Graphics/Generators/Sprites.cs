using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Graphics.Generators
{
    public enum SpriteBlendType
    {
        Additive, Alpha
    }

    public sealed class Sprites : IDisposable
    {
        private bool isDisposed;
        private Game game;
        private SpriteBatch sprites;
        private BasicEffect effect;

        public Sprites(Game game)
        {
            isDisposed = false;
            this.game = game ?? throw new ArgumentNullException("game");
            sprites = new SpriteBatch(this.game.GraphicsDevice);

            effect = new BasicEffect(this.game.GraphicsDevice);
            effect.FogEnabled = false;
            effect.LightingEnabled = false;
            effect.PreferPerPixelLighting = false;
            effect.VertexColorEnabled = true;
            effect.Texture = null;
            effect.TextureEnabled = true;
            effect.Projection = Matrix.Identity;
            effect.View = Matrix.Identity;
            effect.World = Matrix.Identity;
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
                effect?.Dispose();
                sprites?.Dispose();
            }

            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void Begin(Camera camera = null, bool textureFiltering = false, SpriteBlendType blendType = SpriteBlendType.Alpha)
        {
            SamplerState samplerState = SamplerState.PointClamp;
            if (textureFiltering)
            {
                samplerState = SamplerState.AnisotropicClamp;
            }

            // TODO: Do I need to offset the projection by 1/2 pixel?

            if (camera is null)
            {
                Viewport viewport = game.GraphicsDevice.Viewport;
                effect.View = Matrix.Identity;
                effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, 0, 1);
            }
            else
            {
                // Update the camera's view and projection matrices if the camera Z position has changed.
                camera.Update();

                effect.View = camera.View;
                effect.Projection = camera.Projection;

                // TODO: Do I really want anisotropic filtering whenever the camera is farther away then the base Z.
                if (camera.Z > camera.BaseZ)
                {
                    samplerState = SamplerState.AnisotropicClamp;
                }
            }

            BlendState blendState = BlendState.AlphaBlend;
            if (blendType == SpriteBlendType.Additive)
            {
                blendState = BlendState.Additive;
            }

            sprites.Begin(samplerState: samplerState, blendState: blendState, rasterizerState: RasterizerState.CullNone, effect: effect);
        }

        public void End()
        {
            sprites.End();
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            sprites.Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, float scale, Color color)
        {
            sprites.Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            sprites.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.FlipVertically, 0f);
        }

        public void Draw(Texture2D texture, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, Color color)
        {
            sprites.Draw(texture, position, sourceRectangle, color, 0f, origin, 1f, SpriteEffects.FlipVertically, 0f);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            sprites.DrawString(font, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
        }

        public void DrawString(SpriteFont font, StringBuilder text, Vector2 position, Color color)
        {
            sprites.DrawString(font, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, float rotation, Vector2 origin, float scale, Color color)
        {
            sprites.DrawString(font, text, position, color, rotation, origin, scale, SpriteEffects.FlipVertically, 0f);
        }
    }
}
