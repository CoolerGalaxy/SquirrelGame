using UnityEngine;

public class LawnMowerStartupTriggerController : MonoBehaviour
{
    public LawnMowerController lawnMowerController;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lawnMowerController.startup();
        }
        
    }
}
