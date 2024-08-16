using System.Numerics;
using Box2DSharp.Dynamics;
using QuickType;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

public class Entity
{
  protected EntityInstance instance;
  protected Body body;

  public Entity(Body body, EntityInstance instance)
  {
    this.body = body;
    this.instance = instance;
  }

  public bool AreIdentifiersEqual(string identifier)
  {
    return identifier == instance.Identifier;
  }

  public bool AreIidsEqual(string iid)
  {
    return iid == instance.Iid;
  }

  public string GetIdentifier()
  {
    return instance.Identifier;
  }

  public bool isSensor()
  {
    foreach (var fixture in body.FixtureList)
    {
      if (!fixture.IsSensor)
      {
        return false;
      }
    }
    return true;
  }

  public virtual void Update(Game game)
  {
    UpdateTexturePos();
  }

  public virtual void CollisionStart(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection) // bodyDataA.Identifier == body.identifier
  {

  }

  public virtual void CollisionEnd(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection) // bodyDataA.Identifier == body.identifier
  {

  }

  void UpdateTexturePos()
  {
    instance.Px[0] = (long)(body.GetPosition().X * Game.pixelToMeter);
    instance.Px[1] = (long)(body.GetPosition().Y * Game.pixelToMeter);
  }
  
  public void Draw()
  {
    DrawTexturePro(Game.tilesets[instance.Tile.TilesetUid], new Rectangle(instance.Tile.X, instance.Tile.Y, instance.Tile.W, instance.Tile.H), new Rectangle(instance.Px[0] - instance.Tile.W / 2f, instance.Px[1] - instance.Tile.H / 2f, instance.Tile.W, instance.Tile.H), Vector2.Zero, 0, White);
  }
}