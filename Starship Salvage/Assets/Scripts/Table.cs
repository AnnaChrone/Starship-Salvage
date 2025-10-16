using UnityEngine;

public class Table : MonoBehaviour
{
    public bool RangeTable = false;
    private void OnTriggerEnter(Collider collide)
    {
        //press B
        RangeTable = true;
        Debug.Log("In range of B");
    }

    private void OnTriggerExit(Collider collide)
    {
        
        RangeTable = false;
    }



}
