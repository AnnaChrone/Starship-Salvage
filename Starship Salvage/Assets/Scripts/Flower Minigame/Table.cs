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
        PressText.text = "Press [B]";
        Debug.Log("In range of B");
    }

    private void OnTriggerExit(Collider collide)
    {
        PressText.text = "";
        RangeTable = false;
    }



}
