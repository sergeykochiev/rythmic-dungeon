using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private ShakableTrait shake;
    private bool hasShaken = false;
    private Camera camera;
    private float targetOrthographicSize;
    private float initialZoomSize;
    private bool isAnimating = false;

    void Start()
    {
        shake = new ShakableTrait(transform);
        camera = GetComponent<Camera>();
        
        if (camera != null)
        {
            targetOrthographicSize = Constants.MaxFieldSize / 2f + 1f;
            initialZoomSize = targetOrthographicSize * 0.2f;
            camera.orthographicSize = initialZoomSize;
            
            Vector3 initialPosition = transform.position;
            initialPosition.x -= 2f;
            transform.position = initialPosition;
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

    public void StartGameZoomAnimation()
    {
        if (!isAnimating && camera != null)
        {
            StartCoroutine(ZoomOutAnimation());
        }
    }

    private IEnumerator ZoomOutAnimation()
    {
        isAnimating = true;
        float duration = 2.0f;
        float elapsedTime = 0f;
        float startSize = camera.orthographicSize;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(0f, 0f, startPosition.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            camera.orthographicSize = Mathf.Lerp(startSize, targetOrthographicSize, progress);
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }

        camera.orthographicSize = targetOrthographicSize;
        transform.position = targetPosition;
        isAnimating = false;
    }

    private void Update()
    {
        shake.Update();
    }
}