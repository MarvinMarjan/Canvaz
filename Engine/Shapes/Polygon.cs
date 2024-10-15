using SFML.Graphics;
using SFML.System;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a polygon shape.
/// </summary>
public class Polygon : ShapeObject
{
    new public ConvexShape SFShape => (base.SFShape as ConvexShape)!;


    public Polygon(Vector2f[] points)
        : base(new ConvexShape((uint)points.Length), new())
    {
        for (int i = 0; i < points.Length; i++)
            SFShape.SetPoint((uint)i, points[i]);
    }
}