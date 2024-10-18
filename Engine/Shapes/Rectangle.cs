using SFML.Graphics;

using Canvaz.Engine.Types;
using Canvaz.Engine.Shapes.Properties;


namespace Canvaz.Engine.Shapes;


/// <summary>
/// Represents a rectangle shape.
/// </summary>
public class Rectangle : ShapeObject
{
    new public RectangleShape SFShape => (base.SFShape as RectangleShape)!;


    public Property<Vec2f> Size { get; private set; }


    /// <param name="position"> Its initial position. </param>
    /// <param name="size"> Its initial size. </param>
    public Rectangle(Vec2f position, Vec2f size)
        : base(new RectangleShape(size), position)
    {
        Size = new(this, size);
    }


    protected override void UpdateSFMLShapeProperties()
    {
        base.UpdateSFMLShapeProperties();
    
        SFShape.Size = Size.Value;
    }
}