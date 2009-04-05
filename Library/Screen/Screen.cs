﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Library.Input;

namespace Library.Screen
{
    /// <summary>
    /// The screen transition state.
    /// </summary>
    public enum ScreenState
    {
        Active,
        TransitionOn,
        TransitionOff,
        Inactive
    }

    /// <summary>
    /// A screen.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// The current state of the screen.
        /// </summary>
        public ScreenState State { get; protected set; }

        /// <summary>
        /// If this screen should show the screens beneath it in the stack.
        /// </summary>
        public bool ShowBeneath { get; protected set; }

        /// <summary>
        /// The time, in seconds, for this screen to go from inactive to active.
        /// </summary>
        protected float TransitionOnTime { get; set; }

        /// <summary>
        /// The time, in seconds, for this screen to go from active to inactive.
        /// </summary>
        protected float TransitionOffTime { get; set; }

        /// <summary>
        /// Creates a new screen.
        /// </summary>
        /// <param name="stack">The screen stack on which the screen is placed.</param>
        public Screen(ScreenStack stack)
        {
            _stack = stack;
            State = ScreenState.Inactive;
            ShowBeneath = false;
        }

        /// <summary>
        /// Updates this screen.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        public void Update(float time)
        {
            switch (State)
            {
                case ScreenState.Active:
                    UpdateActive(time);
                    break;
                case ScreenState.Inactive:
                    UpdateInactive(time);
                    break;
                case ScreenState.TransitionOn:
                    _transitionElapsed += time;
                    if (_transitionElapsed > TransitionOnTime)
                    {
                        State = ScreenState.Active;
                    }
                    UpdateTransitionOn(time, MathHelper.Clamp(_transitionElapsed / TransitionOnTime, 0f, 1f), _transitionStack);
                    break;
                case ScreenState.TransitionOff:
                    _transitionElapsed += time;
                    if (_transitionElapsed > TransitionOffTime)
                    {
                        State = ScreenState.Inactive;
                    }
                    UpdateTransitionOff(time, MathHelper.Clamp(_transitionElapsed / TransitionOffTime, 0f, 1f), _transitionStack);
                    break;
            }
        }

        /// <summary>
        /// Draws this screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw in.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Transitions this screen to the active state.
        /// </summary>
        /// <param name="pushed">True if the screen was pushed on the top of the stack,
        /// false if it was shown by a newly popped screen.</param>
        internal virtual void Show(bool pushed)
        {
            if (State == ScreenState.Active || State == ScreenState.TransitionOn)
            {
                return; // nothing to do
            }
            if (TransitionOnTime > 0)
            {
                State = ScreenState.TransitionOn;
                _transitionElapsed = 0f;
                _transitionStack = pushed;
            }
            else
            {
                State = ScreenState.Active;
            }
        }

        /// <summary>
        /// Transitions this screen to the inactive state.
        /// </summary>
        /// <param name="popped">True if the screen was popped from the stack,
        /// false if it is obscured by a newly pushed screen.</param>
        internal virtual void Hide(bool popped)
        {
            if (State == ScreenState.Inactive || State == ScreenState.TransitionOff)
            {
                return; // nothing to do
            }
            if (TransitionOffTime > 0)
            {
                State = ScreenState.TransitionOff;
                _transitionElapsed = 0f;
                _transitionStack = popped;
            }
            else
            {
                State = ScreenState.Inactive;
            }
        }

        /// <summary>
        /// Updates this screen when it is active.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        protected virtual void UpdateActive(float time)
        {
        }

        /// <summary>
        /// Updates this scren when it is inactive.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        protected virtual void UpdateInactive(float time)
        {
        }

        /// <summary>
        /// Updates this screen when it is transitioning on.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <param name="progress">The percentage completion of the transition.</param>
        /// <param name="pushed">True if the screen was pushed on the stack; otherwise, false.</param>
        protected virtual void UpdateTransitionOn(float time, float progress, bool pushed)
        {
        }

        /// <summary>
        /// Updates this screen when it is transitioning off.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <param name="progress">The percentage completion of the transition.</param>
        /// <param name="popped">True if the screen was popped from the stack; otherwise, false.</param>
        protected virtual void UpdateTransitionOff(float time, float progress, bool popped)
        {
        }

        private float _transitionElapsed = 0f;
        private bool _transitionStack;

        protected ScreenStack _stack;
    }
}
