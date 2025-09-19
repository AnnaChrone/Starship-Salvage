using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class Spaceship : MonoBehaviour
{
    private SpaceshipDialogue shipDialogueData; //Calls from NPCDialogue class
    private SpaceshipAIController shipDialogueControl; //Calls from DialogueControoler class
    public bool finishedDialogue = false;

    private int dialogueIndex; //Index of lines
    private bool isTyping, isDialogueActive ;
    public GameObject CrashLandingScene;
    public float fadeDuration =3f;
    public CanvasGroup panel;
    public CanvasGroup shader;
    public bool isFrozen = false; //Pauses game
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipDialogueControl = SpaceshipAIController.Instance; //Create an instance
       
    }

    public void StartDialogue(SpaceshipDialogue Andromeda, int dialogueIndex)
    {

        // if (isDialogueActive) return;


        
        shipDialogueData = Andromeda;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isFrozen = true; //Pauses game so that player does not run away from NPC

        isDialogueActive = true;


        shipDialogueControl.ShowDialoguePanel(true);
        shipDialogueControl.SetShipInfo(shipDialogueData.shipName);
        DisplayCurrentLine();
    }

     

        public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            shipDialogueControl.SetDialogue(shipDialogueData.Lines[dialogueIndex]); //Type out all the lines
            isTyping = false;
        }

        //CheckendDialogueLines
        if (shipDialogueData.endLines.Length > dialogueIndex && shipDialogueData.endLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }


        if (++dialogueIndex < shipDialogueData.Lines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

   
       IEnumerator TypeLine()
    {
        isTyping = true;
        shipDialogueControl.SetDialogue("");

        foreach (char letter in shipDialogueData.Lines[dialogueIndex]) //Types out line one char at a time
        {
            shipDialogueControl.SetDialogue(shipDialogueControl.dialogueText.text += letter);
            yield return new WaitForSeconds(shipDialogueData.typingSpeed);
        }

        isTyping = false;

        if (shipDialogueData.autoProgressLines.Length > dialogueIndex && shipDialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(shipDialogueData.autoProgressDelay);
            NextLine(); //If the line is done typing then it will pause and continue with the next line
        }

    }
    void DisplayCurrentLine()
    {
        StopAllCoroutines(); //Forces everything to stop running so that text can write
        StartCoroutine(TypeLine());
    }
    public void EndDialogue()
    {
        Debug.Log("im ending dialogue");
        StopAllCoroutines();
        shipDialogueControl.SetDialogue("");
        finishedDialogue = true;
        if (CrashLandingScene != null)
        {
            StartCoroutine(FadeOutPanel());
        }
        shipDialogueControl.ShowDialoguePanel(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isFrozen = false;
    }

    IEnumerator FadeOutPanel()
    {
        float startAlpha = panel.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            panel.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            shader.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            yield return null;
        }

        CrashLandingScene.SetActive(false);
    }
}
