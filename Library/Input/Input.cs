using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Input
{
    /// <summary>
    /// Polls for input from one controller.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// The currently active controller (i.e., the one being polled).
        /// </summary>
        public PlayerIndex? Controller { get; protected set; }

        /// <summary>
        /// An event triggered when the active controller is disconnected.
        /// </summary>
        public event EventHandler<EventArgs> ControllerDisconnected;

        /// <summary>
        /// Polls the current input state.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        public void Update(float time)
        {
            PollControllerConnectivity();
            if (Controller != null)
            {
                PollState(time, Controller.Value);
                UpdateVibration(time);
            }
        }

        /// <summary>
        /// Polls all the controllers looking for the specified predicate to be true. The
        /// first controller matching the predicate is assigned to ActiveController.
        /// </summary>
        /// <param name="poll">The predicate to test.</param>
        /// <returns>True if a controller was selected; otherwise, false.</returns>
        public bool FindActiveController(PollIsDown poll)
        {
            for (PlayerIndex p = PlayerIndex.One; p <= PlayerIndex.Four; p++)
            {
                GamePadState state = PollState(0f, p);
                if (poll(state))
                {
                    Controller = p;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds vibration on the active controller. Multiple calls are additive.
        /// </summary>
        /// <param name="amount">The strength (x: low frequency, y: high frequency) in [0, 1].</param>
        /// <param name="duration">The duration in seconds.</param>
        public void AddVibration(Vector2 amount, float duration)
        {
#if XBOX
            if (GamePad.SetVibration(Controller.Value, amount.X, amount.Y))
            {
                _vibrationDuration = duration;
            }
#endif
        }

        /// <summary>
        /// Stops the vibration on the active controller.
        /// </summary>
        public void StopVibration()
        {
            GamePad.SetVibration(Controller.Value, 0f, 0f);
        }

        /// <summary>
        /// Updates the controller vibration.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        private void UpdateVibration(float time)
        {
#if XBOX
            if (_vibrationDuration > 0f)
            {
                _vibrationDuration -= time;
                if (_vibrationDuration <= 0f)
                {
                    StopVibration();
                }
            }
#endif
        }

        /// <summary>
        /// Monitors the connected state of the active controller.
        /// </summary>
        private void PollControllerConnectivity()
        {
#if XBOX
            if (Controller != null)
            {
                // check if the controller is disconnected
                GamePadState padState = GamePad.GetState(Controller.Value);
                if (!padState.IsConnected)
                {
                    _prevActiveController = Controller.Value;
                    Controller = null;
                    if (ControllerDisconnected != null)
                    {
                        ControllerDisconnected(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                // check if the controller is reconnected
                GamePadState padState = GamePad.GetState(_prevController);
                if (padState.IsConnected)
                {
                    Controller = _prevController;
                }
            }
#endif
        }

        /// <summary>
        /// Polls a controller for its current state.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <param name="playerIdx">The player index to poll.</param>
        private GamePadState PollState(float time, PlayerIndex playerIdx)
        {
#if XBOX
            return GamePad.GetState(playerIdx);
#elif WINDOWS
            // fabricate the state
            GamePadState state = new GamePadState();
            return state;
#endif
        }

        private float _vibrationDuration;

        private PlayerIndex _prevController;
    }
}
