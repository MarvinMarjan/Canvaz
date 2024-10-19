using Canvaz.Engine;
using Canvaz.Engine.Animation;
using Canvaz.Engine.Shapes;


namespace Canvaz;


public class CanvazApp
{
    public static void Main(string[] args)
    {
        EngineApp manager = new("Canvaz", new() {
            AntialiasingLevel = 3
        });

        Rectangle rect = new(new(100, 500), new(100, 100));
        rect.Color.Set(new(255, 255, 0));

        rect.Position.Animate(new(1500, 500), 2f, EasingType.EaseInOutQuint);
        rect.Color.Animate(new(255, 0, 0), 2, EasingType.EaseInOutQuint);

        manager.Objects.Add(rect);

        while (manager.IsOpen)
        {
            manager.Update();
            manager.Draw();
        }
    }
}