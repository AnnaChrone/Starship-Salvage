using UnityEngine;
[CreateAssetMenu(fileName = "NewSpaceshipDialogue", menuName = "Spaceship Dialogue")]
public class SpaceshipDialogue : ScriptableObject
{
    public string[] Lines; //Actual Spaceship dialogue
    public bool[] autoProgressLines; // Moves on to the Next line
    public bool[] endLines; //Dialogue ends
    public float autoProgressDelay; //Delay between dialogue
    public float typingSpeed; //Speed at which each char is displayed

}
