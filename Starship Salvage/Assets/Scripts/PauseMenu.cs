using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public FPController controller;
    public GameObject Menu;
    public GameObject Controls;
    
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");

        // This will quit the application (only works in builds, not in the Unity editor)
        Application.Quit();

        // If you're in the editor and want to simulate exit:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ReturnToGame()
    {
        controller.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {

    }

    public void OpenControls()
    {
        Controls.SetActive(true);
    }

    public void BacktoPause()
    {
        Controls.SetActive(false);
    }
}
