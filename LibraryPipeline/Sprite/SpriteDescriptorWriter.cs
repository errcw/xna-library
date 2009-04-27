using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Writes the sprite descriptor XML string.
    /// </summary>
    [ContentTypeWriter]
    public class SpriteDescriptorWriter : ContentTypeWriter<SpriteDescriptorContent>
    {
        protected override void Write(ContentWriter output, SpriteDescriptorContent value)
        {
            output.Write(value.Xml);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Pipeline.SpriteDescriptorTemplate, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Pipeline.SpriteDescriptorReader, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }
    }
}
