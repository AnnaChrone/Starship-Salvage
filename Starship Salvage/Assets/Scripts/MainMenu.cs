using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scene assignment")]
    public string Scenename = "GameScene";

    [Header("Controls Canvas")]
    public GameObject ControlCanvas;


    public void SwapScene()
    {
        StartCoroutine(LoadSceneCleanly());
    }

    private IEnumerator LoadSceneCleanly()
    {
        Debug.Log($"Loading scene: {Scenename}");

        yield return null; 
        yield return Resources.UnloadUnusedAssets(); // free old assets

        Time.timeScale = 1f; 

        // Load the scene
        SceneManager.LoadScene(Scenename, LoadSceneMode.Single);
    }

    public void ShowControls()
    {
        ControlCanvas.SetActive(true);
    }

    public void HideControls()
    {
        ControlCanvas.SetActive(false);
    }
}
