using static Raylib_cs.Raylib;
using static MainCode;
using Raylib_cs;
using System.Drawing;
using System.Numerics;

public class Program
{
  static Vector2 windowSizeBeforeResize;
  static bool wasWindowMaximized = false;

  public static void Main()
  {
    SetConfigFlags(ConfigFlags.ResizableWindow);
    InitWindow(200, 200, "Game with LDTK");
    SetWindowSize(GetMonitorWidth(0) / 2, GetMonitorHeight(0) / 2);
    CenterWindow();

    SetTargetFPS(60);
    SetExitKey(KeyboardKey.Null);

    windowSizeBeforeResize = new Vector2(GetScreenWidth(), GetScreenHeight());

    Game game = new Game();
    while (!WindowShouldClose() && !Game.isGameClosed)
    {
      game.Run();
      UpdateWindowInput();

      wasWindowMaximized = false;
      if (IsWindowMaximized())
      {
        wasWindowMaximized = true;
      }
    }
    game.Unload();
    CloseWindow();
  }

  static void UpdateWindowInput()
  {
    bool fullscreenToggled = false;


    // Fullscreen
    if (IsKeyPressed(KeyboardKey.F11) && !IsWindowFullscreen())
    {
      SetWindowSize(GetMonitorWidth(0), GetMonitorHeight(0));
      ToggleFullscreen();

      fullscreenToggled = true;
    }
    else if (IsKeyPressed(KeyboardKey.F11) && IsWindowFullscreen())
    {
      ToggleFullscreen();
      SetWindowSize(GetMonitorWidth(0) / 2, GetMonitorHeight(0) / 2);
      CenterWindow();

      fullscreenToggled = true;
      windowSizeBeforeResize = new Vector2(GetScreenWidth(), GetScreenHeight());
    }

    if (IsWindowResized() && !fullscreenToggled)
    {
      float resizeWidth = GetScreenWidth() - windowSizeBeforeResize.X;
      float resizeHeight = GetScreenHeight() - windowSizeBeforeResize.Y;


      if (MathF.Abs(resizeWidth) > MathF.Abs(resizeHeight))
      {
        int newWidth = (int)(GetScreenWidth() + resizeWidth);
        int newHeight = (int)((GetScreenWidth() + resizeWidth) * (9 / 16f));

        SetWindowSize(newWidth, newHeight);
        CenterWindow();
      }
      else if (MathF.Abs(resizeHeight) >= MathF.Abs(resizeWidth))
      {
        int newWidth = (int)((GetScreenHeight() + resizeHeight) * (16 / 9f));
        int newHeight = (int)(GetScreenHeight() + resizeHeight);

        SetWindowSize(newWidth, newHeight);
        CenterWindow();
      }
    }

    if (!IsWindowMaximized() && wasWindowMaximized)
    {
      SetWindowSize(GetMonitorWidth(0) / 2, GetMonitorHeight(0) / 2);
      CenterWindow();
    }
  }
}