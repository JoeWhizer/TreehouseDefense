namespace TreehouseDefense
{
    class BasicInvader : Invader
    {
        public override int Health { get; protected set; } = 2;
        public override int Score { get; protected set; } = 1;

        public BasicInvader(MonsterPath path) : base(path) { }
    }
}