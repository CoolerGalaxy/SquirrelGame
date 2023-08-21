using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class DeathMenuManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public void displayDeath()
    {
        PauseGame();
    }

    public void PauseGame(bool alterTime = true)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (alterTime)
        {
            Time.timeScale = 0f;
        }
        Cursor.lockState = CursorLockMode.None;
    }
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("GetComponent() cannot find the component you are looking for.");
        }
    }
}
