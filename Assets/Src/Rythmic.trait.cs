using UnityEngine;

public class RythmicTrait
{
    public const float DefaultRythmInterval = 1f;
    public readonly float rythmIntervalSeconds;
    private int cyclesPassed = 0;
    private int globalCycles = 0;
    private float timePassed = 0f;
    private bool isStopped = true;

    public RythmicTrait(float rythmIntervalSeconds = DefaultRythmInterval)
    {
        this.rythmIntervalSeconds = rythmIntervalSeconds;
    }

    public int CyclesPassed()
    {
        return cyclesPassed;
    }

    public int GlobalCycles()
    {
        return globalCycles;
    }

    public float TimePassed()
    {
        return timePassed;
    }

    public void RythmStop()
    {
        isStopped = true;
    }

    public void RythmStart()
    {
        isStopped = false;
    }

    public bool IsRythmStopped()
    {
        return isStopped;
    }

    public void ResetCycles()
    {
        cyclesPassed = 0;    
    }

    public bool FixedUpdate()
    {
        if (isStopped) return false;
        if (timePassed < rythmIntervalSeconds)
        {
            timePassed += Time.deltaTime;
            return false;
        }
        timePassed = 0;
        cyclesPassed++;
        globalCycles++;
        return true;
    }
}