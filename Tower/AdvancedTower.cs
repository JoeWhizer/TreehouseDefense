using System;

namespace TreehouseDefense
{
    class AdvancedTower : Tower
    {
        protected override int Range { get; } = 2;
        protected override int Power { get; } = 1;
        protected override double Accuracy { get; } = .70;
      
        public AdvancedTower(MapLocation location) : base(location) {}
    }
}