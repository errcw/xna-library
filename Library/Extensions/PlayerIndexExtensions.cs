﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

using Library.Properties;

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
        public static bool IsSignedInToLive(this PlayerIndex player)
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

        /// <summary>
        /// Shows the guide market place if this player can purchase content;
        /// otherwise shows an appropriate message.
        /// </summary>
        public static void PurchaseContent(this PlayerIndex player)
        {
            try
            {
                if (player.CanPurchaseContent())
                {
                    Guide.ShowMarketplace(player);
                }
                else
                {
                    Guide.BeginShowMessageBox(
                        player,
                        Resources.PurchaseFailedTitle,
                        Resources.PurchaseFailedText,
                        new string[] { Resources.PurchaseFailedButton },
                        0,
                        MessageBoxIcon.Warning,
                        r => Guide.EndShowMessageBox(r),
                        null);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
