using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Shapes.Properties;


/// <summary>
/// Represents a Canvaz.Object property that can be animated.
/// </summary>
public class Property<T> : IUpdateable
    where T : IAnimateable<T>
{
    /// <summary>
    /// The owner of this property.
    /// </summary>
    public Object Owner { get; set; }

    /// <summary>
    /// The current value of the property.
    /// </summary>
    public T Value { get; set; }


    private AnimationState? _animationState;


    public Property(Object owner, T value)
    {
        Owner = owner;
        Value = value;

        Owner.AddPropertyToUpdateQueue(this);
    }


    public static implicit operator T(Property<T> property) => property.Value;


    public void Set(T value)
        => Value = value;


    public void Animate(T to, float time, EasingType easingType = EasingType.Linear)
    {
        _animationState = Value.AnimateThis(to, time, easingType);

        _animationState.Updated += (obj, arg)
            => Value = Value.ConvertAnimationValues(arg.CurrentValues);
    }


    public void Update()
    {
        if (_animationState is not AnimationState validAnimationState)
            return;

        validAnimationState.Update();

        if (validAnimationState.HasFinished)
            _animationState = null;
    }
}