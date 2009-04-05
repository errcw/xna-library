using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Sprite
{
    /// <summary>
    /// A camera for two-dimensional scenes.
    /// </summary>
    public class CameraSprite : Sprite
    {
        /// <summary>
        /// The view transform matrix.
        /// </summary>
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
                       Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                       Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f) *
                       Matrix.CreateRotationZ(-Rotation) *
                       Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);
            }
        }

        /// <summary>
        /// The title safe area. The x and y components of the vector represent
        /// the top left corner, and the z and w components represent the
        /// bottom right corner.
        /// </summary>
        public Rectangle TitleSafeArea
        {
            get
            {
                return _titleSafeArea;
            }
        }

        /// <summary>
        /// The size of the viewport.
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                return _viewportSize;
            }
        }

        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="device">The graphics device this camera is associated with.</param>
        public CameraSprite(GraphicsDevice device)
        {
            _viewportSize = new Vector2(device.Viewport.Width, device.Viewport.Height);
            _titleSafeArea = device.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Draws nothing.
        /// </summary>
        internal override void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float rotation, Vector2 scale, Color color, float layer)
        {
        }

        private Vector2 _viewportSize;
        private Rectangle _titleSafeArea;
    }
}
