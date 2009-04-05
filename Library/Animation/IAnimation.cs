﻿using System;

namespace Library.Animation
{
    /// <summary>
    /// An animation updates some object over time.
    /// </summary>
    /// <remarks>
    /// By convention, constructing an animation should start it.
    /// </remarks>
    public interface IAnimation
    {
        /// <summary>
        /// Starts this animation.
        /// </summary>
        void Start();

        /// <summary>
        /// Updates this animation for a frame.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <returns>True if the animation is still running; otherwise, false.</returns>
        bool Update(float time);
    }
}
