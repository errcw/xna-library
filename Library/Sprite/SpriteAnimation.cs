﻿using System;

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
        /// <param name="interpolate">The interpolation function for attribute values.</param>
        public SpriteAnimation(Sprite controllee, T target, float duration, Interpolate<T> interpolate)
        {
            _controllee = controllee;
            _target = target;
            _duration = duration;
            _interpolate = interpolate;
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
                Attribute = _interpolate(_start, _target, _elapsed / _duration);
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
        protected abstract T Attribute { get; set; }

        protected Sprite _controllee;

        protected T _start;
        protected T _target;
        protected Interpolate<T> _interpolate;

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
        public PositionAnimation(Sprite controllee, Vector2 target, float duration, Interpolate<Vector2> interpolate)
            : base(controllee, target, duration, interpolate)
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
    }

    /// <summary>
    /// Animates the rotation of a sprite.
    /// </summary>
    public class RotationAnimation : SpriteAnimation<float>
    {
        /// <summary>
        /// Creates and starts a new rotation animation.
        /// </summary>
        public RotationAnimation(Sprite controllee, float target, float duration, Interpolate<float> interpolate)
            : base(controllee, target, duration, interpolate)
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
    }

    /// <summary>
    /// Animates the scale of a sprite.
    /// </summary>
    public class ScaleAnimation : SpriteAnimation<Vector2>
    {
        /// <summary>
        /// Creates and starts a new scale animation.
        /// </summary>
        public ScaleAnimation(Sprite controllee, Vector2 target, float duration, Interpolate<Vector2> interpolate)
            : base(controllee, target, duration, interpolate)
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
    }

    /// <summary>
    /// Animates the color of a sprite.
    /// </summary>
    public class ColorAnimation : SpriteAnimation<Color>
    {
        /// <summary>
        /// Creates and starts a new color animation.
        /// </summary>
        public ColorAnimation(Sprite controllee, Color target, float duration, Interpolate<Color> interpolate)
            : base(controllee, target, duration, interpolate)
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
    }
}
