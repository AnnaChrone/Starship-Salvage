using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Spaceship : MonoBehaviour, AIInteractable
{
    public SpaceshipDialogue shipDialogueData; //Calls from NPCDialogue class
    private SpaceshipAIController shipDialogueControl; //Calls from DialogueControoler class

    private int dialogueIndex; //Index of lines
    private bool isTyping, isDialogueActive;
    private enum ShipState {NotCompleted, NotFound, Part1} //State of ships completion
    private ShipState shipState = ShipState.NotCompleted;
    public Hotbar hotbar; //Calls the hotbar
    private SpaceshipFixing spaceshipFixing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipDialogueControl = SpaceshipAIController.Instance; //Create an instance
        spaceshipFixing = FindAnyObjectByType<SpaceshipFixing>();
    }

    public bool CanInteractAI()
    {
        return !isDialogueActive; //If we can interact with NPC, return that dialogye is not active
    }

    public void InteractAI()
    {
        
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;

        SyncShipState();

        if (shipState == ShipState.NotCompleted)
        {
            dialogueIndex = 0;
            Debug.Log("Not completed");
        }
        else if (shipState == ShipState.NotFound)
        {
            dialogueIndex = shipDialogueData.ShipPartNotFound;
            Debug.Log("Not found");
        }
        else if (shipState == ShipState.Part1)
        {
            dialogueIndex = shipDialogueData.ShipPartIndex;
            Debug.Log("found");
        }

        shipDialogueControl.ShowDialoguePanel(true);
        shipDialogueControl.SetShipInfo(shipDialogueData.shipName);
        DisplayCurrentLine();
    }

        void NextLine()
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

    private void SyncShipState()
    {
         string questID = shipDialogueData.quests.QuestID;

    if (QuestController.Instance.IsQuestCompleted(questID))
    {
        if (spaceshipFixing.TryUseItem())
        {
            shipState = ShipState.Part1;
        }
        else
        {
            shipState = ShipState.NotFound;
        }
    }
    else
    {
        shipState = ShipState.NotCompleted; // not accepted yet
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
        StopAllCoroutines();
        shipDialogueControl.SetDialogue("");
        shipDialogueControl.ShowDialoguePanel(false);
    }
}
