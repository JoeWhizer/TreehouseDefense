namespace TreehouseDefense
{
    class StrongInvader : Invader
    {
        public override int Health { get; protected set; } = 3;
        public override int Score { get; protected set; } = 3;

        public StrongInvader(MonsterPath path) : base(path) { }


    }
}