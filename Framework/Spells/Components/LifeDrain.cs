﻿using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Components
{
    public class LifeDrain : AbstractComponent
    {
        public override string GetId()
        {
            return "life_drain";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            if (target.GetCharacter() is Monster living) 
            {
                var helper = SpellHelper.Instance();
                float damage = helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index) * 2;

                Farmer farmer = ((Farmer)caster.entity);

                if (gameLocation.damageMonster(living.GetBoundingBox(), (int)damage, (int)(damage * (1f + farmer.buffs.AttackMultiplier)), false, farmer))
                {
                    farmer.health = Math.Min(farmer.maxHealth, farmer.health + (int)damage);
                }

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override int ManaCost()
        {
            return 5;
        }
    }
}
