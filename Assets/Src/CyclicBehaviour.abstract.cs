using UnityEngine;

abstract public class CyclicBehaviour : MonoBehaviour
{
    private int cycleInterval = 1;
    private RythmicTrait rythm;

    private void Start()
    {
        rythm = new RythmicTrait();
        CyclicOnStart();
    }

    public void CyclicStart()
    {
        rythm.ResetCycles();
        rythm.RythmStart();
    }

    public void CyclicStop()
    {
        rythm.RythmStop();
    }

    // abstract public bool CyclicShouldStop();

    public int CyclesLeftToWait()
    {
        return cycleInterval - rythm.CyclesPassed();
    }

    public void CyclicUpdateInterval(int cycleInterval)
    {
        this.cycleInterval = cycleInterval;
    }

    public void CyclicNow(bool resetCycles = true)
    {
        CyclicOnCycle();
        if (resetCycles) rythm.ResetCycles();
    }

    public int CyclicCyclesPassed()
    {
        return rythm.CyclesPassed();
    }

    abstract public void CyclicOnStart();

    abstract public void CyclicOnCycle();

    abstract public bool CyclicIsAllowedToCycle();

    abstract public (bool, bool) CyclicShouldCycleNow();

    private void FixedUpdate()
    {
        if (!rythm.FixedUpdate()) return;
        // if (CyclicShouldStop())
        // {
        //     CyclicStop();
        //     return;
        // }
        var (shouldCycleNow, shouldResetCycle) = CyclicShouldCycleNow();
        if (shouldCycleNow)
        {
            CyclicOnCycle();
            if (shouldResetCycle) rythm.ResetCycles();
            return;
        }
        if (rythm.CyclesPassed() != cycleInterval) return;
        if (!CyclicIsAllowedToCycle()) return;
        CyclicOnCycle();
        rythm.ResetCycles();
    }
}