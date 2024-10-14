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

        Polygon polygon = new([
            new(100, 100), new(200, 150),
            new(300, 400), new(100, 300),
            new(50, 50)
        ]);

        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear();

            polygon.Draw(window);
        
            window.Display();
        }
    }
}