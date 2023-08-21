using UnityEngine;

public class MouseController : MonoBehaviour
{
    NPCDialogController npcDialogController;
    
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject eventTrigger = GameObject.Find("Event Trigger");
        npcDialogController = eventTrigger.GetComponent<NPCDialogController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (npcDialogController.tutorialIsActive)
        {
            animator.SetBool("waving", false);
        }
        else 
        {
            animator.SetBool("waving", true);
        }
    }
}
