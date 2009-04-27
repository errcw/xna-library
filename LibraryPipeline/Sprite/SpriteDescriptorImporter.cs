using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Imports the sprite descriptor XML.
    /// </summary>
    [ContentImporter(".spritedesc", DisplayName="Sprite Descriptor Importer")]
    public class SpriteDescriptorImporter : ContentImporter<SpriteDescriptorContent>
    {
        public override SpriteDescriptorContent Import(string filename, ContentImporterContext context)
        {
            string xml = File.ReadAllText(filename);
            return new SpriteDescriptorContent(xml);
        }
    }
}
