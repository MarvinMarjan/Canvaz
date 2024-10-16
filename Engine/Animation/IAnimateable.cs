namespace Canvaz.Engine.Animation;


/// <summary>
/// Describes an object that can be animated.
/// </summary>
public interface IAnimateable<T>
{
    AnimationState AnimateThis(T to, float time, EasingType easingType = EasingType.Linear);

    T ConvertAnimationValues(float[] values);
}