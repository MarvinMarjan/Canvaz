using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public interface IAnimateable<T>
{
    AnimationState AnimateThis(T to, float time, EasingType easingType = EasingType.Linear);
}