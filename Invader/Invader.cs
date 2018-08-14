namespace TreehouseDefense
{
    abstract class Invader : IInvader
    {
        private readonly MonsterPath _path;
        private int _pathStep = 0;

        protected virtual int StepSize { get; } = 1;

        public MapLocation Location => _path.GetLocationAt(_pathStep);

        // True if the invader has reached the end of the path
        public bool HasScored { get { return _pathStep >= _path.Length; } }

        public abstract int Score { get; protected set; }
        public abstract int Health { get; protected set; }

        public bool IsNeutralized => Health <= 0;

        public bool IsActive => !(IsNeutralized || HasScored);

        public Invader(MonsterPath path)
        {
            _path = path;
        }

        public virtual void Move() => _pathStep += StepSize;

        public virtual void DecreaseHealth(int factor)
        {
            Health -= factor;
            System.Console.WriteLine("Shot at and hit an invader!");
        }
    }
}