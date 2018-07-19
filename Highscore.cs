using System;

namespace TreehouseDefense
{
    public class Highscore : IComparable<Highscore>
    {
        public int Score { get; set; }
        public string Name { get; set; }

        public bool CheckForHighscore(int score)
        {

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
