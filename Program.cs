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

        Rectangle rect = new(new(100, 500), new(100, 100))
        {
            Color = new(0, 0, 255)
        };

        AnimationState animation1 = rect.Position.AnimateThis(new(1500, 500), 2f, EasingType.EaseOutBack);
        AnimationState animation2 = Animate.Color(rect.Color, new(255, 0, 0), 2, EasingType.EaseOutBack);
        animation1.Updated += (_, arg) => rect.Position = arg.CurrentValues.ToVec2f();
        animation2.Updated += (_, arg) => rect.Color = arg.CurrentValues.ToColor();

        manager.Objects.Add(rect);

        while (manager.IsOpen)
        {
            animation1.Update();
            animation2.Update();

            manager.Update();
            manager.Draw();
        }
    }
}