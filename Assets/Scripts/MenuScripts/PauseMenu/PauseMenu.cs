using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject defaultButton;
    public CameraController cameraController;
    public LevelRestarter levelRestarter;

    public InputManager InputManager;
    private InputManager.InputMode previousInputMode;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        levelRestarter = FindObjectOfType<LevelRestarter>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (gameIsPaused)
        {
            HighlightMenuButton();
        }
    }

    public void Resume()
    {
        cameraController.canMove = true;

        AudioListener.volume = 1f;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        cameraController.canMove = false;

        AudioListener.volume = 0f;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        levelRestarter.LoadCheckpoint();
        Cursor.lockState = CursorLockMode.Locked;
        Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Game_Menu_Scene");
        Cursor.lockState = CursorLockMode.None;
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
