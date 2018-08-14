using System;
using System.IO;

namespace TreehouseDefense
{
    class Game
    {
        static void Main(string[] args)
        {
            GameController gameController = new GameController();

            try
            {
                gameController.StartGame();

            }
            catch (OutOfBoundsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TreehouseDefenseException)
            {
                Console.WriteLine("Unhandled TreehouseDefenseException");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception: " + ex);
            }
            Console.ReadKey();
        }
    }
}