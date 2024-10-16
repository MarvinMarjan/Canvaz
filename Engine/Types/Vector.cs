using System;

using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public class Vec2<T>(T x, T y) : IAnimateable<Vec2<T>>
    where T : IConvertible
{
    public T X { get; set; } = x;
    public T Y { get; set; } = y;


    public AnimationState AnimateThis(Vec2<T> to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2(this, to, time, easingType);
}