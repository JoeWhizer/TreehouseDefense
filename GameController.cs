using System;
using System.IO;
using System.Collections.Generic;

namespace TreehouseDefense
{
    public enum Difficulty
    {
        VeryEasy, // Mapsize: 10,5  - Intruders: 5-10  - Towers 2-3  - Levels 5   - Basic only
        Easy,     // Mapsize: 12,8  - Intruders: 8-16  - Towers 3-4  - Levels 8   - + Advanced Tower (2)
        Medium,   // Mapsize: 14,10 - Intruders: 12-24 - Towers 4-6  - Levels 12  - + Precise Tower (2)
        Hard,     // Mapsize: 16,12 - Intruders: 15-30 - Towers 6-8  - Levels 15  - + Power Tower (4)
        Brutal    // Mapsize: 20,18 - Intruders: 25-50 - Towers 8-12 - Levels 25
    }

    class GameController
    {
        // Object constants
        static int _screenWidth = 52;                         // Screen width of welcome and highscore screen
        static double _passString = 0.4357654324;             // passPhrase used to encrypt highscore file
        static int[] _amountInvaders = { 5, 8, 12, 15, 25 };  // Amount of invaders relevant to the chosen difficulty
        static int[] _amountTowers = { 2, 3, 4, 5, 8 };      // Amount of towers relevant to the chosen difficulty
        static int[] _amountLevels = { 5, 8, 12, 15, 25 };    // Amount of levels relevant to the chosen difficulty
        static string _highscoreFileName = "Highscore.dat";   // Filename in game directory to save highscore
        
        // Properties
        public bool IsGameRunning { get; set; } = false;
        public Difficulty Difficulty { get; private set; } = 0;
        public Map GameMap { get; private set; }
        public MonsterPath MapPath { get; private set; }
        public IInvader[] Invaders { get; set; }
        public List<Tower> Towers { get; set; } = new List<Tower>();
        public List<Level> Levels { get; set; } = new List<Level>();
        public List<Highscore> Highscores { get; set; }
        public int CurrentScore { get; set; }
        public int CurrentLevel { get; set; } = 1;

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
                Console.Write(": ");
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
                    GameMap.InitMap(10, 5);
                    break;
                case Difficulty.Easy:
                    GameMap.InitMap(12, 8);
                    break;
                case Difficulty.Medium:
                    GameMap.InitMap(14, 10);
                    break;
                case Difficulty.Hard:
                    GameMap.InitMap(16, 12);
                    break;
                case Difficulty.Brutal:
                    GameMap.InitMap(20, 18);
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

        public void AskToPlaceTowers()
        {
            /* Finalize and refactor once PrintMapToScreen() has been re-written/fixed
             * 
             */

            int towerPoints = _amountTowers[(int)Difficulty] * CurrentLevel;
            bool error = false;

            while(towerPoints > 0)
            {
                PrintMapToScreen();

                if(error)
                {
                    Console.WriteLine("!!Error while analyzing coordinates!!");
                    error = false;
                }

                Console.WriteLine("Please choose a tower to place:");
                Console.WriteLine("1 - Basic Tower ({0})", (towerPoints / 1));
                Console.WriteLine("2 - Advanced Tower ({0})", (towerPoints / 2));
                Console.WriteLine("3 - Precise Tower ({0})", (towerPoints / 2));
                Console.WriteLine("4 - Power Tower ({0})", (towerPoints / 4));
                Console.Write(": ");
                var input = Console.ReadKey();

                Console.WriteLine();
                Console.WriteLine("Enter the coordinates (x,y) to place the tower");
                Console.WriteLine("You can place towers beside the path (X)");
                Console.Write(": ");
                string inputCoordinates = Console.ReadLine();
                try
                {
                    string[] placedTower = inputCoordinates.Split(',');
                    int x = Int32.Parse(placedTower[0]);
                    int y = Int32.Parse(placedTower[1]);

                    if (input.Key == ConsoleKey.D1)
                    {
                        Towers.Add(new Tower(new MapLocation(x, y, GameMap)));
                        towerPoints -= 1;
                    }
                    else if (input.Key == ConsoleKey.D2)
                    {
                        Towers.Add(new AdvancedTower(new MapLocation(x, y, GameMap)));
                        towerPoints -= 2;
                    }
                    else if (input.Key == ConsoleKey.D3)
                    {
                        Towers.Add(new PreciseTower(new MapLocation(x, y, GameMap)));
                        towerPoints -= 2;
                    }
                    else if (input.Key == ConsoleKey.D4)
                    {
                        Towers.Add(new PowerTower(new MapLocation(x, y, GameMap)));
                        towerPoints -= 4;
                    }
                    else
                        throw new Exception();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadKey();
                    error = true;
                    continue;
                }
            }
            Console.ReadKey();
            GenerateInvaderAndLevels();
        }

        private void PrintMapToScreen()
        {
            Console.Clear();

            int xWidth = GameMap.Width + 4;
            int yHeight = GameMap.Height + 2;
            int x = 0;

            // Y-Axis
            for (int y = 0; y < yHeight; y++)
            {
                if (x == 0 && y == 0) Console.Write("  x");
                if (y > 1 && y < 12)
                    Console.Write(" " + (y - 2) + "  ");
                else if (y >= 12)
                    Console.Write((y - 2) + "  ");

                // X-Axis
                for (x = 0; x < xWidth; x++)
                {
                    if (x == 1 && y == 1)
                    {
                        Console.Write(" y");
                        continue;
                    }
                    else if (x < 14 && x > 3 && y == 0)
                    {
                        if (x == 4)
                            Console.Write(" " + (x - 4));
                        else
                            Console.Write("  " + (x - 4));

                        continue;
                    }
                    else if (x >= 14 && y == 0)
                    {
                        Console.Write(" " + (x - 4));
                        continue;
                    }
                    else if (x > 3 && y > 1)
                    {
                        // generate actual map points
                        if (MapPath.IsOnPath(new MapLocation(x - 4, y - 2, GameMap)))
                        {
                            Console.Write("X  ");
                        }
                        else
                        {
                            if (Towers.Count > 0)
                            {
                                bool foundTower = false;
                                foreach (var tower in Towers)
                                {
                                    if (tower.IsOnMap(new MapLocation(x - 4, y - 2, GameMap)))
                                    {
                                        Console.Write("T  ");
                                        foundTower = true;
                                    }
                                }
                                if (!foundTower)
                                {
                                    Console.Write("o  ");
                                }
                            }
                            else
                                Console.Write("o  ");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void GenerateInvaderAndLevels()
        {
            int nInvaders = _amountInvaders[(int)Difficulty];
            int nLevels = _amountLevels[(int)Difficulty];

            // TODO: generate levels relevant to the difficulty and tower placements
            // generate Levels
            for (int i = 0; i < nLevels; i++)
            {
                // generate Invaders
                Invaders = new IInvader[nInvaders + i];
                for (int j = 0; j < (nInvaders + i); j++)
                {
                    // TODO: Randomize invaders and type of invaders relevant to the chosen difficulty
                    Invaders[j] = GetRandomInvader(i +1);
                }
                Level level = new Level(Invaders);
                Levels.Add(level);
            }
        }
        
        private IInvader GetRandomInvader(int level)
        {


            return null;
        }

        public void StartGame()
        {
            // TODO: For loop through all levels relevant to the chosen difficulty

            // TODO: Try catch block to check for game over

        }

        public void SaveHighscore()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, _highscoreFileName);

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
            string filePath = Path.Combine(currentDirectory, _highscoreFileName);

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