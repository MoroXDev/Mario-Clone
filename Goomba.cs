using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using QuickType;
using static MainCode;

public class Goomba : Entity
{
  Direction direction = Direction.left;
  public Goomba(Body body, EntityInstance instance) : base(body, instance)
  {

  }

  public override void Update(Game game)
  {
    base.Update(game);
    MoveRightLeft(ref direction, ref body, 20);
  }

  public override void CollisionStart(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection)
  {
    if (colDirection.X > 0 || colDirection.X < 0) // collision from the right
    {
      direction = direction == Direction.left ? Direction.right : Direction.left;
    }
  }
}