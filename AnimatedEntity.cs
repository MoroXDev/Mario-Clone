using System.Diagnostics;
using System.Numerics;
using QuickType;
using static Raylib_cs.Raylib;
using Raylib_cs;
using static Raylib_cs.Color;
using static MainCode;

public class AnimatedEntity
{
  TilesetRectangle tile;
  Stopwatch timer = new Stopwatch();
  Vector2 position;

  //frame 
  readonly long frames;
  readonly long minFramePosX;
  Direction shiftDirectionX;
  readonly long frameChangeTimeMs;
  readonly long shiftX;



  public AnimatedEntity(EntityInstance entity)
  {
    foreach (var field in entity.FieldInstances)
    {
      switch (field.Identifier)
      {
        case "Frames":
          frames = (long)field.Value;
          break;

        case "FrameChangeTimeMs":
          frameChangeTimeMs = (long)field.Value;
          break;

        case "DirectionX":
          Enum.TryParse<Direction>((string)field.Value, true, out shiftDirectionX);
          break;

        case "MinFramePosX":
          minFramePosX = (long)field.Value;
          break;

        case "ShiftX":
          shiftX = (long)field.Value;
          break;

      }
    }
    tile = entity.Tile;
    position.X = entity.Px[0];
    position.Y = entity.Px[1];
    minFramePosX = tile.X;
    timer.Start();

  }

  public void UpdateFrame()
  {
    UpdateAnimationX(ref tile, (int)frames, (int)frameChangeTimeMs, timer, (int)minFramePosX, (int)shiftX);
  }

  public void Draw()
  {
    DrawTexturePro(Game.tilesets[tile.TilesetUid], new Rectangle(tile.X, tile.Y, tile.W, tile.H), new Rectangle(position.X - tile.W / 2f, position.Y - tile.H / 2f, tile.W, tile.H), Vector2.Zero, 0, White);
  }
}