using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace LibraryPipeline.Sprite
{
    public class PackedTexture
    {
        /// <summary>
        /// The parent container texture.
        /// </summary>
        public Texture2DContent Container { get; private set; }

        /// <summary>
        /// The original packed texture.
        /// </summary>
        public Texture2DContent Texture { get; private set; }

        /// <summary>
        /// The position of the child texture inside its container.
        /// </summary>
        public Rectangle Bounds { get; private set; }
    }

    /// <summary>
    /// Packs textures into one or more container textures.
    /// </summary>
    public class TexturePacker
    {
        /// <summary>
        /// Creates a new texture packer.
        /// </summary>
        /// <param name="size">The width/height of the container texture.</param>
        public TexturePacker(int size)
        {
            _size = size;
        }

        /// <summary>
        /// Packs the specified textures.
        /// </summary>
        /// <param name="textures">The textures to pack.</param>
        public List<PackedTexture> Pack(List<Texture2DContent> textures)
        {
            return null;
        }

        private int _size;
    }
}
