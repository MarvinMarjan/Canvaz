using System.IO;
using Canvaz.Engine;
using Canvaz.Engine.Animation;
using Canvaz.Engine.Shapes;

using Canvaz.Language;


namespace Canvaz;


public class CanvazApp
{
    public static void Main(string[] args)
    {
        // Main1();
        Main2();
    }


    public static void Main1()
    {
        EngineApp manager = new("Canvaz", new() {
            AntialiasingLevel = 3
        });

        Rectangle rect = new(new(100, 500), new(100, 100));
        rect.Color.Set(new(255, 255, 0));

        rect.Position.Animate(new(1500, 500), 4f, EasingType.EaseInOutQuint);
        rect.Color.Animate(new(255, 0, 0), 4f, EasingType.EaseInOutQuint);

        manager.Objects.Add(rect);

        while (manager.IsOpen)
        {
            manager.Update();
            manager.Draw();
        }
    }


    public static void Main2()
    {
        CanvazLanguage.Run(File.ReadAllText("/home/marvin/Documentos/program/csharp/Canvaz/Language/example.txt"));
    }
}