using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.SharpZipLib.BZip2;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour, IInteractable //NPC is an interactable
{
    public NPCDialogue dialogueData; //Calls from NPCDialogue class
    private DialogueController dialogueControl; //Calls from DialogueControoler class
    public FlyerQuestDialogue flyerQuest;

    public FlyerAppear flyerappear;
    public Item item; //Calls from Item class
    public bool hasTalked = false;

    private int dialogueIndex; //Index of lines
    private bool isTyping, isDialogueActive;
    private enum QuestState { NotStarted, InProgress, Completed } //States of quests
    private QuestState questState = QuestState.NotStarted; //Initial QuestState
    public bool isFrozen = false; //Pauses game
    public Hotbar hotbar; //Calls the hotbar
    public GameObject RewardItem;

    [Header("Continue Indicator")]
    public TextMeshProUGUI continueIndicator; 

    [Header("NPC activation on quest give")]
    public GameObject CoLuNPC;
    public GameObject RaLuNPC;
    public GameObject LuLuNPC;
    public GameObject MinLuNPC;

    [Header("Region Flowers")]
    public bool RaLuFlower;
    public bool MinLuFlower;
    public bool CoLuFlower;
    public bool LuLuFlower;
    public GameObject FlowerTable;

    [Header("Identity bools")]
    public bool Zorb;
    public bool Zinnia;
    public bool Rami;
    public bool festivalZorb;
    public bool HeadingHome;

    

    public bool QuestFinished;
    public bool FinishedNPC = false;
    public bool FirstTime = true;
    public bool Denied = false;

    [Header("Exclamations and Objective")]
    public GameObject Exclamation;
    public Objectives Objective;
    public GameObject FinalCutscene;
    public GameObject HUD;
    public GameObject Player;
    public GameObject End;

    [Header("NPC Presidents")]
    public NPC CoLu;
    public NPC LuLu;
    public NPC RaLu;

    [Header("Audio Assignment")]
    public AudioClip Clip1;
    public AudioClip Clip2; 
    public AudioClip Clip3;
    public AudioClip Clip4;
    public AudioSource voice;


    public void Start()
    {
        dialogueControl = DialogueController.Instance; //Create an instance
    }
    private bool SetZin = false;
    private bool SetZorb = false;
    public void Update()
    {
        if (Zorb && (CoLu.QuestFinished) && (RaLu.QuestFinished) && (LuLu.QuestFinished) && !SetZorb)
        {
            Exclamation.SetActive(true);
            Objective.GetObjective("DELIVERED");
            SetZorb = true;
        }
        
        if (Zinnia && RaLuFlower && MinLuFlower && CoLuFlower && LuLuFlower && !SetZin)
        {
            Exclamation.SetActive(true);
            Objective.GetObjective("FOUND");
            SetZin = true;
        }
            
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

    public void StartDialogue()
    {

        if (FinishedNPC)
        {
            Debug.Log($"{name}'s quest is finished. No more dialogue.");
            return;
        }
        //check flowers are there
        RaLuFlower = hotbar.hasItem("CLF");
        MinLuFlower = hotbar.hasItem("RLF");
        LuLuFlower = hotbar.hasItem("LLF");
        CoLuFlower = hotbar.hasItem("MLF");

        
        Debug.Log("dialogue has started");
        Exclamation.SetActive(false);
        isFrozen = true; //Pauses game so that player does not run away from NPC

        SyncQuestState(); //Sync dialogue depending on state of quest

        //Set dialogue line based on quest state
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;

            if (Rami && !FirstTime)
            {
                dialogueIndex = dialogueData.RetryRamiindex;
            }
        }
        else if (questState == QuestState.InProgress)
        {
            if (Zinnia && RaLuFlower && MinLuFlower && CoLuFlower && LuLuFlower)
            {
                dialogueIndex = dialogueData.FlowerTableindex;
                if (FlowerTable != null)
                {
                    FlowerTable.SetActive(true);
                    Objective.GetObjective("ARRANGE");
                }
            }
            else
            {
                dialogueIndex = dialogueData.questInProgressIndex; //There is a specific index for what dialogue to display
            }
                
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
            
            RewardItem.SetActive(true); //drops reward item for player
            
        }

        if (Rami)
        {
            FirstTime = false;
        }


        isDialogueActive = true;

        dialogueControl.SetNPCInfo(dialogueData.npcName);
        dialogueControl.ShowDialoguePanel(true);
        DisplayCurrentLine();

    }

    public void SyncQuestState()
    {
        if (dialogueData.quests == null)
            return;

        string questID = dialogueData.quests.QuestID;

        if (QuestController.Instance.IsQuestCompleted(questID))
        {
            questState = QuestState.Completed;
            QuestFinished = true;
            Debug.Log($"{name}: Quest already completed.");
            return;
        }


        if (QuestController.Instance.IsQuestActive(questID))
        {
            if (!Zorb)
            {
                int slotIndex = hotbar.FindItemSlot(questID);
                if (slotIndex != -1)
                {
                    hotbar.RemoveItemAt(slotIndex);
                    questState = QuestState.Completed;
                    QuestFinished = true;
                    if (Zinnia)
                    {
                        Objective.GetObjective("GEARS");
                    }
                    if (Rami)
                    {
                        Objective.GetObjective("METAL");

                    }
                    QuestController.Instance.CompleteQuest(questID);
                    if (!Rami && !Zorb && !Zinnia)
                    {
                        Objective.GetObjective("FLYER");
                    }
                    Debug.Log($"{name}: Quest completed during interaction.");
                }
                else
                {
                    questState = QuestState.InProgress;
                    Debug.Log($"{name}: Quest in progress.");
                    if (Zorb)
                    {
                        Objective.GetObjective("FLYER");
                    } 
                    if (Zinnia)
                    {
                        Objective.GetObjective("FLOWER");

                    }

                    if (Rami)
                    {
                        Objective.GetObjective("COOK");

                    }
                }
            }
            else if (Zorb && (CoLu.QuestFinished) && (RaLu.QuestFinished) && (LuLu.QuestFinished))
            {
                questState = QuestState.Completed;
                Objective.GetObjective("SPANNER");
                QuestFinished = true;
                QuestController.Instance.CompleteQuest(questID);
                Debug.Log($"{name}: Zorb’s quest completed.");
            }
            else
            {
                questState = QuestState.InProgress;
                Debug.Log($"{name}: Quest in progress for Zorb.");
            }
        }
        else
        {
            questState = QuestState.NotStarted;
            Debug.Log($"{name}: Quest not started.");
        }
    }



    public void NextLine()
    {
        // Hide continue indicator as player progresses
        if (continueIndicator != null)
        {
            continueIndicator.gameObject.SetActive(false);
        }

        SyncQuestState();    

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
         if (festivalZorb && dialogueIndex == 7)
        {
            HeadingHome = true;
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
        PlayRandomClip();
        foreach (char letter in dialogueData.Lines[dialogueIndex]) //Types out line one char at a time
        {
            dialogueControl.SetDialogue(dialogueControl.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        // Show continue indicator if auto-progress is not enabled
        if (continueIndicator != null)
        {
            if (dialogueData.autoProgressLines.Length <= dialogueIndex || !dialogueData.autoProgressLines[dialogueIndex])
            {
                continueIndicator.gameObject.SetActive(true);
            }
        }

        // Auto-progress line if enabled
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
        hasTalked = true;



        if (MinLuNPC != null)
        {
            MinLuNPC.SetActive(true);
            RaLuNPC.SetActive(true );
            CoLuNPC.SetActive(true );
            LuLuNPC.SetActive(true );
        }

        if (flyerappear != null)
        {
            flyerappear.FlyerAppears();
        }

        if (flyerQuest != null && flyerappear != null && flyerappear.hasFlyerAppeared == true)
            {
                flyerQuest.FlyerQuestSpeak();
            }

        if (questState == QuestState.Completed)
        {
            FinishedNPC = true;
            Debug.Log($"{name}'s quest dialogue finished.");
        }


        Debug.Log("has talked is true");
        isDialogueActive = false;
        dialogueControl.SetDialogue("");
        dialogueControl.ShowDialoguePanel(false);
        isFrozen = false;

        if (festivalZorb && HeadingHome)
        {
            //play heading home scene
            Debug.Log("Heading Home!");
            FinalCutscene.SetActive(true);
            HUD.SetActive(false);
            isFrozen = true;

        }
    }

    private int lastClipIndex = -1; 
    public void PlayRandomClip()
    {
        if (voice == null)
        {
            Debug.LogWarning("AudioSource not assigned!");
            return;
        }

        AudioClip[] clips = { Clip1, Clip2, Clip3, Clip4 };
        AudioClip[] validClips = System.Array.FindAll(clips, c => c != null);

        if (validClips.Length == 0)
        {
            Debug.LogWarning("No valid audio clips assigned!");
            return;
        }

        int randomClip;

        // Make sure it don't pick the same clip twice in a row
        do
        {
            randomClip = Random.Range(0, validClips.Length);
        }
        while (validClips.Length > 1 && randomClip == lastClipIndex);

        lastClipIndex = randomClip;

        AudioClip selectedClip = validClips[randomClip];
        voice.PlayOneShot(selectedClip);
    }
}

/*Title: Add NPC and Dialogue System to your Game - Top Down Unity 2D #19
Author: Game Code Library
Date Accessed: 12/08/2025
Availability: https://youtu.be/eSH9mzcMRqw?si=TCWqqffeueBr5F4s
*/

/*Title: Create a Dialogue System with Branching Choices - Top Down Unity 2D #22
Author: Game Code Library
Date Accessed: 13/08/2025
Availability: https://youtu.be/zbYuLu_8spI?si=Os6JTDX-wZ3uSI-m
*/

/*Title: NPC Quest Giver with Changing Dialogue Lines! - Top Down Unity 2D #26
Author: Game Code Library
Date Accessed: 16/08/2025
Availability: https://youtu.be/_hA3y45P4Ow?si=mJRbK7tHm-ee6x9d
*/
