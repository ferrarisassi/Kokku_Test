using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health;
        public float BaseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; }
        public CharacterClass charClass;
        public Character(CharacterClass characterClass)
        {
            charClass = characterClass;
        }


        public bool TakeDamage(float amount)
        {
            if((Health -= amount) <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
        }

        public void WalkTO(bool CanWalk)
        {

        }

        public void StartTurn(Grid battlefield)
        {

            if (CheckCloseTargets(battlefield)) 
            {
                var rand = new Random();
                var prob = rand.Next(0, 6);
                if (prob == 0)
                {
                    SpecialHability();
                }
                else
                {
                    Attack(Target);
                }
                //Turn the base damage back if the paladin has nullified it.
                if (BaseDamage == 0)
                {
                    BaseDamage = 20;
                }
                return;
            }
            else
            {   //if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if(this.currentBox.xIndex > Target.currentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                    {
                        currentBox.ocupied = false;
                        currentBox.who = Players.none;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                        currentBox.ocupied = true;
                        currentBox.who = PlayerIndex==0? Players.player:Players.enemy;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.drawBattlefield(battlefield);

                        return;
                    }
                } else if(currentBox.xIndex < Target.currentBox.xIndex)
                {
                    currentBox.ocupied = false;
                    currentBox.who = Players.none;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                    currentBox.ocupied = true;
                    currentBox.who = PlayerIndex == 0 ? Players.player : Players.enemy;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.drawBattlefield(battlefield);
                    //This return was wrongly placed
                    return;
                }

                if (currentBox.yIndex > Target.currentBox.yIndex)
                {
                    
                    currentBox.ocupied = false;
                    currentBox.who = Players.none;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght));
                    currentBox.ocupied = true;
                    currentBox.who = PlayerIndex == 0 ? Players.player : Players.enemy;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    battlefield.drawBattlefield(battlefield);
                    return;
                }
                else if(currentBox.yIndex < Target.currentBox.yIndex)
                {
                    //Both boolean values here where switched
                    currentBox.ocupied = false;
                    currentBox.who = Players.none;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght));
                    currentBox.ocupied = true;
                    currentBox.who = PlayerIndex == 0 ? Players.player : Players.enemy;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.drawBattlefield(battlefield);
                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            //The operator used was & and should be ||
            if (left || right || up || down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            var rand = new Random();
            var damage = rand.Next(0, (int)BaseDamage);
            target.TakeDamage(damage);
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {damage} damage\n");
        }

        //Each class has a Special Ability, which can be randomly used once per turn instead of attack.
        //It has a chance 17% chance of happening per turn.
        void SpecialHability()
        {
            switch (charClass)
            {
                case CharacterClass.Archer:
                    Barrage(Target);
                    break;
                case CharacterClass.Cleric:
                    Heal();
                    break;
                case CharacterClass.Paladin:
                    Shield(Target);
                    break;
                case CharacterClass.Warrior:
                    StrongAttack(Target);
                    break;
            }
        }

        //The Shield is the Paladin's ability and nullifies the target damage in the next turn.
        void Shield(Character target)
        {
            target.BaseDamage = 0;
            Console.WriteLine($"Player {PlayerIndex} used Shield. Player {Target.PlayerIndex} won't deal damage in the next attack\n");
        }
        //The Strong Attack is the Warrior's ability and significantly damages the target.
        void StrongAttack(Character target)
        {
            var rand = new Random();
            var damage = rand.Next((int)(BaseDamage/3), (int)BaseDamage) * 2;
            target.TakeDamage(damage);
            Console.WriteLine($"Player {PlayerIndex} used Strong Attck in the player {Target.PlayerIndex} and did {damage} damage\n");
        }
        //The Heal is the Cleric's ability, which heals the player.
        void Heal()
        {
            var rand = new Random();
            var amount = rand.Next(10, 26);
            if (Health + amount > 100)
            {
                Health = 100;
            }
            else
            {
                Health += amount;
            }
            Console.WriteLine($"Player {PlayerIndex} used Heal. Restored {amount} of heatlh\n");
        }
        //The Barrage is the Archer's ability and hits the target multiple times with lower damage.
        void Barrage(Character target)
        {
            Console.WriteLine($"Player {PlayerIndex} used Barrage");
            var rand1 = new Random();
            var times = rand1.Next(2, 5);
            Console.WriteLine($"Player {PlayerIndex} will attack {times} times");
            for (int i = 0; i < times; i++)
            {
                var rand2 = new Random();
                var damage = rand2.Next(0, (int)BaseDamage/2+1);
                target.TakeDamage(damage);
                Console.WriteLine($"      used Barrage in the player {Target.PlayerIndex} and did {damage} damage");
            }
            Console.Write("\n");
        }
    }
}
