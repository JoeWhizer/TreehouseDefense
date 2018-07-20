using System;
using System.IO;

namespace TreehouseDefense
{
    class Game
    {
        static void Main(string[] args)
        {
            GameController gameController = new GameController();

            gameController.PrintWelcome();
            gameController.SetDifficulty();
            gameController.AskToPlaceTowers();
 
            Map map = new Map(8, 5);
            try
            {
                MonsterPath path = new MonsterPath(
                    new [] {
                        new MapLocation(0, 2, map),
                        new MapLocation(1, 2, map),
                        new MapLocation(2, 2, map),
                        new MapLocation(3, 2, map),
                        new MapLocation(4, 2, map),
                        new MapLocation(5, 2, map),
                        new MapLocation(6, 2, map),
                        new MapLocation(7, 2, map)
                    }
                );

                IInvader[] invaders = 
                {
                    new ShieldedInvader(path),
                    new FastInvader(path),
                    new StrongInvader(path),
                    new ResurrectingInvader(path)
                };
                
                Tower[] towers = {
                    new AdvancedTower(new MapLocation(1, 3, map)),
                    new PowerTower(new MapLocation(3, 3, map)),
                    new PreciseTower(new MapLocation(5, 3, map))
                };
                
                Level leve1 = new Level(invaders)
                {
                    Towers = towers
                };
                
                bool playerWon = leve1.Play();
                
                Console.WriteLine("Player " + (playerWon ? "won" : "lost"));
            }
            catch(OutOfBoundsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(TreehouseDefenseException)
            {
                Console.WriteLine("Unhandled TreehouseDefenseException");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unhandled Exception: " + ex);
            }

            Console.ReadKey();
        }
    }
}