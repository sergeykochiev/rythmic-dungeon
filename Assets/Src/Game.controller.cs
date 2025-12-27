using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public BeamPowerController beamPowerInstance;
    public EnemySpawner enemySpawnerInstance;
    public Player playerInstance;
    public MainCamera mainCameraInstance;
    private RythmicTrait rythm;
    public int initialDifficulty = 1;
    public int difficultyLevel = 0;
    private bool isRunning = false;

    private void Start()
    {
        transform.position.Set(0,0,0);
        transform.localScale.Set(
            Constants.MaxFieldSize,
            Constants.MaxFieldSize,
            0
        );
        enemySpawnerInstance.SetPositionConstrains(
            Constants.MinFieldPos,
            Constants.MaxFieldPos
        );
        rythm = new RythmicTrait();
        
        MainMenuController mainMenu = FindFirstObjectByType<MainMenuController>();
        if (mainMenu == null)
        {
            Run();
        }
    }

    public void Run()
    {
        isRunning = true;
        rythm.RythmStart();
        enemySpawnerInstance.CyclicStart();
        playerInstance.SetPower(beamPowerInstance);
        difficultyLevel = initialDifficulty;
        mainCameraInstance.StartGameZoomAnimation();
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
        StartCoroutine(DeathAnimation());
    }
    
    private System.Collections.IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Color originalColor = mainCamera.backgroundColor;
            mainCamera.backgroundColor = Color.red;
            yield return new WaitForSeconds(0.2f);
            mainCamera.backgroundColor = originalColor;
            yield return new WaitForSeconds(0.1f);
            mainCamera.backgroundColor = Color.red;
            yield return new WaitForSeconds(0.2f);
            mainCamera.backgroundColor = originalColor;
        }
        
        MainMenuController mainMenu = FindFirstObjectByType<MainMenuController>();
        if (mainMenu != null)
        {
            mainMenu.ShowGameOver();
        }
    }

    void FixedUpdate()
    {  
        if (playerInstance.IsDead() && isRunning)
        {
            isRunning = false;
            EndLose();
        }
        if(!rythm.FixedUpdate()) return;
        UpdateDifficultyLevel();
    }
}