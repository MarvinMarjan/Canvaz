using SFML.Graphics;


namespace Canvaz.Engine;


/// <summary>
/// Interface for all things that can be drawn into a window. 
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Whether this drawable is enabled or not. If not enabled, it will not be drawn.
    /// </summary>
    bool Enabled { get; set; }


    /// <summary>
    /// Draws this drawable into a window.
    /// </summary>
    /// <param name="renderWindow"> The window. </param>
    void Draw(RenderWindow renderWindow);
}


/// <summary>
/// The base class for all Canvaz's objects.
/// </summary>
public abstract class Object : IDrawable
{
    public bool Enabled { get; set; } = true;


    public abstract void Draw(RenderWindow renderWindow);
}