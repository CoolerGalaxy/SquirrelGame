using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenuButtonController : MonoBehaviour
{
    public MusicController musicController;
    public GameObject mainMenuCanvas;
    public GameObject creditsCanvas;
    public GameObject startButton;
    public GameObject creditsExitButton;
    public InputManager InputManager;
    private InputManager.InputMode previousInputMode;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        previousInputMode = InputManager.InputMode.KeyboardMouse;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Update()
    {
        if (InputManager.inputMode != previousInputMode){
            previousInputMode = InputManager.inputMode;
            if (InputManager.inputMode == InputManager.InputMode.Gamepad)
            {
                if (mainMenuCanvas.activeInHierarchy == true){
                    EventSystem.current.SetSelectedGameObject(startButton);
                }
                else if (creditsCanvas.activeInHierarchy == true){
                    EventSystem.current.SetSelectedGameObject(creditsExitButton);
                }
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
    
    public void StartButtonClick()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        musicController.ChangeMusic();

        while (musicController.IsTransitioning)
        {
            yield return null;
        }

        SceneManager.LoadScene("MAIN_SCENE");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StartCredits()
    {
        mainMenuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);

        if (InputManager.inputMode == InputManager.InputMode.Gamepad)
        {
            EventSystem.current.SetSelectedGameObject(creditsExitButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void StartMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);

        if (InputManager.inputMode == InputManager.InputMode.Gamepad)
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
