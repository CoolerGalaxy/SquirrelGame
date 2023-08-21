using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject menuUI;
    public GameObject defaultButton;
    public CameraController cameraController;

    public AudioSource audioSource;
    public AudioClip nextClip;

    public InputManager InputManager;
    private InputManager.InputMode previousInputMode;

    void Update()
    {
        if (gameIsPaused)
        {
            HighlightMenuButton();
        }
    }

    public void Resume()
    {
        cameraController.canMove = true;

        menuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        cameraController.canMove = false;

        audioSource.Stop();  
        audioSource.clip = nextClip;  
        audioSource.Play();  

        menuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MAIN_SCENE");
        Cursor.lockState = CursorLockMode.Locked;
        Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Game_Menu_Scene");
        Cursor.lockState = CursorLockMode.None;
        Resume();
    }

    public void ContinueExploring()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Resume();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void HighlightMenuButton()
    {
        if (InputManager.inputMode != previousInputMode){
            previousInputMode = InputManager.inputMode;
            if (InputManager.inputMode == InputManager.InputMode.Gamepad)
            {
                EventSystem.current.SetSelectedGameObject(defaultButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
