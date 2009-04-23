using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Packs sprite groups.
    /// </summary>
    [ContentProcessor(DisplayName = "Packed Image Sprite Processor")]
    public class PackedImageSpritesProcessor : ContentProcessor<List<SpriteGroup>, object>
    {
        public override object Process(List<SpriteGroup> input, ContentProcessorContext context)
        {
            // add for each packed texture, each sprite: context.AddOutputFile(...);
            return null;
        }
    }
}
