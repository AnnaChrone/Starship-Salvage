using UnityEngine;

public class FlyerQuestDialogur : MonoBehaviour
{
    public Spaceship spaceship;
    public SpaceshipDialogue FlyerQuest;
    public Quests quests;
    private enum QuestState { NotStarted, InProgress, Completed } //States of quests
    private QuestState questState = QuestState.NotStarted; //Initial QuestState
    int dialogueIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        SyncQuestState();
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
            {
                dialogueIndex = FlyerQuest.questInProgressIndex;
            }
            else if (questState == QuestState.Completed)
            {
                dialogueIndex = FlyerQuest.questCompletedIndex;
            }

        spaceship.StartDialogue(FlyerQuest);
    }

     void SyncQuestState()
    {
        if (quests == null)
        {
            return;
        }

        string questID = quests.QuestID;
        if (QuestController.Instance.isQuestCompleted(questID))
        {
            questState = QuestState.Completed;
        }
        else if (QuestController.Instance.IsQuestActive(questID))
            {
                questState = QuestState.InProgress;
            }
            else
            {
                questState = QuestState.NotStarted;
            }
    }
}
