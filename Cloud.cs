using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using QuickType;
using static MainCode;

public class Cloud : Entity
{
  bool isTextureChanged = false;

  public Cloud(Body body, EntityInstance instance) : base(body, instance)
  {
  }

  public override void CollisionStart(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection)
  {
    
    if (!isTextureChanged)
    {
      instance.Tile.X += 3 * GridSize;
      isTextureChanged = true;
    }
  }
}