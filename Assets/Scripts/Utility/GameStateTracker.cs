using UnityEngine;
using GameProperties;

public class GameStateTracker : MonoBehaviour
{
    public Levels currentLevel;
    
    public bool gameComplete;
    
    public GameStateEventManager eventManager;

    private void Start()
    {
        // Game begins in the side yard
        gameComplete = false;
        currentLevel = Levels.sideYard;
    }

    public void CompleteLevel()
    {
        // CompleteLevel will be invoked by Sid, usually from the AcornCollector script
        eventManager.TriggerLevelTransition(currentLevel);
        currentLevel++;
    }

    public void CompleteGame()
    {
        gameComplete = true;
        eventManager.TriggerVictory();
    }

    public void PlayerDeath()
    {
        eventManager.TriggerDeath();
    }
}
