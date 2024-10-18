using SFML.Graphics;

using Canvaz.Engine.Types;
using Canvaz.Engine.Shapes.Properties;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a circle shape.
/// </summary>
public class Circle : ShapeObject
{
    new public CircleShape SFShape => (base.SFShape as CircleShape)!;


    public Property<Float> Radius { get; private set; }


    /// <param name="position"> Its initial position. </param>
    /// <param name="radius"> Its initial radius. </param>
    public Circle(Vec2f position, float radius)
        : base(new CircleShape(radius), position)
    {
        Radius = new(this, radius);
    }


    protected override void UpdateSFMLShapeProperties()
    {
        base.UpdateSFMLShapeProperties();

        SFShape.Radius = Radius.Value;
    }
}