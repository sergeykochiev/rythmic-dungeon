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
        rythm.RythmStart();
    }

    public void CyclicStop()
    {
        rythm.RythmStop();
    }

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

    abstract public void CyclicOnStart();

    abstract public void CyclicOnCycle();

    abstract public bool CyclicIsAllowedToCycle();

    abstract public (bool, bool) CyclicShouldCycleNow();

    private void FixedUpdate()
    {
        if (!rythm.FixedUpdate()) return;
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