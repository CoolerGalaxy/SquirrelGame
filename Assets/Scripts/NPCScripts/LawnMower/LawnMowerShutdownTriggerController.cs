using UnityEngine;

public class LawnMowerShutdownTriggerController : MonoBehaviour
{
    public LawnMowerController lawnMowerController;
    
    void OnTriggerEnter()
    {
        lawnMowerController.shutdown();
    }
}
