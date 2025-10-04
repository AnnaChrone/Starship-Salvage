using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShipParts : MonoBehaviour, IUsable
{
    public bool shipPart = true;
    public string Name;
    public int repairCount = 0;
    public GameObject successText;
    public GameObject ship;
    public ShipRepairs repair;
    public bool inRange;

    private void OnTriggerEnter(Collider ship)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
    public void Use(GameObject user)
    {
        

        if (shipPart && inRange)
        {
            var hotbar = FindFirstObjectByType<Hotbar>(); //Finds hotbar in the scene
            repairCount++;
            if (hotbar.RemoveItemByID(Name))
            {
                Debug.Log("Correct item used on ship! Item removed from hotbar.");
                repair.Fix();
                //Destroy(gameObject);

            }
            else
            {
                Debug.Log("You don't have the right item.");

            }
            
        }
    }

}