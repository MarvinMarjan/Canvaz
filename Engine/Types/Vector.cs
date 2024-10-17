using SFML.System;

using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public class Vec2f(float x, float y) : IAnimateable<Vec2f>
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;


    public static implicit operator Vector2f(Vec2f vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2f(Vector2f vec2) => new(vec2.X, vec2.Y);


    public AnimationState AnimateThis(Vec2f to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2f(this, to, time, easingType);
}


public class Vec2i(int x, int y) : IAnimateable<Vec2i>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;


    public static implicit operator Vector2i(Vec2i vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2i(Vector2i vec2) => new(vec2.X, vec2.Y);


    public AnimationState AnimateThis(Vec2i to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2i(this, to, time, easingType);
}


public class Vec2u(uint x, uint y) : IAnimateable<Vec2u>
{
    public uint X { get; set; } = x;
    public uint Y { get; set; } = y;


    public static implicit operator Vector2u(Vec2u vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2u(Vector2u vec2) => new(vec2.X, vec2.Y);


    public AnimationState AnimateThis(Vec2u to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2u(this, to, time, easingType);
}