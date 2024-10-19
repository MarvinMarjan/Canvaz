using SFML.Graphics;

using Canvaz.Engine.Types;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a polygon shape.
/// </summary>
public class Polygon : ShapeObject
{
    new public ConvexShape SFShape => (base.SFShape as ConvexShape)!;

    public Vec2f[] Points { get; set; }


    public Polygon(Vec2f[] points)
        : base(new ConvexShape((uint)points.Length), new())
    {
        Points = points;

        SetPoints();
    }


    private void SetPoints()
    {
        for (int i = 0; i < Points.Length; i++)
            SFShape.SetPoint((uint)i, Points[i]);
    }


    protected override void UpdateSFMLShapeProperties()
    {
        base.UpdateSFMLShapeProperties();

        SetPoints();
    }
}