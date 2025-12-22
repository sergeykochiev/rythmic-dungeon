using UnityEngine;

public class ShakableTrait
{
    private readonly Transform transform;
    private static readonly float baseShakeAngle = 25f;
    private Quaternion? targetRotation;
    private float shakePower = 0;
    private readonly float rotationSpeed = 0.5f;
    private bool isRotating = false;
    private Quaternion prevRotation = Quaternion.Euler(-1,0,0);

    public ShakableTrait(Transform transform)
    {
        this.transform = transform;
    }

    public void InitShake(float shakePower)
    {   
        if (shakePower > 100) shakePower = 100;
        this.shakePower = shakePower;
    }

    private void AdvanceRotation()
    {
        if (targetRotation == null) return;
        prevRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation.Value,
            rotationSpeed
        );
        if (transform.rotation == prevRotation)
        {
            targetRotation = null;
            isRotating = false;
        }
    }

    private void NextShakeRotation()
    {
        shakePower--;
        if (shakePower == 1) {
            targetRotation = Quaternion.Euler(0,0,0);
        } else
        {
            targetRotation = Quaternion.Euler(0,0,Random.Range(
                -baseShakeAngle * shakePower / 100,
                baseShakeAngle * shakePower / 100
            ));
        }
        isRotating = true;
    }

    public void Update()
    {
        if (shakePower == 0) return;
        if (!isRotating)
        {
            NextShakeRotation();
        }
        AdvanceRotation();
    }
}