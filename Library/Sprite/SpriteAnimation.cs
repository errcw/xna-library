using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Library.Animation;

namespace Library.Sprite
{
    /// <summary>
    /// Animates an attribute of a sprite between an initial and target value.
    /// </summary>
    /// <typeparam name="T">The type of attribute being animated.</typeparam>
    public abstract class SpriteAnimation<T> : IAnimation
    {
        /// <summary>
        /// Creates and starts a new sprite animation.
        /// </summary>
        /// <param name="controlee">The sprite to animate.</param>
        /// <param name="target">The target attribute of the sprite.</param>
        /// <param name="duration">The duration, in seconds, of this animaion.</param>
        /// <param name="ease">The easing between attributes.</param>
        public SpriteAnimation(Sprite controllee, T target, float duration, Ease ease)
        {
            _controllee = controllee;
            _target = target;
            _duration = duration;
            _ease = ease;
            Start();
        }

        /// <summary>
        /// Starts this sprite animation.
        /// </summary>
        public void Start()
        {
            _start = Attribute;
            _elapsed = 0f;
        }

        /// <summary>
        /// Updates this animation for the current frame.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <returns>False after the duration has elapsed, true at all times before.</returns>
        public bool Update(float time)
        {
            _elapsed += time;
            if (_elapsed < _duration)
            {
                Attribute = AnimateAttribute(_start, _target, _elapsed / _duration, _ease);
                return true;
            }
            else
            {
                Attribute = _target;
                return false;
            }
        }

        /// <summary>
        /// The animated attribute of the sprite.
        /// </summary>
        protected abstract T Attribute
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the the attribute animated between a start and target value.
        /// </summary>
        protected abstract T AnimateAttribute(T start, T target, float progress, Ease ease);


        protected Sprite _controllee;

        protected T _start;
        protected T _target;
        protected Ease _ease;

        protected float _duration;
        protected float _elapsed;
    }

    /// <summary>
    /// Animates the position of a sprite.
    /// </summary>
    public class PositionAnimation : SpriteAnimation<Vector2>
    {
        /// <summary>
        /// Creates and starts a new position animation.
        /// </summary>
        public PositionAnimation(Sprite controllee, Vector2 target, float duration, Ease ease)
            : base(controllee, target, duration, ease)
        {
        }

        /// <summary>
        /// The position of the sprite.
        /// </summary>
        protected override Vector2 Attribute
        { 
            get { return _controllee.Position; }
            set { _controllee.Position = value; }
        }

        /// <summary>
        /// Returns an interpolated position.
        /// </summary>
        protected override Vector2 AnimateAttribute(Vector2 start, Vector2 target, float progress, Ease ease)
        {
            return new Vector2(
                ease(start.X, target.X - start.X, progress),
                ease(start.Y, target.Y - start.Y, progress));
        }
    }

    /// <summary>
    /// Animates the rotation of a sprite.
    /// </summary>
    public class RotationAnimation : SpriteAnimation<float>
    {
        /// <summary>
        /// Creates and starts a new rotation animation.
        /// </summary>
        public RotationAnimation(Sprite controllee, float target, float duration, Ease ease)
            : base(controllee, target, duration, ease)
        {
        }

        /// <summary>
        /// The rotation of the sprite.
        /// </summary>
        protected override float Attribute
        { 
            get { return _controllee.Rotation; }
            set { _controllee.Rotation = value; }
        }

        /// <summary>
        /// Returns an interpolated rotation.
        /// </summary>
        protected override float AnimateAttribute(float start, float target, float progress, Ease ease)
        {
            return ease(start, target - start, progress);
        }
    }

    /// <summary>
    /// Animates the scale of a sprite.
    /// </summary>
    public class ScaleAnimation : SpriteAnimation<Vector2>
    {
        /// <summary>
        /// Creates and starts a new scale animation.
        /// </summary>
        public ScaleAnimation(Sprite controllee, Vector2 target, float duration, Ease ease)
            : base(controllee, target, duration, ease)
        {
        }

        /// <summary>
        /// The rotation of the sprite.
        /// </summary>
        protected override Vector2 Attribute
        {
            get { return _controllee.Scale; }
            set { _controllee.Scale = value; }
        }

        /// <summary>
        /// Returns an interpolated rotation.
        /// </summary>
        protected override Vector2 AnimateAttribute(Vector2 start, Vector2 target, float progress, Ease ease)
        {
            return new Vector2(
                ease(start.X, target.X - start.X, progress),
                ease(start.Y, target.Y - start.Y, progress));
        }
    }

    /// <summary>
    /// Animates the color of a sprite.
    /// </summary>
    public class ColorAnimation : SpriteAnimation<Color>
    {
        /// <summary>
        /// Creates and starts a new color animation.
        /// </summary>
        public ColorAnimation(Sprite controllee, Color target, float duration, Ease ease)
            : base(controllee, target, duration, ease)
        {
        }

        /// <summary>
        /// The color of the sprite.
        /// </summary>
        protected override Color Attribute
        {
            get { return _controllee.Color; }
            set { _controllee.Color = value; }
        }

        /// <summary>
        /// Returns an interpolated color.
        /// </summary>
        protected override Color AnimateAttribute(Color start, Color target, float progress, Ease ease)
        {
            return new Color(
                ease(start.R / 255f, (target.R - start.R) / 255f, progress),
                ease(start.G / 255f, (target.G - start.G) / 255f, progress),
                ease(start.B / 255f, (target.B - start.B) / 255f, progress),
                ease(start.A / 255f, (target.A - start.A) / 255f, progress));
        }
    }
}
