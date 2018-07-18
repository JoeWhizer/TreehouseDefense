namespace TreehouseDefense
{
    class ShieldedInvader : Invader
    {
      public override int Health { get; protected set; } = 2;
      
      public ShieldedInvader(Path path) : base(path)
      {
      }
      
      public override void DecreaseHealth(int factor)
      {
          if(Random.NextDouble() < .5)
          {
              base.DecreaseHealth(factor);
          }
          else
          {
              System.Console.WriteLine("Shoot at a shieldded invader but it sustained no damage");
          }
      }
    }
}