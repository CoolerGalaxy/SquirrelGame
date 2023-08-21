using UnityEngine;

public class GameVictoryTrigger : MonoBehaviour
{
    public GameStateTracker gameStateTracker;
    public GameObject AcornTracker;

    private void OnTriggerEnter()
    {
        gameStateTracker.CompleteGame();
        AcornTracker.SetActive(false);
    }
    private void OnTriggerExit()
    {
        this.gameObject.SetActive(false);
    }
}
