using System;

namespace Canvaz.Engine.Animation;


public enum EasingType
{
    Linear,

    EaseInQuad, EaseInCubic, EaseInQuart, EaseInQuint,
    EaseOutQuad, EaseOutCubic, EaseOutQuart, EaseOutQuint,
    EaseInOutQuad, EaseInOutCubic, EaseInOutQuart, EaseInOutQuint,

    EaseInExpo, EaseOutExpo, EaseInOutExpo,
    EaseInCirc, EaseOutCirc, EaseInOutCirc,
    EaseInElast, EaseOutElast, EaseInOutElast,
    EaseInBack, EaseOutBack, EaseInOutBack,
    EaseInBounce, EaseOutBounce, EaseInOutBounce
}


/// <summary>
/// Stores a lot of functions for easing animations, where "t" is the animation progress
/// from 0 to 1.
/// </summary>
public static class EasingFunctions
{
    private static float EaseIn(float t, int pow) => MathF.Pow(t, pow);
    private static float EaseOut(float t, int pow) => 1 - MathF.Pow(1 - t, pow);

    private static float EaseInOut(float t, int pow)
    {
        int mult = (int)MathF.Pow(2, pow - 1);

        if (t < 0.5)
            return mult * EaseIn(t, pow);
        else
            return 1 - MathF.Pow(-2 * t + 2, pow) / 2;
    }


    public static float Ease(float t, EasingType type) => type switch
    {
        EasingType.Linear => Linear(t),

        EasingType.EaseInQuad => EaseInQuad(t),
        EasingType.EaseInCubic => EaseInCubic(t),
        EasingType.EaseInQuart => EaseInQuart(t),
        EasingType.EaseInQuint => EaseInQuint(t),

        EasingType.EaseOutQuad => EaseOutQuad(t),
        EasingType.EaseOutCubic => EaseOutCubic(t),
        EasingType.EaseOutQuart => EaseOutQuart(t),
        EasingType.EaseOutQuint => EaseOutQuint(t),

        EasingType.EaseInOutQuad => EaseInOutQuad(t),
        EasingType.EaseInOutCubic => EaseInOutCubic(t),
        EasingType.EaseInOutQuart => EaseInOutQuart(t),
        EasingType.EaseInOutQuint => EaseInOutQuint(t),

        EasingType.EaseInExpo => EaseInExpo(t),
        EasingType.EaseOutExpo => EaseOutExpo(t),
        EasingType.EaseInOutExpo => EaseInOutExpo(t),

        /* EasingType.EaseInCirc => EaseInCirc(t),
        EasingType.EaseOutCirc => EaseOutCirc(t),
        EasingType.EaseInOutCirc => EaseInOutCirc(t),

        EasingType.EaseInElast => EaseInElast(t),
        EasingType.EaseOutElast => EaseOutElast(t),
        EasingType.EaseInOutElast => EaseInOutElast(t),

        EasingType.EaseInBack => EaseInBack(t),
        EasingType.EaseOutBack => EaseOutBack(t),
        EasingType.EaseInOutBack => EaseInOutBack(t),

        EasingType.EaseInBounce => EaseInBounce(t),
        EasingType.EaseOutBounce => EaseOutBounce(t),
        EasingType.EaseInOutBounce => EaseInOutBounce(t), */

        _ => Linear(t)
    };


    public static float Linear(float t) => t;


    public static float EaseInQuad(float t) => EaseIn(t, 2);
    public static float EaseInCubic(float t) => EaseIn(t, 3);
    public static float EaseInQuart(float t) => EaseIn(t, 4);
    public static float EaseInQuint(float t) => EaseIn(t, 5);


    public static float EaseOutQuad(float t) => EaseOut(t, 2);
    public static float EaseOutCubic(float t) => EaseOut(t, 3);
    public static float EaseOutQuart(float t) => EaseOut(t, 4);
    public static float EaseOutQuint(float t) => EaseOut(t, 5);


    public static float EaseInOutQuad(float t) => EaseInOut(t, 2);
    public static float EaseInOutCubic(float t) => EaseInOut(t, 3);
    public static float EaseInOutQuart(float t) => EaseInOut(t, 4);
    public static float EaseInOutQuint(float t) => EaseInOut(t, 5);


    public static float EaseInExpo(float t) => t == 0 ? 0 : MathF.Pow(2, 10 * t - 10);
    public static float EaseOutExpo(float t) => t == 1 ? 1 : 1 - MathF.Pow(2, -10 * t);
    public static float EaseInOutExpo(float t)
    {
        if (t is 0 or 1)
            return t;

        if (t < 0.5)
            return MathF.Pow(2, 20 * t - 10) / 2;
        else
            return (2 - MathF.Pow(2, -20 * t + 10)) / 2;
    }


    /* public static float EaseInCirc(float t) {}
    public static float EaseOutCirc(float t) {}
    public static float EaseInOutCirc(float t) {}


    public static float EaseInElast(float t) {}
    public static float EaseOutElast(float t) {}
    public static float EaseInOutElast(float t) {}


    public static float EaseInBack(float t) {}
    public static float EaseOutBack(float t) {}
    public static float EaseInOutBack(float t) {}


    public static float EaseInBounce(float t) {}
    public static float EaseOutBounce(float t) {}
    public static float EaseInOutBounce(float t) {} */
}