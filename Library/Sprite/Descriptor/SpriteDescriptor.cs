using System;
using System.Collections.Generic;

using Library.Sprite;
using Library.Animation;

namespace Library.Sprite.Descriptor
{
    /// <summary>
    /// Names the components of a sprite and its associated animations.
    /// </summary>
    public class SpriteDescriptor
    {
        /// <summary>
        /// The sprite in this descriptor.
        /// </summary>
        public Sprite Sprite
        {
            get { return _sprite; }
        }

        /// <summary>
        /// Creates a new sprite descriptor.
        /// </summary>
        /// <param name="sprite">The root sprite.</param>
        /// <param name="spriteNames">The sprite name to sprite map.</param>
        /// <param name="animationNames">The animation name to animation map.</param>
        public SpriteDescriptor(Sprite sprite,
                                Dictionary<string, Sprite> spriteNames,
                                Dictionary<string, IAnimation> animationNames)
        {
            _sprite = sprite;
            _spriteNames = spriteNames;
            _animationNames = animationNames;
        }

        /// <summary>
        /// Gets a named sprite.
        /// </summary>
        /// <param name="spriteName">The name to look up.</param>
        /// <returns>The sprite; otherwise, null if no such name exists.</returns>
        public Sprite GetSprite(string spriteName)
        {
            return GetSprite<Sprite>(spriteName);
        }

        /// <summary>
        /// Gets a named sprite of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of sprite expected.</typeparam>
        /// <param name="spriteName">The name to look up.</param>
        /// <returns>The sprite; otherwise, null if no such name exists or the sprite is of the wrong type.</returns>
        public T GetSprite<T>(string spriteName) where T : Sprite
        {
            if (_spriteNames.ContainsKey(spriteName))
            {
                return _spriteNames[spriteName] as T;
            }
            return null;
        }

        /// <summary>
        /// Gets a named animation.
        /// </summary>
        /// <param name="animationName">The name to look up.</param>
        /// <returns>The animation; otherwise, null if no such name exists.</returns>
        public IAnimation GetAnimation(string animationName)
        {
            return GetAnimation<IAnimation>(animationName);
        }

        /// <summary>
        /// Gets a named animation of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of animation expected.</typeparam>
        /// <param name="animationName">The name to look up.</param>
        /// <returns>The animation; otherwise, null if no such name exists or the animation is of the wrong type.</returns>
        public T GetAnimation<T>(string animationName) where T : class, IAnimation
        {
            if (_animationNames.ContainsKey(animationName))
            {
                return _animationNames[animationName] as T;
            }
            return null;
        }

        private Sprite _sprite;
        private Dictionary<string, Sprite> _spriteNames;
        private Dictionary<string, IAnimation> _animationNames;
    }
}
