using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private ShakableTrait shake;

    void Start()
    {
        shake = new ShakableTrait(transform);
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