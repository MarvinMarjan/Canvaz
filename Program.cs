using SFML.Window;
using SFML.Graphics;

using Canvaz.Engine.Shapes;


namespace Canvaz;


public class CanvazApp
{
    public static void Main(string[] args)
    {
        RenderWindow window = new(VideoMode.DesktopMode, "Canvaz");
        window.Closed += (_, _) => window.Close();

        Rectangle[] rects = new Rectangle[10];

        for (int i = 0; i < 10; i++)
            rects[i] = new(new(80 * (i + 1), 400), new(50, 50));

        while (window.IsOpen)
        {
            window.DispatchEvents();

            window.Clear();

            foreach (Rectangle rectangle in rects)
                rectangle.Draw(window);
        
            window.Display();
        }
    }
}