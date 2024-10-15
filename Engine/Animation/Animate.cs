using SFML.System;
using SFML.Graphics;

namespace Canvaz.Engine.Animation;


public static class Animate
{
    public static AnimationState Value(float from, float to, float time, EasingType easingType = EasingType.Linear)
        => new([from], [to], time, easingType);

    public static AnimationState Vector2f(Vector2f from, Vector2f to, float time, EasingType easingType = EasingType.Linear)
        => new([from.X, from.Y], [to.X, to.Y], time, easingType);

    public static AnimationState Color(Color from, Color to, float time, EasingType easingType = EasingType.Linear)
        => new([from.R, from.G, from.B, from.A], [to.R, to.G, to.B, to.A], time, easingType);
}


public static class FloatArrayConverterExtensions
{
    public static float ToValue(this float[] values)
        => values[0];
    
    public static Vector2f ToVector2f(this float[] values)
        => new(values[0], values[1]);

    public static Color ToColor(this float[] values)
        => new((byte)values[0], (byte)values[1], (byte)values[2], (byte)values[3]);
}