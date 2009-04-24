using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace LibraryPipeline.Sprite
{
    /// <summary>
    /// A set of sprite groups to be packed.
    /// </summary>
    public class PackedImageSpritesContent : ContentItem
    {
        /// <summary>
        /// A group of sprites to be packed together
        /// </summary>
        public struct SpriteGroup
        {
            /// <summary>
            /// The unique name of the group.
            /// </summary>
            public string Name;

            /// <summary>
            /// The filenames of the images to pack.
            /// </summary>
            public string[] Filenames;
        }

        /// <summary>
        /// The sprite groups to pack.
        /// </summary>
        public SpriteGroup[] Groups;
    }
}
