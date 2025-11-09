using UnityEngine;

public class CookButtons: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Instructions;
    public GameObject Minigame;
    public GameObject LoseScreen;
    public GameObject CloseGame;
    public AudioSource Music;
    public AudioSource RaLuMusic;
    public GameObject HUD;
    public GameObject Camera;
    public SpawnItems Reset;
    public GameObject Spoon;
    public void startCooking()
    {
        Instructions.SetActive(false);
        Minigame.SetActive(true);
    }

    public void WinButton()
    {
        CloseGame.SetActive(false);
        Music.mute = true;
        Camera.SetActive(true);
        HUD.SetActive(true);
        Spoon.SetActive(true);

    }

    public void Retry()
    {
        LoseScreen.SetActive(false);
        Reset.ResetMinigame();
    }
}
