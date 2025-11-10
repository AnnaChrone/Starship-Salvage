using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool RangeTable = false;
    public TextMeshProUGUI PressText;
    private void OnTriggerEnter(Collider collide)
    {
        //press B
        RangeTable = true;
        if (gameObject.tag == "Table")
        {
            PressText.text = "Press [B]";
            Debug.Log("In range of B");
        } else if (gameObject.tag == "Door")
        {
            PressText.text = "Press [F]";
            Debug.Log("In range of F");
        }

    }

    private void OnTriggerExit(Collider collide)
    {
        PressText.text = "";
        RangeTable = false;
    }



}
