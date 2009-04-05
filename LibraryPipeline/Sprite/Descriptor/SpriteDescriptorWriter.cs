using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace LibraryPipeline.Sprite.Descriptor
{
    /// <summary>
    /// Writes the sprite descriptor XML string.
    /// </summary>
    [ContentTypeWriter]
    public class SpriteDescriptorWriter : ContentTypeWriter<SpriteDescriptorXml>
    {
        protected override void Write(ContentWriter output, SpriteDescriptorXml value)
        {
            output.Write(value.Xml);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Descriptor.SpriteDescriptorTemplate, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Descriptor.SpriteDescriptorReader, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }
    }
}
