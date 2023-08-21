using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class VictoryMenuManager : MonoBehaviour
{
    public GameStateTracker gameStateTracker;
    private CanvasGroup canvasGroup;

    public void displayVictory()
    {
        PauseGame(alterTime: false);
    }

    public void ResumeGame()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
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
