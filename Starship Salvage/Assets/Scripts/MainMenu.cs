using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scene assignment")]
    public string Scenename = "GameScene";

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
}
