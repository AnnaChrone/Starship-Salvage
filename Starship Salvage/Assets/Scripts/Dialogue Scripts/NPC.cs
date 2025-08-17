using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable //NPC is an interactable
{
    public NPCDialogue dialogueData; //Calls from NPCDialogue class
    private DialogueController dialogueControl; //Calls from DialogueControoler class
    public Item item; //Calls from Item class

    private int dialogueIndex; //Index of lines
    private bool isTyping, isDialogueActive;
    private enum QuestState {NotStarted, InProgress, Completed} //States of quests
    private QuestState questState = QuestState.NotStarted; //Initial QuestState
    public bool isFrozen = false; //Pauses game
    public Hotbar hotbar; //Calls the hotbar

    public void Start()
    {
        dialogueControl = DialogueController.Instance; //Create an instance
    }
    public bool CanInteract()
    {
        return !isDialogueActive; //If we can interact with NPC, return that dialogye is not active
    }

    public void Interact()
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isFrozen = true; //Pauses game so that player does not run away from NPC
        
        SyncQuestState(); //Sync dialogue depending on state of quest

        //Set dialogue line based on quest state
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgressIndex; //There is a specific index for what dialogue to display
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;

        dialogueControl.SetNPCInfo(dialogueData.npcName);
        dialogueControl.ShowDialoguePanel(true);
        DisplayCurrentLine();
        
    }

    private void SyncQuestState()
    {
        if (dialogueData.quests == null)
        {
            return;
        }

        string questID = dialogueData.quests.QuestID; //Quest ID to verify quest state

        if (QuestController.Instance.IsQuestActive(questID))
    {
        if (hotbar.hasItem(questID)) //if the item in the hot basr is the same in the quest id, it is complete
        {
            questState = QuestState.Completed;
            QuestController.Instance.CompleteQuest(questID);
        }
        else
        {
            questState = QuestState.InProgress;
        }
    }
    else
    {
        questState = QuestState.NotStarted;
    }

    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueControl.SetDialogue(dialogueData.Lines[dialogueIndex]); //Type out all the lines
            isTyping = false;
        }

        //Clear choices
        dialogueControl.ClearChoices();

        //CheckendDialogueLines
        if (dialogueData.endLines.Length > dialogueIndex && dialogueData.endLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        //Check if choices and display
        foreach (DialogueChoice dialogueChoice in dialogueData.Choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }


        if (++dialogueIndex < dialogueData.Lines.Length)
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
        dialogueControl.SetDialogue("");

        foreach (char letter in dialogueData.Lines[dialogueIndex]) //Types out line one char at a time
        {
            dialogueControl.SetDialogue(dialogueControl.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine(); //If the line is done typing then it will pause and continue with the next line
        }

    }

     void DisplayChoices(DialogueChoice choice)//Displays the choice buttons
    {
        for (int i = 0; i < choice.Choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool givesQuest = choice.givesQuest[i];
            dialogueControl.CreateChoiceButton(choice.Choices[i], () => ChooseOption(nextIndex, givesQuest));
        }
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest) //If the index is a quest, save whether the player accepts it or not
        {
            QuestController.Instance.AcceptQuest(dialogueData.quests);
            questState = QuestState.InProgress;
        }
        
        dialogueIndex = nextIndex;//Next line
        dialogueControl.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines(); //Forces everything to stop running so that text can write
        StartCoroutine(TypeLine());
    }
    public void EndDialogue()
    {
        Debug.Log("End");
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueControl.SetDialogue("");
        dialogueControl.ShowDialoguePanel(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isFrozen = false;
    }
}
