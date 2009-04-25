using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace LibraryPipeline.Sprite
{
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
            _packed = new List<KeyValuePair<Texture2DContent, Rectangle>>();
            _overflow = new List<Texture2DContent>();
        }

        /// <summary>
        /// Packs the specified textures.
        /// </summary>
        /// <param name="textures">The textures to pack.</param>
        /// <returns>True if every texture was successfully packed; otherwise, false.</returns>
        public bool Pack(List<Texture2DContent> textures)
        {
            _packed.Clear();
            _overflow.Clear();

            // sort the textures by area
            textures.Sort((a, b) =>
                (a.Mipmaps[0].Width * a.Mipmaps[0].Height).CompareTo(
                 b.Mipmaps[0].Width * b.Mipmaps[0].Height));

            // pack as many textures into the container as will fit
            Node root = new Node(new Rectangle(0, 0, _size, _size));

            int texidx;
            for (texidx = 0; texidx < textures.Count; texidx++)
            {
                Texture2DContent texture = textures[texidx];
                Rectangle size = new Rectangle(0, 0, texture.Mipmaps[0].Width + 2, texture.Mipmaps[0].Height + 2); // pad the size
                Rectangle? position = root.Insert(size);
                if (position != null)
                {
                    _packed.Add(new KeyValuePair<Texture2DContent, Rectangle>(texture, position.Value));
                }
                else
                {
                    break;
                }
            }

            // track the overflow items
            for (; texidx < textures.Count; texidx++)
            {
                _overflow.Add(textures[texidx]);
            }

            // indicate if there was overflow
            return _overflow.Count == 0;
        }

        /// <summary>
        /// Returns the textures packed inside this container.
        /// </summary>
        public List<KeyValuePair<Texture2DContent, Rectangle>> GetContents()
        {
            return _packed;
        }

        /// <summary>
        /// Returns the textures that were added but could not be packed in the container.
        /// </summary>
        public List<Texture2DContent> GetOverflow()
        {
            return _overflow;
        }

        /// <summary>
        /// A node in a storage tree.
        /// </summary>
        private class Node
        {
            public Node(Rectangle space)
            {
                _box = space;
                _child = null;
                _occupied = false;
            }

            /// <summary>
            /// Inserts a rectangle into the container.
            /// </summary>
            /// <param name="insert">The rectangle to insert.</param>
            /// <returns>The position of the rectangle, or null if it did not fit.</returns>
            public Rectangle? Insert(Rectangle insert)
            {
                if (_child != null)
                {
                    // if we're a parent try inserting into our children
                    Rectangle? box = _child[0].Insert(insert);
                    if (box != null)
                    {
                        return box;
                    }
                    else
                    {
                        return _child[1].Insert(insert);
                    }
                }
                else
                {
                    // otherwise create children in which to store the box
                    if (_occupied || insert.Width > _box.Width || insert.Height > _box.Height)
                    {
                        return null;
                    }
                    else if (insert.Width == _box.Width && insert.Height == _box.Height)
                    {
                        _occupied = true;
                        return _box;
                    }

                    _child = new Node[2];

                    if ((_box.Width - insert.Width) > (_box.Height - insert.Height)) // delta width > delta height
                    {
                        _child[0] = new Node(new Rectangle(_box.X, _box.Y, insert.Width, _box.Height));
                        _child[1] = new Node(new Rectangle(_box.X + insert.Width, _box.Y, _box.Width - insert.Width, _box.Height));
                    }
                    else
                    {
                        _child[0] = new Node(new Rectangle(_box.X, _box.Y, insert.Width, insert.Height));
                        _child[1] = new Node(new Rectangle(_box.X, _box.Y + insert.Height, _box.Width, _box.Height - insert.Height));
                    }

                    return _child[0].Insert(insert);
                }
            }

            private Rectangle _box; /// space occupied by this node
            private Node[] _child;  /// children of this node
            private bool _occupied; /// if this node contains a box
        }

        private int _size;
        private List<KeyValuePair<Texture2DContent, Rectangle>> _packed;
        private List<Texture2DContent> _overflow;
    }
}
