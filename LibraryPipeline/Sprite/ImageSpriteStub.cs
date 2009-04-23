using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// An image sprite stub for loading.
    /// </summary>
    public class ImageSpriteStub
    {
        /// <summary>
        /// The texture containing the image for the sprite.
        /// </summary>
        public readonly ExternalReference<Texture2DContent> Texture;

        /// <summary>
        /// A rectangle describing which section of the texture contains the image.
        /// </summary>
        public readonly Rectangle Rectangle;

        /// <summary>
        /// Creates a new image sprite stub.
        /// </summary>
        public ImageSpriteStub(ExternalReference<Texture2DContent> texture, Rectangle rectangle)
        {
            Texture = texture;
            Rectangle = rectangle;
        }
    }
}
