using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortality : MonoBehaviour
{
    public GameStateTracker gameStateTracker;
    public bool sidIsDead = false;
    private void OnCollisionEnter(Collision collision)
    {
        SidKiller sk = collision.gameObject.GetComponent<SidKiller>();
        if (sk)
        {
            sidIsDead = true;
            gameStateTracker.PlayerDeath();
        }
    }
}