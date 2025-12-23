using UnityEngine;

public class BeamAnimation
{
    private readonly ScalableTrait scalable;
    private readonly MovableTrait movable;

    public BeamAnimation(Transform transform)
    {
        scalable = new ScalableTrait(transform, speed: 0.0005f);
        movable = new MovableTrait(transform, speed: 0.03f);
    }

    public bool IsFinished()
    {
        return !scalable.IsAnimating() && !movable.IsAnimating();
    }

    public void Run(Vector2 direction)
    {
        scalable.InitScale(Vector3.zero);
        movable.QueueMove(new(direction, Constants.MaxFieldSize));
        movable.InitMove();
    }

    public void Update()
    {
        if (scalable.IsAnimating()) scalable.Update();
        if (movable.IsAnimating()) movable.Update();
    }
}