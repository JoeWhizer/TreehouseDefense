using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace TreehouseDefense
{
    class Highscore
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

        }

        public List<Highscore> LoadHighscore()
        {
            List<Highscore> loadedHighscore = new List<Highscore>();
            

            return loadedHighscore;
        }

    }
}
