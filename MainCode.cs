using System.Globalization;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using QuickType;
using System.Numerics;
using Box2DSharp.Dynamics;
using Box2DSharp.Collision.Collider;
using System.Diagnostics;
using System.Linq.Expressions;

public class MainCode
{
  public static readonly int GridSize = 16;

  public static Color HexToColor(string hex)
  {
    int r = int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
    int g = int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
    int b = int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
    return new Color(r, g, b, 255);
  }

  public static void CenterWindow()
  {
    SetWindowPosition(GetMonitorWidth(0) / 2 - GetScreenWidth() / 2, GetMonitorHeight(0) / 2 - GetScreenHeight() / 2);
  }

  public static Body FindBody(string Iid)
  {
    return Game.box2D.BodyList.FirstOrDefault(x => (x.UserData as BodyData).Iid == Iid);
  }

  public static Entity FindEntity(string Iid, Entity[] entities)
  {
    return entities.FirstOrDefault(x => x.AreIidsEqual(Iid));
  }

  public static string GetOtherIdentifier(string[] identifiers, string wrongIdentifier)
  {
    return identifiers.FirstOrDefault(x => x != wrongIdentifier);
  }

  public static string GetRelativePath(string path, string relativeTo)
  {
    return path.Remove(0, path.IndexOfAny(relativeTo.ToCharArray()));
  }

  public static Rectangle GetRectSrc(TileInstance tile, LayerInstance layer)
  {
    return new(tile.Src[0], tile.Src[1], layer.GridSize, layer.GridSize);
  }

  public static Rectangle GetRectSrc(EntityInstance entity)
  {
    return new(entity.Tile.X, entity.Tile.Y, entity.Tile.W, entity.Tile.H);
  }

  public static Vector2 GetManifoldNormal(string IidA, string IidB, Manifold manifold, string targetIid)
  {
    if (IidA == targetIid)
    {
      return manifold.LocalNormal;
    }
    else return -manifold.LocalNormal;
  }

  public static void MoveRightLeft(ref Direction direction, ref Body body, float speed)
  {
    float yVelocity = body.LinearVelocity.Y;

    if (direction == Direction.left)
    {
      body.SetLinearVelocity(new Vector2(-speed / Game.pixelToMeter, yVelocity));
    }
    else if (direction == Direction.right)
    {
      body.SetLinearVelocity(new Vector2(speed / Game.pixelToMeter, yVelocity));
    }
  }

  public static void UpdateAnimationX(ref TilesetRectangle tile, int frames, int timeMs, Stopwatch timer, int minPosX, int GridSize)
  {
    if (timer.Elapsed.TotalMilliseconds > timeMs)
    {
      if (!(tile.X + GridSize > minPosX + (frames - 1) * GridSize))
      {
        tile.X += GridSize;
      }
      else tile.X = minPosX;
      timer.Restart();
    }
  }

  public static void UpdateAnimationXReverse(ref TilesetRectangle tile, int frames, int timeMs, Stopwatch timer, int minPosX, int GridSize, ref Direction direction)
  {
    if (timer.Elapsed.TotalMilliseconds > timeMs)
    {
      if (direction == Direction.right && !(tile.X + GridSize > minPosX + (frames - 1) * GridSize))
      {
        tile.X += GridSize;
      }
      else if (direction == Direction.left && !(tile.X - GridSize < minPosX))
      {
        tile.X -= GridSize;
      }
      else
      {
        direction = direction == Direction.left ? Direction.right : Direction.left;
      }
      timer.Restart();
    }
  }
}