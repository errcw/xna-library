using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Imports the packed sprite XML.
    /// </summary>
    [ContentImporter(".sprites", DisplayName = "Packed Image Sprite Importer")]
    public class PackedImageSpritesImporter : ContentImporter<PackedImageSpritesContent>
    {
        public override PackedImageSpritesContent Import(string filename, ContentImporterContext context)
        {
            List<PackedImageSpritesContent.SpriteGroup> spriteGroups = new List<PackedImageSpritesContent.SpriteGroup>();

            var groups = XElement.Load(filename).Elements("Group");
            foreach (var group in groups)
            {
                PackedImageSpritesContent.SpriteGroup spriteGroup = new PackedImageSpritesContent.SpriteGroup();
                spriteGroup.Name = group.Attribute("Name").Value;
                spriteGroup.Filenames = group.Elements("Sprite").Select(e => e.Attribute("Texture").Value).ToArray();
                spriteGroups.Add(spriteGroup);
            }

            PackedImageSpritesContent content = new PackedImageSpritesContent();
            content.Groups = spriteGroups.ToArray();
            content.Identity = new ContentIdentity(filename);
            content.Name = Path.GetFileNameWithoutExtension(filename);

            return content;
        }
    }
}
