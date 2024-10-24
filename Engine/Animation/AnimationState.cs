using System;


namespace Canvaz.Engine.Animation;


public class AnimationUpdatedEventArgs(float[] currentValues) : EventArgs
{
    public float[] CurrentValues { get; set; } = currentValues;
}


/// <summary>
/// Represents an animation. <br/>
/// Note that this class works with an array of floats. The reason for that
/// it's because it's an approach for animating structures that have more than one
/// data property. For example, animating a single float number isn't a problem... but
/// what about a Color or a Vector2f? these have more than one value inside of them.
/// By using an array of floats, animating structures like these becomes easier and
/// more efficient that other ways. Each array index represents a property member of the
/// structure: <br/><br/>
/// 
/// In case of Vector2f: new AnimationState(startVec.X, startVec.Y], [endVec.X, endVec.Y], 1f); <br/>
/// In case of Color: new AnimationState([startColor.R, startColor.G, startColor.B], [endColor.R, endColor.G, endColor.B], 1f);
/// 
/// And so on...
/// </summary>
/// <param name="startValues"> The start values. </param>
/// <param name="endValues"> The final values. </param>
/// <param name="time"> The time the animation will take to finish. </param>
public class AnimationState(float[] startValues, float[] endValues, float time, EasingType easingType = EasingType.Linear)
{
    public float[] StartValues { get; init; } = startValues;
    public float[] EndValues { get; init; } = endValues;
    public float[] CurrentValues { get; private set; } = new float[startValues.Length];

    public float Time { get; init; } = time;
    public float ElapsedTime { get; private set; }

    public EasingType EasingType { get; init; } = easingType;

    /// <summary>
    /// How much the animation has progressed from 0 to 1.
    /// </summary>
    public float Progress { get; private set; }

    /// <summary>
    /// Same as this.Progress, but with eased by this.EasingType.
    /// </summary>
    public float EasedProgress { get; private set; }

    public bool HasFinished => ElapsedTime >= Time;


    public event EventHandler<AnimationUpdatedEventArgs>? Updated;
    public event EventHandler? Finished;


    protected virtual void OnUpdated(AnimationUpdatedEventArgs eventArgs)
    {
        ElapsedTime += EngineApp.DTSeconds;
        Progress = ElapsedTime / Time;

        EasedProgress = EasingFunctions.Ease(Progress, EasingType);

        for (int i = 0; i < StartValues.Length; i++)
            CurrentValues[i] = StartValues[i] + (EndValues[i] - StartValues[i]) * EasedProgress;

        if (HasFinished)
            eventArgs.CurrentValues = CurrentValues = EndValues;

        Updated?.Invoke(this, eventArgs);
    }

    protected virtual void OnFinished(EventArgs eventArgs)
        => Finished?.Invoke(this, eventArgs);


    /// <summary>
    /// Updates the animation.
    /// </summary>
    public void Update()
    {
        if (HasFinished)
            return;

        OnUpdated(new(CurrentValues));

        if (HasFinished)
            OnFinished(EventArgs.Empty);
    }
}