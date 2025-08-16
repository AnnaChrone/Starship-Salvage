using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; } //static class cannot be instantiated, works like a global variable
    public List<QuestProgress> activeQuests = new(); //List containg players quest progress
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); //Insures that only one QuestController exists at a time
    }

    public void AcceptQuest(Quests quest) //When a player clicks YES to a quest
    {
        if (IsQuestActive(quest.QuestID)) //Checks if the quest is active already
        {
            return;
        }

        activeQuests.Add(new QuestProgress(quest)); //Otherwise it will be added to the QuestProgress list
    }

    public void CompleteQuest(string questID) //Checks if the quest is complete
    {
        QuestProgress quest = activeQuests.Find(q => q.questID == questID);
        if (quest != null)
        {
            activeQuests.Remove(quest);
        }
    }


    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.questID == questID); //This will return true if the input quest ID matches the ID in a list
    
}
