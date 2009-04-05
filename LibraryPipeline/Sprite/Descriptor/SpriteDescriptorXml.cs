using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryPipeline.Sprite.Descriptor
{
    /// <summary>
    /// Wraps the raw sprite descriptor XML.
    /// </summary>
    public class SpriteDescriptorXml
    {
        /// <summary>
        /// The raw XML text.
        /// </summary>
        public readonly string Xml;

        /// <summary>
        /// Creates a new sprite descriptor XML wrapper.
        /// </summary>
        /// <param name="xml">The XML text.</param>
        public SpriteDescriptorXml(string xml)
        {
            Xml = xml;
        }
    }
}
