using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Library.Sprite;
using Library.Animation;

namespace Library.Sprite.Pipeline
{
    /// <summary>
    /// Reads an image sprite stub from a binary file.
    /// </summary>
    public class ImageSpriteTemplateReader : ContentTypeReader<ImageSpriteTemplate>
    {
        /// <summary>
        /// Reads the sprite descriptor XML and loads it into a template.
        /// </summary>
        protected override ImageSpriteTemplate Read(ContentReader input, ImageSpriteTemplate existingInstance)
        {
            Texture2D texture = input.ReadExternalReference<Texture2D>();
            Rectangle rectangle = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());
            return new ImageSpriteTemplate(texture, rectangle);
        }
    }
}
