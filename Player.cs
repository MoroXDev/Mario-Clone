using QuickType;
using Box2DSharp.Dynamics;
using static Raylib_cs.Raylib;
using Raylib_cs;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using System.Diagnostics;
using static MainCode;
using Box2DSharp.Collision.Collider;

public class Player : Entity
{
  public EntityInstance entity;

  Stopwatch oneSecondTimer = new Stopwatch();
  bool isMoving = false;
  bool isOnTheGround = false;

  public Player(Body body, EntityInstance instance) : base(body, instance)
  {
    oneSecondTimer.Start();
  }

  public override void Update(Game game)
  {
    base.Update(game);

    UpdateCamera(ref game.cameraScreen, new Vector2(Game.level.PxWid, Game.level.PxHei), ref game.camera);
    Vector2 pSize = (body.FixtureList[0].Shape as PolygonShape).Vertices[2] * 2 * Game.pixelToMeter;

    if (!isSensor())
    {
      UpdateInput(pSize);
    }

    DecreaseVelocity();

    // exit game when player fall out of the bounds
    if (body.GetPosition().Y * Game.pixelToMeter - pSize.Y / 2 > Game.level.PxHei)
    {
      Game.isGameClosed = true;
    }

    ResetBooleans();
  }

  public void UpdateCamera(ref RenderTexture2D cameraScreen, Vector2 levelSize, ref Camera2D camera)
  {
    Vector2 playerPixPos = body.GetPosition() * Game.pixelToMeter;

    if (playerPixPos.Y + cameraScreen.Texture.Height / 2 < levelSize.Y && playerPixPos.Y - cameraScreen.Texture.Height / 2 > 0)
    {
      camera.Target.Y = playerPixPos.Y;
    }
    if (playerPixPos.X + cameraScreen.Texture.Width / 2 < levelSize.X && playerPixPos.X - cameraScreen.Texture.Width / 2 > 0)
    {
      camera.Target.X = playerPixPos.X;
    }
  }

  void UpdateInput(Vector2 pSize)
  {
    Vector2 velocity = body.LinearVelocity;

    if (IsKeyPressed(KeyboardKey.C) && isOnTheGround)
    {
      isMoving = true;

      velocity.Y = -MathF.Sqrt(2 * Game.box2D.Gravity.Y * (pSize.Y * 3.5f / Game.pixelToMeter));
    }
    if (IsKeyDown(KeyboardKey.Right))
    {
      isMoving = true;

      velocity.X = 100f / Game.pixelToMeter;
    }
    if (IsKeyDown(KeyboardKey.Left))
    {
      isMoving = true;

      velocity.X = -100f / Game.pixelToMeter;
    }

    body.SetLinearVelocity(velocity);
  }

  void ResetBooleans()
  {
    isMoving = false;
  }

  void DecreaseVelocity()
  {
    if (oneSecondTimer.Elapsed.TotalSeconds > 1)
    {
      if (!isMoving)
      {
        float newVelocityX = 0;
        float decrementSpeedX = 10 / Game.pixelToMeter;
        if (body.LinearVelocity.X > 0)
        {
          newVelocityX += -decrementSpeedX;
        }
        if (body.LinearVelocity.X < 0)
        {
          newVelocityX += decrementSpeedX;
        }

        if (MathF.Abs(body.LinearVelocity.X + newVelocityX) < decrementSpeedX)
        {
          body.SetLinearVelocity(new Vector2(0, body.LinearVelocity.Y));
        }
        else
        {
          body.SetLinearVelocity(body.LinearVelocity + new Vector2(newVelocityX, 0));
        }
      }
    }
  }

  public override void CollisionStart(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection)
  {
    if (!isSensor())
    {
      switch (enemyBodyDataB.Identifier)
      {
        case "Goomba":
        case "Turtle":
          if (colDirection.Y >= 0)
          {
            body.FixtureList[0].IsSensor = true;
            body.SetLinearVelocity(new Vector2(0, -MathF.Sqrt(Game.box2D.Gravity.Y * 2 * (instance.Tile.H / Game.pixelToMeter))));
          }
          break;

        case "Collider":
          if (colDirection.Y < 0)
          {
            isOnTheGround = true;
          }
          break;

        case "Cloud":
        case "Second_Cloud":
          body.FixtureList[0].IsSensor = true;
          body.SetLinearVelocity(new Vector2(0, -MathF.Sqrt(Game.box2D.Gravity.Y * 2 * (instance.Tile.H / Game.pixelToMeter))));
          break;

        default:
          break;
      }
    }
  }

  public override void CollisionEnd(BodyData bodyDataA, BodyData enemyBodyDataB, Vector2 colDirection)
  {
    switch (enemyBodyDataB.Identifier)
    {
      case "Collider":
        isOnTheGround = false;
        break;
      default:
        break;
    }
  }
}