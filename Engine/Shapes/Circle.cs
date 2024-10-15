using SFML.Graphics;
using SFML.System;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a circle shape.
/// </summary>
/// <param name="position"> Its initial position. </param>
/// <param name="radius"> Its initial radius. </param>
public class Circle(Vector2f position, float radius)
    : ShapeObject(new CircleShape(radius), position)
{
    new public CircleShape SFShape => (base.SFShape as CircleShape)!;


    public float Radius
    {
        get => SFShape.Radius;
        set => SFShape.Radius = value;
    }
}