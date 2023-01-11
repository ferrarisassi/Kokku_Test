using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {
        //Comment what is not beign used now but can be used in the future
        /*public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;

        }*/

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;
            public Players who;

            public GridBox(int x, int y, bool ocupied, Players who, int index)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.who = who;
                this.Index = index;
            }

        }

        public enum Players : uint
        {
            none = 0,
            player = 1,
            enemy = 2,
        }

        /*
        public struct CharacterSkills
        {
            string Name;
            float damage;
            float damageMultiplier;
        }
        */

        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

    }
}
