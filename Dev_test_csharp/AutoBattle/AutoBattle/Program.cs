using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main()
        {
            //Get the battlefield size from the player
            Console.Write("Choose the width of the battlefield: ");
            //test if the input in in correct format
            int.TryParse(Console.ReadLine(), out int width);
            while (width > 15 || width < 2)
            {
                Console.WriteLine("The sizes have to be between 3 and 20");
                Console.Write("Choose the width of the battlefield: ");
                int.TryParse(Console.ReadLine(), out width);
            }
            Console.Write("Choose the height of the battlefield: ");
            int.TryParse(Console.ReadLine(), out int height);
            while (height > 15 || height < 2)
            {
                Console.WriteLine("The sizes have to be between 3 and 20");
                Console.Write("Choose the height of the battlefield: ");
                int.TryParse(Console.ReadLine(), out height);
            }
            Grid grid = new Grid(width, height);
            int numberOfPossibleTiles = grid.grids.Count;
            Console.WriteLine("The Battlefield is set!\n\n");

            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            Setup(); 

            void Setup()
            {

                GetPlayerChoice();
            }

            void GetPlayerChoice()
            {
                
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }

            void CreatePlayerCharacter(int classIndex)
            {
               
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                PlayerCharacter = new Character(characterClass);
                PlayerCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 20;
                PlayerCharacter.PlayerIndex = 0;
                
                CreateEnemyCharacter();

            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                //I'll use the function to take a random int that is already created
                int randomInteger = GetRandomInt(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                EnemyCharacter = new Character(enemyClass);
                EnemyCharacter.Health = 100;
                //Both lines where changing Player Characters parameters and it should be Player Character
                EnemyCharacter.BaseDamage = 20;
                EnemyCharacter.PlayerIndex = 1;
                StartGame();

            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                //Randomize which character starts first
                if (currentTurn == 0)
                {
                    if (GetRandomInt(0, 2) == 0)
                    {
                        Console.WriteLine("\nThe Player goes first\n");
                        AllPlayers.Add(PlayerCharacter);
                        AllPlayers.Add(EnemyCharacter);
                    }
                    else
                    {
                        Console.WriteLine("\nThe Enemy goes first\n");
                        AllPlayers.Add(EnemyCharacter);
                        AllPlayers.Add(PlayerCharacter);
                    }
                }
                AlocatePlayers();

                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Click on any key to start the game...\n");
                Console.Write(Environment.NewLine + Environment.NewLine);
                ConsoleKeyInfo key = Console.ReadKey();

                StartTurn();
            }

            void StartTurn()
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("TURN: {0}", currentTurn);
                if (PlayerCharacter.Health > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Player Health: {0}", PlayerCharacter.Health);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The Player is dead.");
                }
                if (EnemyCharacter.Health > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enemy Health: {0}", EnemyCharacter.Health);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The Enemy is dead.");
                }
                Console.ResetColor();
                Console.Write("\n");
                foreach (Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }
                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if(PlayerCharacter.Health <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("YOU LOSE");
                    EndGame();
                } 
                else if (EnemyCharacter.Health <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("YOU WON");
                    EndGame();
                } else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            //Winning or loosing the game ends here
            void EndGame()
            {
                Console.ResetColor();
                Console.WriteLine("Play Again?");
                Console.WriteLine("[y] Yes, [n] No");
                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "y":
                        Console.Clear();
                        Main();
                        break;
                    case "n":
                        Environment.Exit(0);
                        break;
                    default:
                        EndGame();
                        break;
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }

            void AlocatePlayers()
            {
                AlocatePlayerCharacter();
            }

            void AlocatePlayerCharacter()
            {
                //Made the position random based in how many tiles there are
                int random = GetRandomInt(0, numberOfPossibleTiles);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                //I'll show the position as a cartesian point
                Console.Write($"\nPlayer's start position is: ({(random % grid.yLength) + 1},{(random / grid.yLength) + 1})\n");
                if (!RandomLocation.ocupied)
                {
                    //Here was created an uneeded new variable
                    RandomLocation.ocupied = true;
                    RandomLocation.who = Players.player;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                } else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = GetRandomInt(0, numberOfPossibleTiles);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"Enemy's start position is: ({(random % grid.yLength) + 1},{(random / grid.yLength) + 1})\n\n");
                if (!RandomLocation.ocupied)
                {
                    RandomLocation.ocupied = true;
                    RandomLocation.who = Players.enemy;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    Console.WriteLine("Initial Battlefield");
                    grid.drawBattlefield(grid);
                }
                else
                {
                    AlocateEnemyCharacter();
                }
            }

        }
    }
}
