using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using QuickType;
using static MainCode;
using System.Numerics;
using Box2DWorld = Box2DSharp.Dynamics.World;
using Box2DSharp.Dynamics;
using Box2DSharp.Collision.Shapes;

public class Game
{
  // public
  public Camera2D camera = new Camera2D();
  public List<Entity> entities = new List<Entity>();
  public List<AnimatedEntity> AnimatedEntities = new List<AnimatedEntity>();
  public Player kosmo;
  public RenderTexture2D cameraScreen;

  // public static 
  public static bool isGameClosed = false;
  public static Box2DWorld box2D = new Box2DWorld(new Vector2(0, 20));
  public static readonly float pixelToMeter = 25.0f;
  public static Vector2 mousePos;
  public static Level level;
  public static Dictionary<long, Texture2D> tilesets = new Dictionary<long, Texture2D>();
  public static int coinsNumber = 0;



  // private 
  ContactListener listener;

  public Game()
  {
    InitLdtk();
    InitBox2D();
    InitScreen();

    InitEntities();
    box2D.SetContactListener(new ContactListener(ref kosmo, ref entities));
  }

  public void Unload()
  {
    UnloadRenderTexture(cameraScreen);

    for (int i = 0; i < tilesets.Count; i++)
    {
      UnloadTexture(tilesets.ElementAt(i).Value);
    }
  }

  public void Run()
  {
    Update();
    Draw();
  }

  void Update()
  {
    mousePos.X = GetMousePosition().X * (cameraScreen.Texture.Width / (float)GetScreenWidth());
    mousePos.Y = GetMousePosition().Y * (cameraScreen.Texture.Height / (float)GetScreenHeight());

    UpdateEntities();
    UpdateAnimatedEntities();

    box2D.Step(GetFrameTime(), 8, 3);

    foreach (var entity in entities)
    {
      entity.UpdateTexturePos();
    }

    // close game when esc is pressed
    if (IsKeyPressed(KeyboardKey.Escape))
    {
      isGameClosed = true;
    }
  }

  void UpdateEntities()
  {
    foreach (Entity entity in entities)
    {
      entity.Update(this);
    }
  }

  void UpdateAnimatedEntities()
  {
    foreach (var entity in AnimatedEntities)
    {
      entity.UpdateFrame();
    }
  }

  void Draw()
  {
    // Draw On Camera
    BeginTextureMode(cameraScreen);
    BeginMode2D(camera);

    ClearBackground(HexToColor(level.BgColor));
    DrawLayers();
    DrawEntities();
    DrawAnimatedEntities();
    DrawColliders();

    EndMode2D();
    EndTextureMode();
    // Draw On Camera

    // Draw On Window
    BeginDrawing();
    ClearBackground(White);

    DrawTexturePro(cameraScreen.Texture, new Rectangle(0, 0, cameraScreen.Texture.Width, -cameraScreen.Texture.Height), new Rectangle(0, 0, GetScreenWidth(), GetScreenHeight()), Vector2.Zero, 0, White);

    EndDrawing();
    // Draw On Window
  }

  void InitLdtk()
  {
    Ldtk ldtk = Ldtk.FromJson(File.ReadAllText(@"ldtk\LdtkMap.ldtk"));
    level = ldtk.Levels[0];

    List<EntityInstance> entitiesTemp = new List<EntityInstance>();

    foreach (var layer in level.LayerInstances)
    {
      if (layer.TilesetDefUid.HasValue)
      {
        if (!tilesets.ContainsKey(layer.TilesetDefUid.Value))
        {
          tilesets.Add(layer.TilesetDefUid.Value, LoadTexture(GetRelativePath(layer.TilesetRelPath, "tilesets")));
        }
      }
    }
  }

  void InitBox2D()
  {
    foreach (var layer in level.LayerInstances)
    {
      if (layer.Identifier == "Collision_Entities" || layer.Identifier == "Colliders")
      {
        foreach (var entity in layer.EntityInstances)
        {
          string bodyTypeStr = (string)entity.FieldInstances.FirstOrDefault(x => x.Type == "LocalEnum.BodyType").Value;
          BodyType bdType;
          Enum.TryParse<BodyType>(bodyTypeStr, true, out bdType);

          if (layer.Identifier == "Colliders")
          {
            CreateEntityBody(entity, BodyType.StaticBody, true, new Vector2(entity.Width, entity.Height) / 2);
          }
          else
          {
            CreateEntityBody(entity, bdType, true, Vector2.Zero);
          }
        }
      }
    }
  }

  void InitEntities()
  {
    //load entities to classes
    foreach (var layer in level.LayerInstances)
    {
      if (layer.Identifier == "Collision_Entities" || layer.Identifier == "Colliders")
      {
        foreach (var entity in layer.EntityInstances)
        {
          switch (entity.Identifier)
          {
            case "Player":
              entities.Add(new Player(FindBody(entity.Iid), entity));
              kosmo = (Player)entities.Last();
              break;

            case "Goomba":
              entities.Add(new Goomba(FindBody(entity.Iid), entity));
              break;

            case "Cloud":
            case "Second_Cloud":

              entities.Add(new Cloud(FindBody(entity.Iid), entity));
              break;

            case "Turtle":
              entities.Add(new Turtle(FindBody(entity.Iid), entity));
              break;

            case "Mystery_Box":
              entities.Add(new MysteryBox(FindBody(entity.Iid), entity));
              break;

            default:
              entities.Add(new Entity(FindBody(entity.Iid), entity));
              break;
          }
        }
      }
      else if (layer.Identifier == "Animated_Entities")
      {
        foreach (var entity in layer.EntityInstances)
        {
          Console.WriteLine(entity.Tile.X);
          AnimatedEntities.Add(new AnimatedEntity(entity));
        }
      }
    }
  }

  void InitScreen()
  {
    cameraScreen = LoadRenderTexture((int)(level.PxHei * (16f / 9f)), (int)level.PxHei);
    camera.Zoom = 1;
    camera.Target = new Vector2(cameraScreen.Texture.Width / 2, cameraScreen.Texture.Height / 2);
    camera.Rotation = 0;
    camera.Offset = new Vector2(cameraScreen.Texture.Width / 2, cameraScreen.Texture.Height / 2);
  }

  void DrawLayers()
  {
    foreach (var layer in level.LayerInstances)
    {
      if (layer.TilesetDefUid.HasValue)
      {
        foreach (var tile in layer.GridTiles)
        {
          DrawTexturePro(tilesets[layer.TilesetDefUid.Value], new Rectangle(tile.Src[0], tile.Src[1], layer.GridSize, layer.GridSize), new Rectangle(tile.Px[0], tile.Px[1], layer.GridSize, layer.GridSize), Vector2.Zero, 0, White);
        }

        foreach (var tile in layer.AutoLayerTiles)
        {
          DrawTexturePro(tilesets[layer.TilesetDefUid.Value], new Rectangle(tile.Src[0], tile.Src[1], layer.GridSize, layer.GridSize), new Rectangle(tile.Px[0], tile.Px[1], layer.GridSize, layer.GridSize), Vector2.Zero, 0, White);
        }
      }
    }
  }

  void DrawEntities()
  {
    foreach (Entity entity in entities)
    {
      if (entity.GetIdentifier() != "Collider" && entity.GetIdentifier() != "Player")
      {
        entity.Draw();
      }
    }

    kosmo.Draw();
  }

  void DrawAnimatedEntities()
  {
    foreach (AnimatedEntity entity in AnimatedEntities)
    {
      entity.Draw();
    }
  }

  void CreateEntityBody(EntityInstance entity, BodyType bodytype, bool fixedRotation, Vector2 origin)
  {
    var bDef = new BodyDef()
    {
      Position = new Vector2((entity.Px[0] + origin.X) / pixelToMeter, (entity.Px[1] + origin.Y) / pixelToMeter),
      BodyType = bodytype,
      FixedRotation = fixedRotation
    };

    var shape = new PolygonShape();
    shape.SetAsBox(entity.Width / 2 / pixelToMeter, entity.Height / 2 / pixelToMeter);

    var fDef = new FixtureDef()
    {
      Shape = shape,
      Density = 1.0f,
      Friction = 0.0f,
      Restitution = 0.0f,
    };
    Body body;
    (body = box2D.CreateBody(bDef)).CreateFixture(fDef);
    body.UserData = new BodyData(entity.Iid, entity.Identifier);
  }

  void DrawColliders()
  {
    foreach (var body in box2D.BodyList)
    {
      var size = (body.FixtureList[0].Shape as PolygonShape).Vertices[2] * 2;
      var rec = new Rectangle(body.GetPosition() * pixelToMeter, size * pixelToMeter);
      DrawRectanglePro(rec, size * pixelToMeter / 2, body.GetAngle() / (MathF.PI * 2) * 360, new Color(64, 238, 247, 100));
    }
  }
}