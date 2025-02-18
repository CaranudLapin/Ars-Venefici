﻿using ArsVenefici.Framework.Spells;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public static class Extentions
    {
        public static void Resize<T>(this IList<T> list, int size)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (list is List<T> genericList)
            {
                genericList.RemoveRange(size, list.Count - size);
            }
            else
            {
                while (list.Count > size)
                    list.RemoveAt(list.Count - 1);
            }
        }

        /// <summary>Get the mod API for Generic Mod Config Menu, if it's loaded and compatible.</summary>
        /// <param name="modRegistry">The mod registry to extend.</param>
        /// <param name="monitor">The monitor with which to log errors.</param>
        /// <returns>Returns the API instance if available, else <c>null</c>.</returns>
        public static IGenericModConfigMenuApi GetGenericModConfigMenuApi(this IModRegistry modRegistry, IMonitor monitor)
        {
            return modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        public static int GetCurrentMana(this Farmer player)
        {
            return ModEntry.Mana.GetMana(player);
        }

        public static void AddMana(this Farmer player, int amt)
        {
            ModEntry.Mana.AddMana(player, amt);
        }

        public static int GetMaxMana(this Farmer player)
        {
            return ModEntry.Mana.GetMaxMana(player);
        }

        public static void SetMaxMana(this Farmer player, int newCap)
        {
            ModEntry.Mana.SetMaxMana(player, newCap);
        }

        /// <summary>Get a self-updating cached view of the player's magic metadata.</summary>
        public static SpellBook GetSpellBook(this Farmer player)
        {
            return ModEntry.GetSpellBook(player);
        }
    }
}
