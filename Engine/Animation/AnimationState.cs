using System;

namespace Canvaz.Engine.Animation;


public class AnimationUpdatedEventArgs(float[] currentValues) : EventArgs
{
    public float[] CurrentValues { get; set; } = currentValues;
}


public class AnimationState(float[] startValues, float[] endValues, float time)
{
    public float[] StartValues { get; init; } = startValues;
    public float[] EndValues { get; init; } = endValues;
    public float[] CurrentValues { get; private set; } = new float[startValues.Length];

    public float Time { get; init; } = time;
    public float ElapsedTime { get; private set; }

    public float Progress { get; private set; }

    public bool HasFinished => ElapsedTime >= Time;


    public event EventHandler<AnimationUpdatedEventArgs>? Updated;
    public event EventHandler? Finished;


    protected virtual void OnUpdated(AnimationUpdatedEventArgs eventArgs)
    {
        ElapsedTime += EngineApp.DTSeconds;
        Progress = ElapsedTime / Time;

        for (int i = 0; i < StartValues.Length; i++)
            CurrentValues[i] = StartValues[i] + (EndValues[i] - StartValues[i]) * Progress;

        if (HasFinished)
            eventArgs.CurrentValues = CurrentValues = EndValues;

        Updated?.Invoke(this, eventArgs);
    }

    protected virtual void OnFinished(EventArgs eventArgs)
        => Finished?.Invoke(this, eventArgs);


    public void Update()
    {
        if (HasFinished)
            return;

        OnUpdated(new(CurrentValues));

        if (HasFinished)
            OnFinished(EventArgs.Empty);
    }
}