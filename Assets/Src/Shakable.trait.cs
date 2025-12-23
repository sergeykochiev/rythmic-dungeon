using System;
using UnityEngine;

public class ShakableTrait : AnimatedTrait
{
    private const float BaseShakeAngle = 25f;
    private const float DefaultShakingSpeed = 0.1f;
    private readonly Transform transform;
    private Quaternion? targetRotation;
    private float shakePower = 0;
    private Quaternion? prevRotation;

    public ShakableTrait(Transform transform, float shakingSpeed = DefaultShakingSpeed)
    {
        this.transform = transform;
        SetAnimationSpeed(shakingSpeed);
    }

    public void InitShake(float shakePower)
    {   
        if (shakePower > 100) shakePower = 100;
        this.shakePower = shakePower;
        AnimationStart();
    }

    public override void OnAnimationStart()
    {
        targetRotation = Quaternion.Euler(0,0,UnityEngine.Random.Range(
            -BaseShakeAngle * shakePower / 100,
            BaseShakeAngle * shakePower / 100
        ));
    }

    public override void OnAnimationStep(float animationSpeed)
    {
        prevRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation.Value,
            animationSpeed 
        );
    }

    public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2, float angleTolerance = 0.001f)
    {
        float dot = Mathf.Abs(Quaternion.Dot(q1, q2));
        dot = Mathf.Clamp(dot, -1.0f, 1.0f);
        float angle = Mathf.Acos(dot) * 2.0f * Mathf.Rad2Deg;
        return angle < angleTolerance;
    }

    public override bool IsAnimationEndConditionMet()
    {
        bool shouldEnd = prevRotation.HasValue && AreQuaternionsClose(prevRotation.Value, transform.rotation);
        return shouldEnd;
    }

    public override void OnAnimationEnd()
    {
        shakePower--;
        prevRotation = null;
        if (shakePower == 0) {
            targetRotation = null;
            transform.rotation = Quaternion.Euler(0,0,0);
            return;
        }
        AnimationStart();
    }
}