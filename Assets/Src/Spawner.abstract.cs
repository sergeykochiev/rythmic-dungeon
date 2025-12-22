using UnityEngine;

abstract public class Spawner<T> : CyclicBehaviour where T : Component
{
    public T prefub;
    abstract public void InitInstance(T instance);
    abstract public Vector3 GetInstatiatePosition();
    abstract public Quaternion GetInstatiateRotation();

    public void SetSpawnRate(int spawnrate)
    {
        CyclicUpdateInterval(spawnrate);
    }
    public void StartSpawning()
    {
        CyclicStart();
    }
    public override void CyclicOnCycle()
    {
        T instance = Instantiate(
            prefub,
            GetInstatiatePosition(),
            GetInstatiateRotation()
        );
        InitInstance(instance);
    }

}