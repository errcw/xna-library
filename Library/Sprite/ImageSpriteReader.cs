using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Library.Sprite;
using Library.Animation;

namespace Library.Sprite
{
    /// <summary>
    /// Reads an image sprite stub from a binary file.
    /// </summary>
    public class ImageSpriteReader : ContentTypeReader<ImageSprite>
    {
        /// <summary>
        /// Reads the sprite descriptor XML and loads it into a template.
        /// </summary>
        protected override ImageSprite Read(ContentReader input, ImageSprite existingInstance)
        {
            Texture2D texture = input.ReadExternalReference<Texture2D>();
            Rectangle rectangle = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());
            return new ImageSprite(texture, rectangle);
        }
    }
}
