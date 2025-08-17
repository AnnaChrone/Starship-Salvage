using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class SpaceshipAIController : MonoBehaviour
{
    public static SpaceshipAIController Instance { get; private set; } //Singleton instance allows us to call the scc=ripts from any where
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); //Make sure only one instance of the dialogue controller 
    }

    public void ShowDialoguePanel(bool show)
    {
        dialoguePanel.SetActive(show); //Toggle panel visibility
    }

    public void SetShipInfo()
    {
        nameText.text = "Spaceship";
    }

    public void SetDialogue(string text)
    {
        dialogueText.text = text;
    }

}
