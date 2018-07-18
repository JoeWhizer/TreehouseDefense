using System;

namespace TreehouseDefense
{
    class PowerTower : Tower
    {
        protected override int Range { get; } = 1;
        protected override int Power { get; } = 3;
        protected override double Accuracy { get; } = .60;
      
        public PowerTower(MapLocation location) : base(location) {}
    }
}