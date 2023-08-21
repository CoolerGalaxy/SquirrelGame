using UnityEngine;
using GameProperties;

public class GameStateEventManager : MonoBehaviour
{
    public GateController sideYardGateOpener;
    public GateController yardOneGateOpener;
    public GateController yardTwoGateOpener;
    public VictoryMenu victoryMenu;
    public DeathMenu deathMenu;
    public void TriggerLevelTransition(Levels completedLevel)
    {
        /* TriggerLevelTransition should be used as a wrapper for anything that needs to happen upon the completion of any level
        Input completedLevel will be a member of the Levels enum from GameProperties, and represents the level that has just been
        completed. */

        switch (completedLevel)
        {
            case Levels.sideYard:
                // Trigger all events that should occur upon completion of the sideYard level
                sideYardGateOpener.OpenGate();
                break;
            case Levels.yardOne:
                // Trigger all events that should occur upon completion of the yardOne level
                yardOneGateOpener.OpenGate();
                break;
            case Levels.yardTwo:
                // Trigger all events that should occur upon completion of the yardTwo level
                yardTwoGateOpener.OpenGate();
                break;
        }
    }

    public void TriggerVictory()
    {
        victoryMenu.Pause();
    }

    public void TriggerDeath()
    {
        deathMenu.Pause();
    }
}
