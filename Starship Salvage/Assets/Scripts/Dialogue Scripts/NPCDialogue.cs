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
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //Line where the choice appears
    public string[] Choices; //Player responses
    public int[] nextDialogueIndexes; //Points to specid response to choice
}
