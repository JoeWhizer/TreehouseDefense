using System;
using System.IO;
using System.Collections.Generic;

namespace TreehouseDefense
{
    public enum Difficulty
    {
        VeryEasy, // Mapsize:  5,10 - Intruders: 5 - Towers 2 - Levels 5
        Easy,     // Mapsize:  8,14 - Intruders: 8 - Towers 4 - Levels 8
        Medium,   // Mapsize: 12,20 - Intruders: 12 - Towers 6 - Levels 11
        Hard,     // Mapsize: 16,28 - Intruders: 15 - Towers 8 - Levels 15
        Brutal    // Mapsize: 25,50 - Intruders: 25 - Towers 10 - Levels 25
    }

    class GameController
    {
        static int _screenWidth = 52;
        static double _passString = 0.4357654324;

        private string HighscoreFileName { get; set; } = "Highscore.dat";
        public bool IsGameRunning { get; set; } = false;
        public Difficulty Difficulty { get; private set; } = 0;
        public Map GameMap { get; private set; }
        public MonsterPath MapPath { get; private set; }
        public Tower[] Towers { get; set; }
        public Level[] Levels { get; set; }
        public List<Highscore> Highscores { get; set; }
        public int currentScore { get; set; }

        public void PrintWelcome()
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("**          Welcome to TreehouseDefense           **");
            Console.WriteLine("****************************************************");
            Console.WriteLine("**               H I G H S C O R E                **");
            Console.WriteLine("**                                                **");

            PrintHighscore();

            Console.WriteLine("**                                                **");
            Console.WriteLine("****************************************************");
            Console.WriteLine("**       Please press any key to continue...      **");
            Console.WriteLine("****************************************************");
            Console.ReadKey();
        }

        private void PrintHighscore()
        {
            LoadHighscore();
            

            if(Highscores.Count > 0)
            {
                foreach (var highscore in Highscores)
                {
                    // Total number of chars before and between Score and Name: 15
                    Console.Write("**        " + highscore.Score + " - " + highscore.Name);
                    int stringLength = highscore.Score.ToString().Length + 15 + highscore.Name.Length;
                    for (int i = 0; i < (_screenWidth - stringLength); i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("**");
                }
            }
        }

        public void SetDifficulty()
        {
            bool correctKeyPressed = false;
            bool error = false;
            while (correctKeyPressed == false)
            {
                Console.Clear();
                if (error)
                {
                    Console.WriteLine("Error: Invalid input!");
                    error = false;
                }

                Console.WriteLine("Please choose the difficulty:");
                Console.WriteLine("1 - Very Easy");
                Console.WriteLine("2 - Easy");
                Console.WriteLine("3 - Medium");
                Console.WriteLine("4 - Hard");
                Console.WriteLine("5 - Brutal");
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Difficulty = Difficulty.VeryEasy;
                        correctKeyPressed = true;
                        break;
                    case ConsoleKey.D2:
                        correctKeyPressed = true;
                        Difficulty = Difficulty.Easy;
                        break;
                    case ConsoleKey.D3:
                        Difficulty = Difficulty.Medium;
                        correctKeyPressed = true;
                        break;
                    case ConsoleKey.D4:
                        Difficulty = Difficulty.Hard;
                        correctKeyPressed = true;
                        break;
                    case ConsoleKey.D5:
                        Difficulty = Difficulty.Brutal;
                        correctKeyPressed = true;
                        break;
                    default:
                        error = true;
                        break;
                }
            }
            GenerateMap();
            GeneratePath();
        }

        private void GenerateMap()
        {
            GameMap = new Map();
            switch (Difficulty)
            {
                case Difficulty.VeryEasy:
                    GameMap.InitMap(5, 10);
                    break;
                case Difficulty.Easy:
                    GameMap.InitMap(8, 14);
                    break;
                case Difficulty.Medium:
                    GameMap.InitMap(12, 20);
                    break;
                case Difficulty.Hard:
                    GameMap.InitMap(16, 28);
                    break;
                case Difficulty.Brutal:
                    GameMap.InitMap(25, 50);
                    break;
            }
        }

        private void GeneratePath()
        {
            // initialize variables to decide path direction
            bool startOnXAxis = false;
            int startPoint = 0;
            int endPoint = 0;

            // Decide random if path start on x-Axis or y-Axis
            if (Random.NextDouble() <= 0.5) startOnXAxis = true;

            // Decide random where on the axis to start the path
            if (startOnXAxis == true)
            {
                endPoint = GameMap.Height;
                startPoint = Random.Range(0, GameMap.Width);
            }
            else
            {
                endPoint = GameMap.Width;
                startPoint = Random.Range(0, GameMap.Height);
            }

            // Draw path
            MapLocation[] mapLocs = new MapLocation[endPoint];
            for (int i = 0; i < endPoint; i++)
            {
                if (startOnXAxis)
                    mapLocs[i] = new MapLocation(startPoint, i, GameMap);
                else
                    mapLocs[i] = new MapLocation(i, startPoint, GameMap);
            }
            MapPath = new MonsterPath(mapLocs);
        }

        private void GenerateLevels()
        {
        }

        public void AskToPlaceTowers()
        {
        }

        public void StartGame()
        {
        }

        public void SaveHighscore()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, HighscoreFileName);

            Highscores.Sort();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                try
                {
                    foreach (var highscore in Highscores)
                    {
                        writer.WriteLine(EncryptString.Encrypt(highscore.Score + "§" + highscore.Name, _passString.ToString()));
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Error writing to File!");
                }
                finally
                {
                    writer.Close();
                }
            }
        }

        public void LoadHighscore()
        {
            List<Highscore> loadedHighscores = new List<Highscore>();
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, HighscoreFileName);

            if (!File.Exists(filePath))
                return;

            string line;
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string decryptedLines = EncryptString.Decrypt(line, _passString.ToString());
                        string[] entries = decryptedLines.Split('§');
                        loadedHighscores.Add(new Highscore { Score = int.Parse(entries[0]), Name = entries[1] });
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Error  reading file!");
                    Console.WriteLine(ex);
                }
                finally
                {
                    reader.Close();
                }
            }

            this.Highscores = loadedHighscores;
        }

    }
}