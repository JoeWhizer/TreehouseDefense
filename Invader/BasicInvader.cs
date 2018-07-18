namespace TreehouseDefense
{
    class BasicInvader : Invader
    {
      public BasicInvader(MonsterPath path) : base(path) {}
      public override int Health { get; protected set; } = 2;
    }
}