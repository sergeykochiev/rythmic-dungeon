using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private ShakableTrait shake;
    private bool hasShaken = false;

    void Start()
    {
        shake = new ShakableTrait(transform);
        
        if (TryGetComponent<Camera>(out var camera))
        {
            camera.orthographicSize = Constants.MaxFieldSize / 2f + 1f;
        }
    }

    public void Shake(float shakePower)
    {   
        if (!hasShaken) {
            shake.InitShake(shakePower);
            hasShaken = true;
        }
    }

    public void ResetShake()
    {
        hasShaken = false;
    }

    private void Update()
    {
        shake.Update();
    }
}