using UnityEngine;
using UnityEngine.SceneManagement;

public class END : MonoBehaviour
{

    public string SceneName;
    //  Swaps to another scene by name
    public void OnSwap()
    {
        SceneManager.LoadScene(SceneName);
        Debug.Log("Swapping to main");
    }

    //  Exits the game (works in build, stops play mode in Editor)
    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stops play mode in Editor
#else
        Application.Quit(); // quits the built game
#endif
    }
}