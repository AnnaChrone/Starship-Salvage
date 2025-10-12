using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public FPController controller;
    public GameObject Menu;
    public GameObject Controls;
    public AudioSource ButtonSound;
    
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");

        Application.Quit();

        // If you're in the editor this will run
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

    public void OpenControls()
    {
        Controls.SetActive(true);
    }

    public void PlaySound()
    {
        ButtonSound.Play();
    }
    public void BacktoPause()
    {
        Controls.SetActive(false);
    }
}
