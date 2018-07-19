using System;
using System.Collections.Generic;

namespace TreehouseDefense
{
    public class Highscore : IComparable<Highscore>
    {
        public int Score { get; set; }
        public string Name { get; set; }

        public bool CheckForHighscore(List<Highscore> highscoreList, int score)
        {
            highscoreList.Sort();
            if (highscoreList[0].Score < score)
                return true;
            else
                return false;
        }

        public int CompareTo(Highscore that)
        {
            int result = this.Score.CompareTo(that.Score) * -1;

            if (result == 0)
            {
                result = this.Name.CompareTo(that.Name);
            }

            return result;
        }
    }
}
