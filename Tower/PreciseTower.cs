namespace TreehouseDefense
{
    class PreciseTower : Tower
    {
        protected override int Range { get; } = 1;
        protected override int Power { get; } = 1;
        protected override double Accuracy { get; } = .95;
        protected override int Cost { get; } = 2;

        public PreciseTower(MapLocation location) : base(location) { }
    }
}