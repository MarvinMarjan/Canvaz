using System;

using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public class ColorRGBA : IAnimateable<ColorRGBA>
{
    private byte _r;

    public byte R
    {
        get => _r;
        set => _r = Math.Clamp(value, (byte)0, (byte)255);
    }


    private byte _g;

    public byte G
    {
        get => _g;
        set => _g = Math.Clamp(value, (byte)0, (byte)255);
    }


    private byte _b;

    public byte B
    {
        get => _b;
        set => _b = Math.Clamp(value, (byte)0, (byte)255);
    }


    private byte _a;

    public byte A
    {
        get => _a;
        set => _a = Math.Clamp(value, (byte)0, (byte)255);
    }


    public ColorRGBA(byte red, byte green, byte blue, byte alpha = 255)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }


    public static implicit operator SFML.Graphics.Color(ColorRGBA color) => new(color.R, color.G, color.B, color.A);
    public static implicit operator ColorRGBA(SFML.Graphics.Color color) => new(color.R, color.G, color.B, color.A);


    public AnimationState AnimateThis(ColorRGBA to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Color(this, to, time, easingType);
}