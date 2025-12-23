using UnityEngine;

abstract public class PowerController : CyclicBehaviour
{
    public Transform powerWielder;
    private bool isReady = false;
    abstract public void Init(Transform powerWielder);
    abstract public int GetUseInterval();

    public override void CyclicOnStart()
    {
        CyclicUpdateInterval(GetUseInterval());
    }

    public override void CyclicOnCycle()
    {
        isReady = true;
    }

    public bool IsReadyToUse()
    {
        return isReady;
    }

    public override bool CyclicIsAllowedToCycle()
    {
        return !isReady;
    }
    public void GoOnCooldown()
    {
        isReady = false;
        CyclicStart();
    }

    abstract public void OnUse();

    public void Use()
    {
        if (!IsReadyToUse()) return;
        OnUse();
        GoOnCooldown();
    }

    public override (bool, bool) CyclicShouldCycleNow()
    {
        return (false, false);
    }
}