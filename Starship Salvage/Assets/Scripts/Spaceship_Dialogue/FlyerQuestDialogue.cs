using UnityEngine;

public class FlyerQuestDialogue : MonoBehaviour
{
    public Spaceship spaceship;
    public SpaceshipDialogue FlyerQuest;
    public Quests quests;
    public NPC npc;
    private enum QuestState { NotStarted, InProgress, Completed } //States of quests
    private QuestState questState = QuestState.NotStarted; //Initial QuestState
    int dialogueIndex;


    public void FlyerQuestSpeak()
    {
         if (npc != null && npc.hasTalked == true )
        {
            SyncQuestState();
            setIndex();
            spaceship.StartDialogue(FlyerQuest, dialogueIndex);
            Debug.Log("Andromeda talk about progress");
        } 
    }
    

    void SyncQuestState()
    {
        if (quests == null)
        {
            return;
        }

        string questID = quests.QuestID;
        if (QuestController.Instance.IsQuestCompleted(questID))
        {
            questState = QuestState.Completed;
            Debug.Log("quest complete");
        }
        else if (QuestController.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
            Debug.Log("quest in progress");
        }
        else
        {
            questState = QuestState.NotStarted;
            Debug.Log("quest not started");
        }
    }

    void setIndex()
    {
        if (questState == QuestState.NotStarted)
            {
                dialogueIndex = 0;
                //spaceship.StartDialogue(FlyerQuest, dialogueIndex);
                Debug.Log("Andromeda quest not started");
            }
            else if (questState == QuestState.InProgress)
            {
                dialogueIndex = FlyerQuest.questInProgressIndex;
               // spaceship.StartDialogue(FlyerQuest, dialogueIndex);
                Debug.Log("Andromeda accepts quest");
            }
            else if (questState == QuestState.Completed)
            {
                dialogueIndex = FlyerQuest.questCompletedIndex;
                //spaceship.StartDialogue(FlyerQuest, dialogueIndex);
                Debug.Log("Andromeda completes quest");
            }
    }
}
