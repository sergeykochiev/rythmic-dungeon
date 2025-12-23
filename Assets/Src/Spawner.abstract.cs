using UnityEngine;

abstract public class Spawner<T> : CyclicBehaviour where T : Component
{
    public T prefub;
    abstract public void InitInstance(T instance);
    abstract public Vector3 GetInstantiatePosition();
    abstract public Quaternion GetInstatiateRotation();
    abstract public bool ShouldSpawn(Vector2 position, Quaternion rotation);
    public override void CyclicOnCycle()
    {
        T instance = Instantiate(
            prefub,
            GetInstantiatePosition(),
            GetInstatiateRotation()
        );
        InitInstance(instance);
    }

}