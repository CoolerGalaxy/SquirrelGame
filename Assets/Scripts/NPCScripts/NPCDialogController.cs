using UnityEngine;

public class NPCDialogController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject dialogCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform standingPoint;
    public bool tutorialIsActive = false;
    public bool jumpEnabled;
    private bool tutorialCompleted = false;

    private void Start()
    {
        jumpEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && tutorialCompleted == false)
        {
            Transform avatar = other.transform;
            
            // Teleport the avatar to standing point
            avatar.position = standingPoint.position;
            avatar.rotation = standingPoint.rotation;
            
            // Disable main camera
            mainCamera.SetActive(false);
            dialogCamera.SetActive(true);
            canvas.SetActive(true);
            
            tutorialIsActive = true;
            jumpEnabled = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        mainCamera.SetActive(true);
        dialogCamera.SetActive(false);
        canvas.SetActive(false);
        
        tutorialIsActive = false;
        jumpEnabled = true;
        tutorialCompleted = true;
    }
}
