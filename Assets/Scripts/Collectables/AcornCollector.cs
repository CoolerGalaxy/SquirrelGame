using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProperties;

public class AcornCollector : MonoBehaviour
{
    public int sideYardCollectedAcorns;
    
    public int yardOneCollectedAcorns;
    
    public int yardTwoCollectedAcorns;
    
    public GameObject gameStateTracker;
    
    public AudioClip acornCollectionSound;

    public void CollectAcorn(Levels collectableLevelLocation)
    {
        AudioSource.PlayClipAtPoint(acornCollectionSound, transform.position);
        GameStateTracker gameState = gameStateTracker.GetComponent<GameStateTracker>();
        switch (collectableLevelLocation)
        {
            case Levels.sideYard:
                sideYardCollectedAcorns++;
                if (sideYardCollectedAcorns >= GameConstants.sideYardTotalAcorns)
                {
                    gameState.CompleteLevel();
                }
                break;
            
            case Levels.yardOne:
                yardOneCollectedAcorns++;
                if (yardOneCollectedAcorns >= GameConstants.yardOneTotalAcorns)
                {
                    gameState.CompleteLevel();
                }
                break;
            
            case Levels.yardTwo:
                yardTwoCollectedAcorns++;
                if (yardTwoCollectedAcorns >= GameConstants.yardTwoTotalAcorns)
                {
                    gameState.CompleteLevel();
                }
                break;
        }
    }
}
