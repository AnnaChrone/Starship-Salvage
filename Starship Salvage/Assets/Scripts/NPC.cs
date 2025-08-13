using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

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
        Debug.Log("StartDialogue");
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        dialoguePanel.SetActive(true);
        Debug.Log("Panel is active");

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        Debug.Log("Next Line");
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.Lines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.Lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
        

    IEnumerator TypeLine()
{
    isTyping = true;
    dialogueText.SetText("");

    foreach (char letter in dialogueData.Lines[dialogueIndex])
    {
        dialogueText.text += letter;
        yield return new WaitForSeconds(dialogueData.typingSpeed);
    }

    isTyping = false;

    if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
    {
        yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        NextLine();
    }
}
    public void EndDialogue()
    {
        Debug.Log("End");
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }
}
