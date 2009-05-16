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

namespace Library.Sprite.Pipeline
{
    /// <summary>
    /// Reads a sprite descriptor template from a binary file.
    /// </summary>
    public class SpriteDescriptorReader : ContentTypeReader<SpriteDescriptorTemplate>
    {
        /// <summary>
        /// Reads the sprite descriptor XML and loads it into a template.
        /// </summary>
        protected override SpriteDescriptorTemplate Read(ContentReader input, SpriteDescriptorTemplate existingInstance)
        {
            XElement descriptorXml = XElement.Load(new StringReader(input.ReadString()));
            return new SpriteDescriptorTemplate(descriptorXml, input.ContentManager);
        }
    }
}
