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
    /// A set of gamepad predicates.
    /// </summary>
    public static class Controls
    {
        /// <summary>
        /// The A button on a controller.
        /// </summary>
        public static readonly PollIsDown A = (s => s.Buttons.X == ButtonState.Pressed);

        /// <summary>
        /// The B button on a controller.
        /// </summary>
        public static readonly PollIsDown B = (s => s.Buttons.B == ButtonState.Pressed);

        /// <summary>
        /// The X button on a controller.
        /// </summary>
        public static readonly PollIsDown X = (s => s.Buttons.X == ButtonState.Pressed);

        /// <summary>
        /// The Y button on a controller.
        /// </summary>
        public static readonly PollIsDown Y = (s => s.Buttons.Y == ButtonState.Pressed);

        /// <summary>
        /// The Start button on a controller.
        /// </summary>
        public static readonly PollIsDown Start = (s => s.Buttons.Start == ButtonState.Pressed);

        /// <summary>
        /// The Back button on a controller.
        /// </summary>
        public static readonly PollIsDown Back = (s => s.Buttons.Back == ButtonState.Pressed);

        /// <summary>
        /// The up button on a controller directional pad.
        /// </summary>
        public static readonly PollIsDown Up = (s => s.DPad.Up == ButtonState.Pressed);

        /// <summary>
        /// The down button on a controller directional pad.
        /// </summary>
        public static readonly PollIsDown Down = (s => s.DPad.Down == ButtonState.Pressed);

        /// <summary>
        /// The left button on a controller directional pad.
        /// </summary>
        public static readonly PollIsDown Left = (s => s.DPad.Left == ButtonState.Pressed);

        /// <summary>
        /// The right button on a controller directional pad.
        /// </summary>
        public static readonly PollIsDown Right = (s => s.DPad.Right == ButtonState.Pressed);

        /// <summary>
        /// The left thumb stick pushed up.
        /// </summary>
        public static readonly PollIsDown LeftStickUp = (s => s.ThumbSticks.Left.Y > StickDeadZone);

        /// <summary>
        /// The left thumb stick pushed down.
        /// </summary>
        public static readonly PollIsDown LeftStickDown = (s => s.ThumbSticks.Left.Y < -StickDeadZone);

        /// <summary>
        /// The left thumb stick pushed left.
        /// </summary>
        public static readonly PollIsDown LeftStickLeft = (s => s.ThumbSticks.Left.X < -StickDeadZone);

        /// <summary>
        /// The left thumb stick pushed right.
        /// </summary>
        public static readonly PollIsDown LeftStickRight = (s => s.ThumbSticks.Left.X > StickDeadZone);

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

        private const float StickDeadZone = 0.1f;
    }
}
