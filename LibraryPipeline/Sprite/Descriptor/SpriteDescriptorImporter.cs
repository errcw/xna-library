using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace LibraryPipeline.Sprite.Descriptor
{
    /// <summary>
    /// Imports the sprite descriptor XML.
    /// </summary>
    [ContentImporter(".spritedesc", DisplayName="Sprite Descriptor Importer")]
    public class SpriteDescriptorImporter : ContentImporter<SpriteDescriptorXml>
    {
        public override SpriteDescriptorXml Import(string filename, ContentImporterContext context)
        {
            string xml = System.IO.File.ReadAllText(filename);
            return new SpriteDescriptorXml(xml);
        }
    }
}
