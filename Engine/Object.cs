using System.Collections.Generic;

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


public interface IUpdateable
{
    void Update();
}


/// <summary>
/// The base class for all Canvaz's objects.
/// </summary>
public abstract class Object : IDrawable, IUpdateable
{
    protected List<IUpdateable> PropertyUpdateQueue  { get; set; }

    public bool Enabled { get; set; } = true;


    public Object()
    {
        PropertyUpdateQueue = [];
    }


    public abstract void Draw(RenderWindow renderWindow);
    public abstract void Update();


    public void AddPropertyToUpdateQueue(IUpdateable property)
        => PropertyUpdateQueue.Add(property);

    public void RemovePropertyFromAnimationQueue(IUpdateable property)
        => PropertyUpdateQueue.Remove(property);
}