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

    public Vector2f Position => SFShape.Position;
    public float Rotation => SFShape.Rotation;

    public float BorderSize => SFShape.OutlineThickness;

    public Color Color => SFShape.FillColor;
    public Color BorderColor => SFShape.OutlineColor;


    public ShapeObject(Shape SFShape, Vector2f position)
    {
        this.SFShape = SFShape;
        this.SFShape.Position = position;
    }


    public override void Draw(RenderWindow renderWindow)
    {
        if (!Enabled)
            return;

        renderWindow.Draw(SFShape);
    }
}