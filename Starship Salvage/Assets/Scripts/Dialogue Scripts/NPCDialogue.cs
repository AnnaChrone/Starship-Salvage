using UnityEngine;
using UnityEngine.Android;
[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] Lines;
    public bool[] autoProgressLines;
    public bool[] endLines;
    public float autoProgressDelay;
    public float typingSpeed;

    public DialogueChoice[] Choices;
    public int questInProgressIndex; //What does NPC say when quest is in progress
    public int questCompletedIndex; //What does NPC say when qiest is completed
    public Quests quests; //the actual quest
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //Line where the choice appears
    public string[] Choices; //Player responses
    public int[] nextDialogueIndexes; //Points to specid response to choice
    public bool[] givesQuest; //If the choice is a quest
}
