using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;


namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Imports the packed sprite XML.
    /// </summary>
    [ContentImporter(".sprites", DisplayName = "Packed Sprites Importer")]
    public class SpriteDescriptorImporter : ContentImporter<Dictionary<string,string[]>>
    {
    }
}
