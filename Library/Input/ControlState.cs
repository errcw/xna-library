using System;

using Microsoft.Xna.Framework.Input;

namespace Library.Input
{
    /// <summary>
    /// The state of a single control (key, button, etc.).
    /// </summary>
    public class ControlState
    {
        /// <summary>
        /// If the control is held down this frame.
        /// </summary>
        public bool Down
        {
            get { return _isDown; }
        }

        /// <summary>
        /// If the control was pressed this frame.
        /// </summary>
        public bool Pressed
        {
            get { return !_wasDown && _isDown; }
        }

        /// <summary>
        /// If the control was released this frame.
        /// </summary>
        public bool Released
        {
            get { return _wasDown && !_isDown; }
        }

        /// <summary>
        /// If the control was pressed (physically or virtually with autorepeat) this frame.
        /// </summary>
        public bool PressedRepeat
        {
            get { return (_repeatState != AutoRepeatState.Idle && _repeatElapsed == 0f); }
        }

        /// <summary>
        /// Creates a new control state.
        /// </summary>
        /// <param name="pollIsDown">A predicate to test the state of this control.</param>
        public ControlState(PollIsDown pollIsDown)
        {
            _pollIsDown = pollIsDown;
        }

        /// <summary>
        /// Updates this control.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <param name="state">The current gamepad state.</param>
        internal void Update(float time, GamePadState state)
        {
            _wasDown = _isDown;
            _isDown = _pollIsDown(state);

            if (_isDown)
            {
                _repeatElapsed += time;
                switch (_repeatState)
                {
                    case AutoRepeatState.Idle:
                        _repeatState = AutoRepeatState.Delay;
                        _repeatElapsed = 0f;
                        break;
                    case AutoRepeatState.Delay:
                        if (_repeatElapsed >= AutoRepeatStartDelay)
                        {
                            _repeatState = AutoRepeatState.Repeat;
                            _repeatElapsed = 0f;
                        }
                        break;
                    case AutoRepeatState.Repeat:
                        if (_repeatElapsed >= AutoRepeatTriggerDelay)
                        {
                            _repeatElapsed = 0f;
                        }
                        break;
                }
            }
            else
            {
                _repeatState = AutoRepeatState.Idle;
            }
        }

        private enum AutoRepeatState
        {
            Idle,
            Delay,
            Repeat
        }

        private PollIsDown _pollIsDown;

        private bool _isDown;
        private bool _wasDown;

        private AutoRepeatState _repeatState;
        private float _repeatElapsed;

        private const float AutoRepeatStartDelay = 0.3f;
        private const float AutoRepeatTriggerDelay = 0.08f;
    }
}
