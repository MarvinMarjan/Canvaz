using SFML.Graphics;

using Canvaz.Engine.Types;
using Canvaz.Engine.Shapes.Properties;


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

    public Property<Vec2f> Position { get; private set; }
    public Property<Float> Rotation { get; private set; }

    public Property<Float> BorderSize { get; private set; }

    public Property<ColorRGBA> Color { get; private set; }
    public Property<ColorRGBA> BorderColor { get; private set; }


    public ShapeObject(Shape SFShape, Vec2f position)
    {
        this.SFShape = SFShape;

        Position = new(this, position);
        Rotation = new(this, 0f);

        BorderSize = new(this, 0f);

        Color = new(this, new(255, 255, 255));
        BorderColor = new(this, new(255, 255, 255));
    }


    public override void Draw(RenderWindow renderWindow)
    {
        if (!Enabled)
            return;

        renderWindow.Draw(SFShape);
    }


    public override void Update()
    {
        UpdateSFMLShapeProperties();

        foreach (IUpdateable updateable in PropertyUpdateQueue)
            updateable.Update();
    }


    protected virtual void UpdateSFMLShapeProperties()
    {
        SFShape.Position = Position.Value;
        SFShape.Rotation = Rotation.Value;

        SFShape.OutlineThickness = BorderSize.Value;

        SFShape.FillColor = Color.Value;
        SFShape.OutlineColor = BorderColor.Value;
    }
}