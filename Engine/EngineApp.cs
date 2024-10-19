using System.Collections.Generic;

using SFML.System;
using SFML.Window;
using SFML.Graphics;


namespace Canvaz.Engine;


public class EngineApp : RenderWindow
{
    public List<Object> Objects { get; set; } = [];

    public static Time DeltaTime { get; private set; }
    public static float DTSeconds => DeltaTime.AsSeconds();
    public static int DTMilliseconds => DeltaTime.AsMilliseconds();

    private readonly Clock _deltaTimeClock;


    public EngineApp(string title, ContextSettings contextSettings)
        : base(VideoMode.DesktopMode, title, Styles.Default, contextSettings)
    {
        Closed += (_, _) => Close();

        _deltaTimeClock = new();
        
        SetVerticalSyncEnabled(true);
    }


    public void Update()
    {
        DeltaTime = _deltaTimeClock.Restart();

        foreach (Object @object in Objects)
            @object.Update();

        DispatchEvents();
    }


    public void Draw()
    {
        Clear();

        foreach (Object @object in Objects)
            @object.Draw(this);

        Display();
    }
}