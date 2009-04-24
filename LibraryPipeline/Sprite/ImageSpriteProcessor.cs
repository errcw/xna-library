using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using LibraryPipeline.Extensions;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Generates a single image sprite.
    /// </summary>
    [ContentProcessor(DisplayName = "Image Sprite Processor")]
    public class ImageSpriteProcessor : ContentProcessor<Texture2DContent, ImageSpriteStub>
    {
        public override ImageSpriteStub Process(Texture2DContent input, ContentProcessorContext context)
        {
            string textureName = Path.GetFileNameWithoutExtension(context.OutputFilename) + "Texture";
            BitmapContent texture = input.Mipmaps[0];

            ExternalReference<Texture2DContent> texRef = context.WriteAsset(input, textureName);
            Rectangle texRect = new Rectangle(0, 0, texture.Width, texture.Height);

            return new ImageSpriteStub(texRef, texRect);
        }
    }
}
