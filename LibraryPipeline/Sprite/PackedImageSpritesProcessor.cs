using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Packs sprite groups.
    /// </summary>
    [ContentProcessor(DisplayName = "Packed Image Sprite Processor")]
    public class PackedImageSpritesProcessor : ContentProcessor<PackedImageSpritesContent, DummyObject>
    {
        public override DummyObject Process(PackedImageSpritesContent input, ContentProcessorContext context)
        {
            TexturePacker packer = new TexturePacker(0);
            foreach (var group in input.Groups)
            {
                List<Texture2DContent> textures = new List<Texture2DContent>();

                foreach (var file in group.Filenames)
                {
                    string source = Path.GetDirectoryName(input.Identity.SourceFilename) + @"\" + file;
                    textures.Add(context.BuildAndLoadAsset<Texture2DContent, Texture2DContent>(new ExternalReference<Texture2DContent>(source), null));
                    context.AddDependency(source);
                }

                List<PackedTexture> packed = packer.Pack(textures);

                /*foreach (var pack in packed)
                {
                    ImageSpriteStub stub = new ImageSpriteStub(new ExternalReference<Texture2DContent>(""), pack.Bounds);
                    // write the stub
                }*/
                // write out container textures
                // write out individual sprites
            }

            return new DummyObject();
        }
    }
}
