namespace TreehouseDefense
{
    class MonsterPath
    {
        private readonly MapLocation[] _path;

        public int Length => _path.Length;

        public MonsterPath(MapLocation[] path)
        {
            _path = path;
        }

        public MapLocation GetLocationAt(int pathStep)
        {
            return (pathStep < _path.Length) ? _path[pathStep] : null;
        }

        public bool IsOnPath(MapLocation location)
        {
            foreach (var pathLocation in _path)
            {
                if (location.Equals(pathLocation))
                {
                    return true;
                }
            }
            return false;
        }

        public void PrintPath()
        {
            foreach (var location in _path)
            {
                System.Console.Write(location.X + "," + location.Y + " - ");
            }
        }

    }
}