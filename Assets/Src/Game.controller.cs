using UnityEngine;

public class GameController : MonoBehaviour
{
    public EnemySpawner enemySpawnerInstance;
    public Player playerInstance;
    public MainCamera mainCameraInstance;
    private RythmicTrait rythm;
    private int difficultyLevel = 0;
    private bool isRunning = false;

    private void Start()
    {
        transform.position.Set(0,0,0);
        transform.localScale.Set(
            Constants.MaxFieldSize,
            Constants.MaxFieldSize,
            Constants.MaxFieldSize
        );
        enemySpawnerInstance.SetPositionConstrains(
            Constants.MinFieldPos,
            Constants.MaxFieldPos
        );
        rythm = new RythmicTrait();
    }

    public void Run()
    {
        isRunning = true;
        rythm.RythmStart();
        enemySpawnerInstance.StartSpawning();
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    void UpdateDifficultyLevel()
    {
        if (difficultyLevel == Constants.MaxDifficultyLevel)
        {
            return;
        }
        if (rythm.GlobalCycles() % 5 != 0) return;
        difficultyLevel += 1;
        enemySpawnerInstance.UpdateDifficultyLevel(difficultyLevel);
    }

    public void Stop(bool killEnemies = false)
    {
        isRunning = false;
        rythm.RythmStop();
        enemySpawnerInstance.StopSpawning(killAll: killEnemies);
    }

    public void EndLose()
    {
        Stop(true);
        mainCameraInstance.Shake(Constants.GameEndCameraShake);
        // TODO: display death message and go back to main menu
    }

    void FixedUpdate()
    {  
        if (playerInstance.IsDead() && isRunning)
        {
            EndLose();
        }
        if(!rythm.FixedUpdate()) return;
        UpdateDifficultyLevel();
    }
}