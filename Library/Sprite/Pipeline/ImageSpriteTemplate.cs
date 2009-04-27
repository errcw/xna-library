using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Sprite.Pipeline
{
    /// <summary>
    /// A template for constructing image sprites.
    /// </summary>
    public class ImageSpriteTemplate
    {
        /// <summary>
        /// Creates a new image sprite template.
        /// </summary>
        public ImageSpriteTemplate(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
        }

        /// <summary>
        /// Creates an image sprite from this template.
        /// </summary>
        public ImageSprite Create()
        {
            return new ImageSprite(_texture, _rectangle);
        }

        private Texture2D _texture;
        private Rectangle _rectangle;
    }
}
