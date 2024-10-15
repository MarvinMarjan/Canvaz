using SFML.System;
using SFML.Graphics;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a rectangle shape.
/// </summary>
/// <param name="position"> Its initial position. </param>
/// <param name="size"> Its initial size. </param>
public class Rectangle(Vector2f position, Vector2f size)
    : ShapeObject(new RectangleShape(size), position)
{
    new public RectangleShape SFShape => (base.SFShape as RectangleShape)!;
}