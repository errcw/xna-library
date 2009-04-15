using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Input
{
    /// <summary>
    /// A predicate to check if a gamepad control is down.
    /// </summary>
    /// <param name="state">The gamepad state to test.</param>
    /// <returns>True if the control is down; otherwise, false.</returns>
    public delegate bool PollIsDown(GamePadState state);

    /// <summary>
    /// A function to poll the state of a gamepad control.
    /// </summary>
    /// <param name="state">The gamepad state to test.</param>
    /// <returns>The state of the polled control.</returns>
    public delegate T PollValue<T>(GamePadState state);

    /// <summary>
    /// A set of gamepad predicates.
    /// </summary>
    public static class Controls
    {
        /// <summary>
        /// Creates a predicate to check if one or more buttons is down.
        /// </summary>
        /// <param name="buttons">The button(s) to test.</param>
        /// <returns>A predicate.</returns>
        public static PollIsDown One(Buttons buttons)
        {
            return (state => state.IsButtonDown(buttons));
        }

        /// <summary>
        /// Creates a predicate to check if any of the specified predicates return true.
        /// </summary>
        /// <param name="pollers">The predicates to test.</param>
        /// <returns>A composite predicate.</returns>
        public static PollIsDown Any(params PollIsDown[] pollers)
        {
            return (state => pollers.Any(p => p(state)));
        }

        /// <summary>
        /// Creates a predicate to check if all of the specified predicates return true.
        /// </summary>
        /// <param name="pollers">The predicates to test.</param>
        /// <returns>A composite predicate.</returns>
        public static PollIsDown All(params PollIsDown[] pollers)
        {
            return (state => pollers.All(p => p(state)));
        }
    }
}
