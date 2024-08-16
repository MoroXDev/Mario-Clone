using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using QuickType;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

public class Coin : LootEntity
{
  TilesetRectangle tile = new TilesetRectangle();

  public Coin(Vector2 position, long TilesetDefUid)
  {
    tile.Y = 5 * 16;
    tile.X = 0;
    tile.H = 16;
    tile.W = 16;
    this.position = position;
    tile.TilesetUid = TilesetDefUid;
  }

  public override void Update()
  {
    position.Y -= 40 * GetFrameTime();
  }

  public override void Draw()
  {
    DrawTexturePro(Game.tilesets[tile.TilesetUid], new Rectangle(tile.X, tile.Y, tile.W, tile.H), new Rectangle(position, tile.W, tile.H), new Vector2(tile.W, tile.H) / 2, 0, White);
  }
}