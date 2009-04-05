using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Input
{
    /// <summary>
    /// Polls for player input.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// The currently active controller.
        /// </summary>
        public PlayerIndex? ActiveController { get; protected set; }

        /// <summary>
        /// An event triggered when the active controller is disconnected.
        /// </summary>
        public event EventHandler<EventArgs> ActiveControllerDisconnected;

        /// <summary>
        /// Polls the current input state.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        public void Update(float time)
        {
            PollActiveController();
            if (ActiveController != null)
            {
                PollState(time, ActiveController.Value);
                UpdateVibration(time);
            }
        }

        /// <summary>
        /// Polls all the controllers looking for the specified predicate to be true.
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
                    ActiveController = p;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the vibration on the active controller.
        /// </summary>
        /// <param name="amount">The strength (x: low frequency, y: high frequency) in [0, 1].</param>
        /// <param name="duration">The duration in seconds.</param>
        public void SetVibration(Vector2 amount, float duration)
        {
#if XBOX
            if (GamePad.SetVibration(ActiveController.Value, amount.X, amount.Y))
            {
                _vibrationDuration = duration;
            }
#endif
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
                    GamePad.SetVibration(ActiveController.Value, 0f, 0f);
                }
            }
#endif
        }

        /// <summary>
        /// Monitors the connected state of the active controller.
        /// </summary>
        private void PollActiveController()
        {
#if XBOX
            if (ActiveController != null)
            {
                // check if the controller is disconnected
                GamePadState padState = GamePad.GetState(ActiveController.Value);
                if (!padState.IsConnected)
                {
                    _prevActiveController = ActiveController.Value;
                    ActiveController = null;
                    if (ActiveControllerDisconnected != null)
                    {
                        ActiveControllerDisconnected(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                // check if the controller is reconnected
                GamePadState padState = GamePad.GetState(_prevActiveController);
                if (padState.IsConnected)
                {
                    ActiveController = _prevActiveController;
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
            GamePadState state = new GamePadState();
            return state;
#endif
        }

        private float _vibrationDuration;

        private PlayerIndex _prevActiveController;
    }
}
