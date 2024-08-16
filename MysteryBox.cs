using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using Box2DSharp.Dynamics;
using Newtonsoft.Json.Converters;
using QuickType;

public partial class MysteryBox : Entity
{
  bool isOpened = false;
  LootEntity? lootEntity;
  int transparency = 0;

  public MysteryBox(Body body, EntityInstance instance) : base(body, instance)
  {
    Random rnd = new Random();
    RandomLoot loot = (RandomLoot)rnd.Next(0, 4);
    if (loot == RandomLoot.coin)
    {
      lootEntity = new Coin(body.GetPosition() * Game.pixelToMeter, instance.Tile.TilesetUid);
    }
  }

  public override void Update(Game game)
  {
    base.Update(game);

    if (isOpened)
    {
      lootEntity?.Update();
    }
    if (lootEntity != null)
    {
      transparency = (int)(body.GetPosition().Y * Game.pixelToMeter - lootEntity.position.Y);
    }

    if (lootEntity != null && body.GetPosition().Y * Game.pixelToMeter - lootEntity.position.Y > 50)
    {
      lootEntity = null;
    }
  }

  public override void CollisionStart(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection)
  {
    if (enemyBodyDataB.Identifier == "Player")
    {
      if (colDirection.Y > 0)
      {
        isOpened = true;
      }
    }
  }

  public override void Draw()
  {
    base.Draw();
    if (isOpened)
    {
      lootEntity?.Draw();
    }
  }
}