using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activeQuests = new();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AcceptQuest(Quests quest)
    {
        if (IsQuestActive(quest.QuestID))
        {
            return;
        }
        
        activeQuests.Add(new QuestProgress(quest));
    }

    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.questID == questID);
}
