using UnityEngine;

public class Table : MonoBehaviour
{
    public bool RangeTable = false;
    private void OnTriggerEnter(Collider collide)
    {
        //press B
        RangeTable = true;
    }

    private void OnTriggerExit(Collider collide)
    {
        
        RangeTable = false;
    }
}
