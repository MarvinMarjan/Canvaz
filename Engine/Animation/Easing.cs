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

        EasingType.EaseInCirc => EaseInCirc(t),
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
        EasingType.EaseInOutBounce => EaseInOutBounce(t),

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


    public static float EaseInExpo(float t) => t == 0f ? 0f : MathF.Pow(2f, 10f * t - 10f);
    public static float EaseOutExpo(float t) => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
    public static float EaseInOutExpo(float t)
    {
        if (t is 0f or 1f)
            return t;

        if (t < 0.5f)
            return MathF.Pow(2f, 20f * t - 10f) / 2f;
        else
            return (2f - MathF.Pow(2f, -20f * t + 10f)) / 2f;
    }


    public static float EaseInCirc(float t) => 1f - MathF.Sqrt(1f - MathF.Pow(t, 2f));
    public static float EaseOutCirc(float t) => MathF.Sqrt(1f - MathF.Pow(t - 1f, 2f));
    public static float EaseInOutCirc(float t)
    {
        if (t < 0.5f)
            return (1f - MathF.Sqrt(1f - MathF.Pow(2f * t, 2f))) / 2f;
        else
            return (MathF.Sqrt(1f - MathF.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
    }


    public static float EaseInElast(float t)
    {
        const float c4 = 2f * MathF.PI / 3f;

        if (t is 0f or 1f)
            return t;

        return -MathF.Pow(2f, 10f * t - 10f) * MathF.Sin((t * 10f - 10.75f) * c4);
    }

    public static float EaseOutElast(float t)
    {
        const float c4 = 2f * MathF.PI / 3f;

        if (t is 0f or 1f)
            return t;

        return MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    public static float EaseInOutElast(float t)
    {
        const float c5 = 2f * MathF.PI / 4.5f;

        if (t is 0f or 1f)
            return t;

        if (t < 0.5f)
            return -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * c5)) / 2f;
        else
            return MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * c5) / 2f + 1f;
    }


    public static float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return c3 * MathF.Pow(t, 3f) - c1 * MathF.Pow(t, 2f);
    }

    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return 1f + c3 * MathF.Pow(t - 1f, 3f) + c1 * MathF.Pow(t - 1f, 2f);
    }

    public static float EaseInOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        if (t < 0.5f)
            return MathF.Pow(2f * t, 2f) * ((c2 + 1f) * 2f * t - c2) / 2f;
        else
            return (MathF.Pow(2f * t - 2f, 2f) * ((c2 + 1f) * (t * 2f - 2f) + c2) + 2f) / 2f;
    }


    public static float EaseInBounce(float t) => 1 - EaseOutBounce(1f - t);

    public static float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1f / d1)
            return n1 * t * t;
        
        else if (t < 2 / d1) 
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        
        else if (t < 2.5 / d1) 
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        
        else 
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    public static float EaseInOutBounce(float t)
    {
        if (t < 0.5f)
            return (1f - EaseOutBounce(1f - 2f * t)) / 2f;
        else
            return (1f + EaseOutBounce(2f * t - 1f)) / 2f;
    }
}