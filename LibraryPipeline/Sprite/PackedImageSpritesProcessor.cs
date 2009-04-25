using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

using LibraryPipeline.Extensions;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// Packs sprite groups.
    /// </summary>
    [ContentProcessor(DisplayName = "Packed Image Sprite Processor")]
    public class PackedImageSpritesProcessor : ContentProcessor<PackedImageSpritesContent, DummyObject>
    {
        /// <summary>
        /// The possible sizes of the container texture.
        /// </summary>
        public enum ContainerSizes
        {
            S256 = 256,
            S512 = 512,
            S1024 = 1024,
            S2048 = 2048,
            S4096 = 4096
        }

        /// <summary>
        /// The width and height, in texels, of the container texture.
        /// </summary>
        [DisplayName("Container Texture Size")]
        [Description("The width and height, in texels, of the container texture.")]
        [DefaultValue(ContainerSizes.S4096)]
        public ContainerSizes ContainerSize { get; set; }

        public PackedImageSpritesProcessor()
        {
            ContainerSize = ContainerSizes.S4096;
        }

        public override DummyObject Process(PackedImageSpritesContent input, ContentProcessorContext context)
        {
            TexturePacker packer = new TexturePacker((int)ContainerSize);
            List<Texture2DContent> textures = new List<Texture2DContent>();

            foreach (var group in input.Groups)
            {
                textures.Clear();

                // load the textures to be packed
                foreach (var fileName in group.Filenames)
                {
                    string textureFile = Path.GetDirectoryName(input.Identity.SourceFilename) + @"\" + fileName;
                    context.AddDependency(textureFile);

                    ExternalReference<Texture2DContent> textureReference = new ExternalReference<Texture2DContent>(textureFile);
                    Texture2DContent texture = context.BuildAndLoadAsset<Texture2DContent, Texture2DContent>(textureReference, null);
                    texture.Name = Path.GetFileNameWithoutExtension(fileName);
                    textures.Add(texture);
                }

                // pack the textures
                int containerTextures = 0;
                while (textures.Count > 0 && containerTextures < MaximumContainersPerGroup)
                {
                    packer.Pack(textures);
                    var packedTextures = packer.GetContents();

                    Texture2DContent containerTexture = CreateContainerTexture(packedTextures);
                    ExternalReference<Texture2DContent> container = context.WriteAsset(containerTexture, group.Name + "Texture" + containerTextures);

                    foreach (var pack in packedTextures)
                    {
                        // undo the padding
                        Rectangle texturePosition = pack.Value;
                        texturePosition.X += 1;
                        texturePosition.Y += 1;
                        texturePosition.Width -= 2;
                        texturePosition.Height -= 2;

                        // write the stub
                        ImageSpriteStub stub = new ImageSpriteStub(container, texturePosition);
                        context.WriteAsset(stub, pack.Key.Name);

                        context.Logger.LogMessage("Packed {0} at {1}", pack.Key.Name, texturePosition);
                    }

                    // repeat with the textures that did not fit
                    textures = packer.GetOverflow();
                    containerTextures++;
                }

                if (containerTextures > MaximumContainersPerGroup)
                {
                    context.Logger.LogImportantMessage(
                        "Too many textures to pack in group {0}; {1} left over.", group.Name, textures.Count);
                }
            }

            return new DummyObject();
        }

        private Texture2DContent CreateContainerTexture(List<KeyValuePair<Texture2DContent, Rectangle>> textures)
        {
            BitmapContent output = new PixelBitmapContent<Color>((int)ContainerSize, (int)ContainerSize);
            foreach (var textureInfo in textures)
            {
                BitmapContent texture = textureInfo.Key.Mipmaps[0];
                int w = texture.Width;
                int h = texture.Height;

                Rectangle texturePosition = textureInfo.Value;
                int x = texturePosition.X;
                int y = texturePosition.Y;

                // Copy the main sprite data to the output sheet
                BitmapContent.Copy(texture, new Rectangle(0, 0, w, h), output, new Rectangle(x + 1, y + 1, w, h));

                // Copy a border strip from each edge of the sprite, creating
                // a one pixel padding area to avoid filtering problems if the
                // sprite is scaled or rotated
                BitmapContent.Copy(texture, new Rectangle(0, 0, 1, h), output, new Rectangle(x, y + 1, 1, h));
                BitmapContent.Copy(texture, new Rectangle(w - 1, 0, 1, h), output, new Rectangle(x + w + 1, y + 1, 1, h));
                BitmapContent.Copy(texture, new Rectangle(0, 0, w, 1), output, new Rectangle(x + 1, y, w, 1));
                BitmapContent.Copy(texture, new Rectangle(0, h - 1, w, 1), output, new Rectangle(x + 1, y + h + 1, w, 1));

                // Copy a single pixel from each corner of the sprite, filling in the corners of the one pixel padding area
                BitmapContent.Copy(texture, new Rectangle(0, 0, 1, 1), output, new Rectangle(x, y, 1, 1));
                BitmapContent.Copy(texture, new Rectangle(w - 1, 0, 1, 1), output, new Rectangle(x + w + 1, y, 1, 1));
                BitmapContent.Copy(texture, new Rectangle(0, h - 1, 1, 1), output, new Rectangle(x, y + h + 1, 1, 1));
                BitmapContent.Copy(texture, new Rectangle(w - 1, h - 1, 1, 1), output, new Rectangle(x + w + 1, y + h + 1, 1, 1));
            }
            Texture2DContent tex2d = new Texture2DContent();
            tex2d.Mipmaps = new MipmapChain(output);
            return tex2d;
        }

        private const int MaximumContainersPerGroup = 20;
    }
}
