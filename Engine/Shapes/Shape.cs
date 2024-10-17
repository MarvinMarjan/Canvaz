using SFML.System;
using SFML.Graphics;

using Canvaz.Engine.Types;


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


    public Vec2f Position
    {
        get => SFShape.Position;
        set => SFShape.Position = value;
    }

    public float Rotation
    {
        get => SFShape.Rotation;
        set => SFShape.Rotation = value;
    }


    public float BorderSize 
    {
        get => SFShape.OutlineThickness;
        set => SFShape.OutlineThickness = value;
    }


    public ColorRGBA Color
    {
        get => SFShape.FillColor;
        set => SFShape.FillColor = value;
    }

    public ColorRGBA BorderColor
    {
        get => SFShape.OutlineColor;
        set => SFShape.OutlineColor = value;
    }


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


    public override void Update()
    {
        
    }
}