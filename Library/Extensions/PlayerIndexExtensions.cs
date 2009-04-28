using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Library.Extensions
{
    /// <summary>
    /// Extensions to PlayerIndex.
    /// </summary>
    public static class PlayerIndexExtensions
    {
        /// <summary>
        /// Returns true if this player is signed in; otherwise, false.
        /// </summary>
        public static bool IsSignedIn(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];
            return gamer != null;
        }

        /// <summary>
        /// Returns true if this player is signed in to LIVE; otherwise, false.
        /// </summary>
        public static bool IsSignedInToLIVE(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];
            return gamer != null && gamer.IsSignedInToLive;
        }

        /// <summary>
        /// Returns true if this player can purchase content; otherwise, false.
        /// </summary>
        public static bool CanPurchaseContent(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];
            return gamer != null && gamer.IsSignedInToLive && gamer.Privileges.AllowPurchaseContent;
        }
    }
}
