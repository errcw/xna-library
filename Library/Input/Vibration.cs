using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Library.Animation;

namespace Library.Input
{
    public struct VibrationKeyFrame
    {
        public readonly Vector2 Amount;
        public readonly float Duration;
        public readonly Ease Ease;

        public VibrationKeyFrame(Vector2 amount, float duration) : this(amount, duration, Easing.Uniform)
        {
        }

        public VibrationKeyFrame(Vector2 amount, float duration, Ease easing)
        {
            Amount = amount;
            Duration = duration;
            Ease = easing;
        }
    }

    /// <summary>
    /// A vibration animation.
    /// </summary>
    public class Vibration : IAnimation
    {
        /// <summary>
        /// Creates a constant vibration.
        /// </summary>
        /// <param name="amount">The strength (x: low frequency, y: high frequency) in [0, 1].</param>
        /// <param name="duration">The duration in seconds.</param>
        public static Vibration Constant(Vector2 amount, float duration)
        {
            return new Vibration(new VibrationKeyFrame(amount, duration), new VibrationKeyFrame(amount, duration));
        }

        /// <summary>
        /// Creates a new vibration animation.
        /// </summary>
        public Vibration(params VibrationKeyFrame[] frames)
        {
            if (frames.Length < 2)
            {
                throw new ArgumentException("Vibration needs two or more key frames.");
            }
            Start();
        }

        public void Start()
        {

        }

        public bool Update(float time)
        {
            return false;
        }
    }
}
