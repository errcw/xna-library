using System;

using Microsoft.Xna.Framework;

namespace Library.Extensions
{
    /// <summary>
    /// Extensions to MathHelper.
    /// </summary>
    public static class MathHelperExtensions
    {
        /// <summary>
        /// Returns abs(a - b) <= epsilon.
        /// </summary>
        /// <param name="a">The first number to compare.</param>
        /// <param name="b">The second number to compare.</param>
        /// <param name="epsilon">The epsilon tolerance.</param>
        /// <returns>If a and b are within epsilon of each other.</returns>
        public static bool EpsilonEquals(this MathHelper math, float a, float b, float epsilon)
        {
            return Math.Abs(a - b) <= epsilon;
        }
    }
}
