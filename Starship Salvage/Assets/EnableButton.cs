using UnityEngine;

public class EnableButton : MonoBehaviour
{
    public bool GreenCorrect = false;
    public bool RedCorrect = false;
    public bool BlueCorrect = false;
    public bool PurpleCorrect = false;
    private bool correct = false;

    void Update()
    {
        if (!correct)
        {
            if (RedCorrect && BlueCorrect && GreenCorrect && PurpleCorrect)
            {
              Debug.Log("Button enabled");
                correct = true;
    
             //logic to make button visible here
             }
        }
    }
}
