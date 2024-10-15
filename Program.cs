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

        Rectangle rect = new(new(300, 300), new(100, 100));
        Circle circle = new(new(900, 500), 10f);
        circle.SFShape.SetPointCount(300);

        AnimationState animation1 = Animate.Vector2f(rect.Position, new(500, 500), 2f);
        AnimationState animation2 = Animate.Color(rect.Color, new(255, 0, 0), 2f);
        AnimationState animation3 = Animate.Value(rect.Rotation, 50, 2f);
        AnimationState animation4 = Animate.Value(circle.Radius, 200, 3f);
        animation1.Updated += (_, arg) => rect.Position = arg.CurrentValues.ToVector2f();
        animation2.Updated += (_, arg) => rect.Color = arg.CurrentValues.ToColor();
        animation3.Updated += (_, arg) => rect.Rotation = arg.CurrentValues.ToValue();
        animation4.Updated += (_, arg) => circle.Radius = arg.CurrentValues.ToValue();

        manager.Objects.Add(rect);
        manager.Objects.Add(circle);

        while (manager.IsOpen)
        {
            circle.SFShape.Origin = new(circle.Radius, circle.Radius);

            animation1.Update();
            animation2.Update();
            animation3.Update();
            animation4.Update();

            /* Console.WriteLine($"color: {animation2.CurrentValues[0]}, {animation2.CurrentValues[1]}, {animation2.CurrentValues[2]}");
            Console.WriteLine($"color2: {rect.Color}"); */

            manager.Update();
            manager.Draw();
        }
    }
}