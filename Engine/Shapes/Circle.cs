using SFML.Graphics;
using SFML.System;


namespace Canvaz.Engine.Shapes;


public class Circle(Vector2f position, float radius)
    : ShapeObject(new CircleShape(radius), position)
{
    new public CircleShape SFShape => (base.SFShape as CircleShape)!;
}