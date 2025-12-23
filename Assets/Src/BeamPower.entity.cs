using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BeamPowerEntity : MonoBehaviour
{
    private BeamAnimation beamScaleAnimation;
    private bool isDestroyed = false;

    private void Start()
    {
        tag = "Power";
    }

    public void Blast(Vector2 direction)
    {
        beamScaleAnimation = new BeamAnimation(transform);
        beamScaleAnimation.Run(direction);
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        if (isDestroyed) return;
        if (beamScaleAnimation.IsFinished())
        {
            DestroySelf();
            return;
        }
        beamScaleAnimation.Update();
    }
}