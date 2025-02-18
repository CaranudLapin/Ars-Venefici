﻿using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.DragNDrop
{
    public class SpellPartSourceArea : DragSourceArea<SpellPartDraggable>
    {
        private static int X_PADDING = 4;
        private static int ROWS = 3;
        private static int COLUMNS = 8;
        private List<KeyValuePair<SpellPartDraggable, KeyValuePair<int, int>>> cachedContents = new List<KeyValuePair<SpellPartDraggable, KeyValuePair<int, int>>>();
        private string nameFilter;
        private bool showShapes;
        private bool showComponents;
        private bool showModifiers;

        public ModEntry modEntry;

        public SpellPartSourceArea(Rectangle bounds, ModEntry modEntry, string name) : base(bounds, ROWS * COLUMNS, name)
        {
            this.modEntry = modEntry;

            nameFilter = "";
            showShapes = true;
            showComponents = true;
            showModifiers = false;
            UpdateVisibility();
        }

        public SpellPartSourceArea(Rectangle bounds, ModEntry modEntry, string name, string lable) : base(bounds, ROWS * COLUMNS, name, lable)
        {
            this.modEntry = modEntry;

            nameFilter = "";
            showShapes = true;
            showComponents = true;
            showModifiers = false;
            UpdateVisibility();
        }

        public void SetNameFilter(string nameFilter)
        {
            this.nameFilter = nameFilter.ToLower();
            UpdateVisibility();
        }

        public void SetTypeFilter(bool shapes, bool components, bool modifiers)
        {
            showShapes = shapes;
            showComponents = components;
            showModifiers = modifiers;
            UpdateVisibility();
        }

        public override SpellPartDraggable ElementAt(int mouseX, int mouseY)
        {
            //return cachedContents.stream()
            //        .filter(e->mouseX >= e.getSecond().getFirst() && mouseX < e.getSecond().getFirst() + SpellPartDraggable.SIZE && mouseY >= e.getSecond().getSecond() && mouseY < e.getSecond().getSecond() + SpellPartDraggable.SIZE)
            //        .findAny()
            //        .map(Pair::getFirst)
            //        .orElse(null);

            //List<KeyValuePair<SpellPartDraggable, KeyValuePair<int, int>>> newList = cachedContents
            //    .Where(e => mouseX >= e.Value.Key && mouseX < e.Value.Key + SpellPartDraggable.SIZE && mouseY >= e.Value.Value && mouseY < e.Value.Value + SpellPartDraggable.SIZE)
            //    .ToList();

            ////newList.Select(x => x.Value);

            //return newList.Select(x => x.Key).GetEnumerator().Current;

            return cachedContents.AsEnumerable()
                    .Where(e => mouseX >= e.Value.Key && mouseX < e.Value.Key + SpellPartDraggable.SIZE && mouseY >= e.Value.Value && mouseY < e.Value.Value + SpellPartDraggable.SIZE)
                    .FirstOrDefault()
                    .Key
                    ?? null;
        }

        public override void Draw(SpriteBatch spriteBatch, int positionX, int positionY, float partialTicks)
        {
            IClickableMenu.drawTextureBox(spriteBatch, x, y, width, height, Color.White);

            foreach (KeyValuePair<SpellPartDraggable, KeyValuePair<int, int>> pair in cachedContents)
            {
                KeyValuePair<int, int> xy = pair.Value;
                pair.Key.Draw(spriteBatch, xy.Key, xy.Value, partialTicks);
            }
        }

        public override List<SpellPartDraggable> GetAll()
        {
            return GetParts()
                    .Select(c => new SpellPartDraggable(c, modEntry))
                    .ToList();
        }

        public override List<SpellPartDraggable> GetVisible()
        {
            UpdateVisibility();

            return cachedContents
                   .Select(x => x.Key)
                   .ToList();
        }

        private void UpdateVisibility()
        {
            if (modEntry != null)
            {
                cachedContents.Clear();

                ////List<SpellPartDraggable> list = getAll().stream()
                ////        .filter(part-> switch (part.getPart().getType())
                ////{
                ////    case SHAPE->showShapes;
                ////    case COMPONENT->showComponents;
                ////    case MODIFIER-> (showShapes || showComponents) && showModifiers;
                ////})
                ////    .filter(part->part.getTranslationKey().getString().toLowerCase().contains(nameFilter))
                ////    .limit(maxDisplay)
                ////    .toList();

                //List<SpellPartDraggable> list = getAll()
                //        .Where(
                //                delegate (SpellPartDraggable part)
                //                {
                //                    switch (part.getPart().GetType())
                //                    {
                //                        case SpellPartType.SHAPE:
                //                            return showShapes;
                //                        case SpellPartType.COMPONENT:
                //                            return showComponents;
                //                        case SpellPartType.MODIFIER:
                //                            return (showShapes || showComponents) && showModifiers;
                //                    }

                //                    return false;
                //                }
                //        )
                //        .Where(part => part.getTranslationKey().ToLower().Contains(nameFilter))
                //        .ToList();

                ////list.Resize(maxDisplay);

                ////SpellPartDraggable[] visible = new SpellPartDraggable[maxDisplay];

                ////for (int i = 0; i < visible.Length; i++)
                ////{
                ////    visible[i] = getAll()[i];
                ////}

                ////list = visible.ToList();

                List<SpellPartDraggable> list = GetAll().AsEnumerable()
                    .Where(part =>
                    {
                        switch (part.GetPart().GetType())
                        {
                            case SpellPartType.SHAPE:
                                return showShapes;
                            case SpellPartType.COMPONENT:
                                return showComponents;
                            case SpellPartType.MODIFIER:
                                return (showShapes || showComponents) && showModifiers;
                            default:
                                return false;
                        }
                    })
                    .Where(part => part.GetNameTranslationKey().ToString().ToLower().Contains(nameFilter))
                    .Take(maxDisplay)
                    .ToList();

                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        int index = i * COLUMNS + j;
                        if (index >= list.Count()) return;
                        cachedContents.Add(KeyValuePair.Create(list[index], KeyValuePair.Create(x + j * SpellPartDraggable.SIZE + X_PADDING, y + i * SpellPartDraggable.SIZE)));
                    }
                }
            }
        }

        private List<ISpellPart> GetParts()
        {
            //return ArsMagicaAPI.get().getSpellPartRegistry().stream()
            //        .filter(e->ArsMagicaAPI.get().getSkillHelper().knows(Objects.requireNonNull(Minecraft.getInstance().player), e.getId()))
            //        .toList();

            return modEntry.spellPartManager.spellParts.Values.ToList();
        }
    }
}
