namespace TreehouseDefense
{
    class PowerTower : Tower
    {
        protected override int Range { get; } = 2;
        protected override int Power { get; } = 3;
        protected override double Accuracy { get; } = .80;
        protected override int Cost { get; } = 4;

        public PowerTower(MapLocation location) : base(location) { }
    }
}