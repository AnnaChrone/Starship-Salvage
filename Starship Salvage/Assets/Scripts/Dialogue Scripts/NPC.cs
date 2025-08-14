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

        Debug.Log("StartDialogue");
        isDialogueActive = true;
        dialogueIndex = 0;

      
        Debug.Log("Panel is active");
        dialogueControl.SetNPCInfo(dialogueData.npcName);
        dialogueControl.ShowDialoguePanel(true);
        DisplayCurrentLine();
        
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
            dialogueControl.CreateChoiceButton(choice.Choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
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
        Cursor.visible = false;;
    }
}
