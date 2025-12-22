using UnityEngine;

public class ScenesController : MonoBehaviour
{
    public GameController gameControllerInstance;

    private void Update()
    {
        if (!gameControllerInstance.IsRunning()) gameControllerInstance.Run();
    }
}