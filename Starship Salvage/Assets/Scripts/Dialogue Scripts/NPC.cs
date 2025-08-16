using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueControl;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    
    private enum QuestState {NotStarted, InProgress, Completed}
    private QuestState questState = QuestState.NotStarted;
    public bool isFrozen = false;

    public void Start()
    {
        dialogueControl = DialogueController.Instance;
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        Debug.Log("Interact called");
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
        isFrozen = true;
        
         //Set dialogue line based on quest state
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        Debug.Log("StartDialogue");
        isDialogueActive = true;

      
        Debug.Log("Panel is active");
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

        string questID = dialogueData.quests.QuestID;

        //Future update add completing quest and handing in
        if (QuestController.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }
    void NextLine()
    {
        Debug.Log("Next Line");
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueControl.SetDialogue(dialogueData.Lines[dialogueIndex]);
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

        foreach (char letter in dialogueData.Lines[dialogueIndex])
        {
            dialogueControl.SetDialogue(dialogueControl.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }

    }

     void DisplayChoices(DialogueChoice choice)
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
         if (givesQuest)
        {
            QuestController.Instance.AcceptQuest(dialogueData.quests);
            questState = QuestState.InProgress;
        }

        dialogueIndex = nextIndex;
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
