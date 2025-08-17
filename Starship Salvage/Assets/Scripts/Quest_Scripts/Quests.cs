using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
[CreateAssetMenu( menuName = "Quest")]
public class Quests : ScriptableObject
{
    public string QuestID; //special identifier for quests
    public string QuestName;
    public string Description; // description 
    public List<QuestObjective> objectives;

}
[System.Serializable] //We want this to save to our system

    public class QuestObjective
    {
        public int ObjectiveID;//Match with item ID we fetched,delivered... this will be updated when all quests are finalized
        public string Description;
        public ObjectiveType type;
        public int Incomplete; //This value will be a 0 for our fetch quest. 
        public int Complete;// This value will be 1, to indicate an itme is found

        public bool isCompleted => Incomplete >= Complete; //=>. is a lambda expression. isCompleted should return the value based on the expression
    }

    public enum ObjectiveType { Fetch } //This enum will grow and will contain different categories of quests

    [System.Serializable]
    public class QuestProgress
    {
        public Quests quest;
        public List<QuestObjective> objectives;

        public QuestProgress(Quests quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjective>();

            //Making a copy to not modify original argument; like a reference
            foreach (var obj in objectives)
            {
                objectives.Add(new QuestObjective
                {
                    ObjectiveID = obj.ObjectiveID,
                    Description = obj.Description,
                    type = obj.type,
                    Incomplete = 0,
                    Complete = 1
                });
            }
        }

        //The quest and objective information will be linked to UI so that the player can keep track of progress
        public bool isCompleted => objectives.TrueForAll(o => o.isCompleted);
        public string questID => quest.QuestID; //Gets quest ID

    }

    
/*Title: Create a Quest System with Scriptable Objects - Top Down Unity 2D #25
Author: Game Code Library
Date Accessed: 15/08/2025
Availability: 
*/

/*Title: Quest Items and Saving Quest Progress - Top Down Unity 2D #27
Author: Game Code Library
Date Accessed: 16/08/2025
Availability: https://youtu.be/AdPu5r2pP5E?si=mgy3ZlrIAGjl0DsY
*/