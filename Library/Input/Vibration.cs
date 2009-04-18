using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Library.Animation;

namespace Library.Input
{
    /// <summary>
    /// A vibration animation.
    /// </summary>
    public class VibrationAnimation : IAnimation
    {
        public struct VibrationKeyFrame
        {
            public readonly Vector2 Amount;
            public readonly float Duration;
            public readonly Ease Ease;

            public VibrationKeyFrame(Vector2 amount)
                : this(amount, 0f)
            {
            }

            public VibrationKeyFrame(Vector2 amount, float duration)
                : this(amount, duration, Easing.Uniform)
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
        /// Creates a constant vibration.
        /// </summary>
        /// <param name="leftMotor">The speed of the left, low-frequency motor in [0, 1].</param>
        /// <param name="rightMotor">The speed of the right, high-frequency motor in [0, 1].</param>
        /// <param name="duration">The duration, in seconds, to vibrate for.</param>
        public static VibrationAnimation Constant(float leftMotor, float rightMotor, float duration)
        {
            Vector2 amount = new Vector2(leftMotor, rightMotor);
            return new VibrationAnimation(
                new VibrationKeyFrame(amount, duration),
                new VibrationKeyFrame(amount));
        }

        /// <summary>
        /// Creates a vibration that fades in from nothing to a final amount.
        /// </summary>
        /// <param name="amount">The final amount of vibration.</param>
        /// <param name="duration">The time, in seconds, to fade in.</param>
        /// <returns>The vibration animation.</returns>
        public static VibrationAnimation FadeIn(float leftMotor, float rightMotor, float duration, Ease easing)
        {
            return new VibrationAnimation(
                new VibrationKeyFrame(Vector2.Zero, duration, easing),
                new VibrationKeyFrame(new Vector2(leftMotor, rightMotor)));
        }

        /// <summary>
        /// Creates a vibration that starts at some amount and fades to nothing.
        /// </summary>
        /// <param name="amount">The final amount of vibration.</param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static VibrationAnimation FadeOut(float leftMotor, float rightMotor, float duration, Ease easing)
        {
            return new VibrationAnimation(
                new VibrationKeyFrame(new Vector2(leftMotor, rightMotor), duration, easing),
                new VibrationKeyFrame(Vector2.Zero));
        }

        /// <summary>
        /// Creates a new vibration animation.
        /// </summary>
        public VibrationAnimation(params VibrationKeyFrame[] frames)
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
