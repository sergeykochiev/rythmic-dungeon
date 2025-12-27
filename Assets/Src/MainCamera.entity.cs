using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private ShakableTrait shake;

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
        shake.InitShake(shakePower);
    }

    private void Update()
    {
        shake.Update();
    }
}