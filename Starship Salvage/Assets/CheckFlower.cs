using UnityEngine;

public class CheckFlower : MonoBehaviour
{
    public string RequiredFlower;
    private string GivenFlower;
    public bool CorrectFlower = false;
    public EnableButton OverallCheck;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RequiredFlower != null)
        {
            GivenFlower = transform.GetChild(0).name;


            if (GivenFlower == RequiredFlower)
            {

                if (RequiredFlower == "Green")
                {
                    OverallCheck.GreenCorrect = true;
                }

                if (RequiredFlower == "Blue")
                {
                    OverallCheck.BlueCorrect = true;
                }

                if (RequiredFlower == "Red")
                {
                    OverallCheck.RedCorrect = true;
                }

                if (RequiredFlower == "Purple")
                {
                    OverallCheck.PurpleCorrect = true;
                }
                if (!CorrectFlower)
                { Debug.Log("Correct Flower"); }
                    
            
                CorrectFlower = true;
            }
        }
    }
}
