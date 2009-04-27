using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace LibraryPipeline.Sprite.Descriptor
{
    /// <summary>
    /// Writes the image sprite stub.
    /// </summary>
    [ContentTypeWriter]
    public class ImageSpriteStubWriter : ContentTypeWriter<ImageSpriteStub>
    {
        protected override void Write(ContentWriter output, ImageSpriteStub value)
        {
            output.WriteExternalReference(value.Texture);
            output.Write(value.Rectangle.X);
            output.Write(value.Rectangle.Y);
            output.Write(value.Rectangle.Width);
            output.Write(value.Rectangle.Height);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Pipeline.ImageSpriteTemplate, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Library.Sprite.Pipeline.ImageSpriteTemplateReader, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }
    }
}
