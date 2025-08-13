using UnityEngine;
using UnityEngine.Android;
[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] Lines;
    public bool[] autoProgressLines;
    public float autoProgressDelay;
    public float typingSpeed;
}
