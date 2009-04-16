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
    [ContentProcessor(DisplayName = "Sprite Packing Processor")]
    public class SpritePackerProcessor : ContentProcessor<List<SpriteGroup>, object>
    {
        public override object Process(List<SpriteGroup> input, ContentProcessorContext context)
        {
            // add for each packed texture, each sprite: context.AddOutputFile(...);
            return null;
        }
    }
}
