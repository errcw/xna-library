using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

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
            string contentName = Path.GetFileNameWithoutExtension(context.OutputFilename);
            string textureName = contentName + "Texture";

            context.AddOutputFile(context.OutputDirectory + textureName);
            ExternalReference<Texture2DContent> texRef = context.BuildAsset<Texture2DContent, Texture2DContent>(
                new ExternalReference<Texture2DContent>(input.Identity.SourceFilename), null, null, null, textureName);

            Rectangle texRect = new Rectangle(0, 0, input.Mipmaps[0].Width, input.Mipmaps[0].Height);

            return new ImageSpriteStub(texRef, texRect);
        }
    }
}
