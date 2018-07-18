using System;
using System.IO;
using System.Collections.Generic;

namespace TreehouseDefense
{
    public class Highscore
    {
        public int Score { get; set; }
        public int Name { get; set; }
        private string FileName { get; set; } = "Highscore.dat";

        public bool CheckForHighscore(int score)
        {

            return false;
        }

        public void SaveHighscore(List<Highscore> highscores)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, FileName);
            using (StreamWriter file = new StreamWriter(filePath))
            {
                try
                {
                    highscores.ForEach(file.WriteLine);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Error writing to File!");
                }
                finally
                {
                    file.Close();
                }
            }
        }

        public List<Highscore> LoadHighscore()
        {
            List<Highscore> loadedHighscore = new List<Highscore>();
            

            return loadedHighscore;
        }

    }
}
