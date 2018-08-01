namespace TreehouseDefense
{
    class FastInvader : Invader
    {
        public override int Health { get; protected set; } = 2;
        public override int Score { get; protected set; } = 1;
        protected override int StepSize { get; } = 2;

        public FastInvader(MonsterPath path) : base(path) {}
    }
}