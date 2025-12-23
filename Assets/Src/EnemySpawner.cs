using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemySpawner : Spawner<Enemy>
{
    private int posMin;
    private int posMax;
    private int difficultyLevel;
    public Player playerInstance;

    // public override bool CyclicShouldStop()
    // {
    //     return false;
    // }

    private Enemy[] GetEnemies()
    {
        return FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }

    public override bool CyclicIsAllowedToCycle()
    {
        return GetEnemies().Length < (difficultyLevel * 5);
    }

    public override (bool, bool) CyclicShouldCycleNow()
    {
        return (GetEnemies().Length == 0, true);
    }

    public void StopSpawning(bool killAll = false)
    {
        CyclicStop();
        if (!killAll) return;
        foreach (Enemy enemy in GetEnemies())
        {
            enemy.Die();
        }
    }

    public void UpdateDifficultyLevel(int difficultyLevel)
    {
        this.difficultyLevel = difficultyLevel;
    }

    // private void UpdateEnemySpawnRate()
    // {
    //     CyclicUpdateInterval(7 - (int)Mathf.Ceil(difficultyLevel * 7 / Constants.MaxDifficultyLevel));
    // }

    public void SetPositionConstrains(int min, int max)
    {
        posMin = min;
        posMax = max;
    }

    public override Vector3 GetInstantiatePosition()
    {
        return (Vector3)new Vector2(
            Random.Range(posMin, posMax + 1),
            Random.Range(posMin, posMax + 1)
        );
    }

    public override Quaternion GetInstatiateRotation()
    {
        return Quaternion.identity;
    }

    public override void CyclicOnStart()
    {
        CyclicUpdateInterval(2);
    }

    private bool CoordinateWithinDistance(float coordinate, float point, float distance)
    {
        return coordinate + distance > point || coordinate - distance < point;
    }

    private bool PositionWithinRadius(Vector2 position, Vector2 point, float radius)
    {
        return CoordinateWithinDistance(position.x, point.x, radius) &&
            CoordinateWithinDistance(position.y, point.y, radius);
    }

    public override bool ShouldSpawn(Vector2 position, Quaternion rotation)
    {
        Vector2 playerPos = playerInstance.transform.position;
        return PositionWithinRadius(playerPos, position, 2);
    }

    public override void InitInstance(Enemy instance)
    {
        instance.SetDifficultyLevel(difficultyLevel);
        instance.SetPlayerInstance(playerInstance);
    }
}