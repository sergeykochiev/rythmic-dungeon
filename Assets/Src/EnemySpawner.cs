using UnityEngine;

public class EnemySpawner : Spawner<Enemy>
{
    private int posMin;
    private int posMax;
    private int difficultyLevel;
    public Player playerInstance;

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
        UpdateEnemySpawnRate();
    }

    private void UpdateEnemySpawnRate()
    {
        CyclicUpdateInterval((int)Mathf.Ceil((Constants.MaxDifficultyLevel + 1 - difficultyLevel) / 3));
    }

    public void SetPositionConstrains(int min, int max)
    {
        posMin = min;
        posMax = max;
    }

    public override Vector3 GetInstatiatePosition()
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
    }

    public override void InitInstance(Enemy instance)
    {
        instance.SetDifficultyLevel(difficultyLevel);
        instance.SetPlayerInstance(playerInstance);
    }
}