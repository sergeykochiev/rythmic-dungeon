using UnityEngine;

public class ScalableTrait : AnimatedTrait 
{
    private readonly Transform transform;
    private Vector3? targetScale;

    public ScalableTrait(Transform transform, float speed = 1f)
    {
        this.transform = transform;
        SetAnimationSpeed(speed);
    }

    public void InitScale(Vector3 targetScale)
    {
        this.targetScale = targetScale;
        AnimationStart();
    }

    public override bool IsAnimationEndConditionMet()
    {
        return transform.localScale == targetScale;
    }

    public override void OnAnimationEnd()
    {
        targetScale = null;
    }

    public override void OnAnimationStart()
    {
    }

    public override void OnAnimationStep(float animationSpeed)
    {
        if (!targetScale.HasValue)
        {
            AnimationAbort();
            return;
        }
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            targetScale.Value,
            animationSpeed
        );
    }
}