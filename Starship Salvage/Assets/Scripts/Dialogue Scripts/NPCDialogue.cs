using UnityEngine;
using UnityEngine.Android;
[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName; //Name of NPC
    public string[] Lines; //Actual NPC dialogue
    public bool[] autoProgressLines; // Moves on to the Next line
    public bool[] endLines; //Dialogue ends
    public float autoProgressDelay; //Delay between dialogue
    public float typingSpeed; //Speed at which each char is displayed

    public DialogueChoice[] Choices;
    public int questInProgressIndex; //What does NPC say when quest is in progress
    public int questCompletedIndex; //What does NPC say when quest is completed
    public int FlowerTableindex;
    public Quests quests; //the actual quest
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //Line where the choice appears
    public string[] Choices; //Player responses
    public int[] nextDialogueIndexes; //Points to specific response to choice
    public bool[] givesQuest; //If the choice is a quest
}
