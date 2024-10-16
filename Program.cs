using Canvaz.Engine;
using Canvaz.Engine.Animation;
using Canvaz.Engine.Shapes;
using Canvaz.Engine.Types;


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

        AnimationState animation1 = rect.Position.AnimateThis(new(1500, 500), 2f, EasingType.EaseOutCubic);
        AnimationState animation2 = Animate.Color(rect.Color, new(255, 0, 0), 2, EasingType.EaseOutCubic);
        animation1.Updated += (_, arg) => rect.Position = arg.CurrentValues.ToVector2f().ToVec2f();
        animation2.Updated += (_, arg) => rect.Color = arg.CurrentValues.ToColor();

        manager.Objects.Add(rect);

        while (manager.IsOpen)
        {
            animation1.Update();
            animation2.Update();

            /* Console.WriteLine($"color: {animation2.CurrentValues[0]}, {animation2.CurrentValues[1]}, {animation2.CurrentValues[2]}");
            Console.WriteLine($"color2: {rect.Color}"); */

            manager.Update();
            manager.Draw();
        }
    }
}