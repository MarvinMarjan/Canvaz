using SFML.System;
using SFML.Graphics;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents primitive geometrical shapes, like rectangles, circles, polygons, etc...
/// </summary>
public abstract class ShapeObject : Object
{
    /// <summary>
    /// The SFML Shape object.
    /// </summary>
    public Shape SFShape { get; set; }


    public ShapeObject(Shape SFShape, Vector2f position)
    {
        this.SFShape = SFShape;
        this.SFShape.Position = position;
    }
}