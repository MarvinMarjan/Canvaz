using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public struct Float(float value) : IAnimateable<Float>
{
    public float Value { get; set; } = value;


    public static implicit operator float(Float @float) => @float.Value;
    public static implicit operator Float(float @float) => new(@float);

    
    public readonly AnimationState AnimateThis(Float to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Value(Value, to, time, easingType);

    public readonly Float ConvertAnimationValues(float[] values)
        => values.ToValue();

    public override readonly string ToString()
        => $"{Value}";
}