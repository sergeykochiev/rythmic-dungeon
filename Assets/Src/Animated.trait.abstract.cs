abstract public class AnimatedTrait
{
    private float animationSpeed = 0.5f;
    private bool isAnimating = false;

    public void SetAnimationSpeed(float animationSpeed)
    {
        this.animationSpeed = animationSpeed;
    }

    public void AnimationStart()
    {   
        OnAnimationStart();
        isAnimating = true;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }

    abstract public void OnAnimationStart();

    abstract public bool IsAnimationEndConditionMet();

    abstract public void OnAnimationEnd();

    private void AnimationStep()
    {
        if (IsAnimationEndConditionMet())
        {
            AnimationAbort();
            OnAnimationEnd();
            return;
        }
        OnAnimationStep(animationSpeed);
    }

    public void AnimationAbort()
    {
        isAnimating = false;
    }

    abstract public void OnAnimationStep(float animationSpeed);

    public void Update()
    {
        if (!isAnimating) return;
        AnimationStep();
    }
}