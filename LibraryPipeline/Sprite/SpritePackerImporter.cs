using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;


namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// A group of sprites to be packed together
    /// </summary>
    public struct SpriteGroup
    {
        /// <summary>
        /// The unique name of the group.
        /// </summary>
        public string Name;

        /// <summary>
        /// The filenames of the images to pack.
        /// </summary>
        public string[] Filenames;
    }

    /// <summary>
    /// Imports the packed sprite XML.
    /// </summary>
    [ContentImporter(".sprites", DisplayName = "Packed Sprites Importer")]
    public class SpriteDescriptorImporter : ContentImporter<List<SpriteGroup>>
    {
        public override List<SpriteGroup> Import(string filename, ContentImporterContext context)
        {
            // TODO
            return null;
        }
    }
}
