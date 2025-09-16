using Unity.VisualScripting;
using UnityEngine;

public class SpannerItem : MonoBehaviour, IUsable
{

    [SerializeField] private SpaceshipFixing targetShip;
    [SerializeField] private bool inRange = false;

   
    public void Use(GameObject user)
    {
        // Find the spaceship nearby
        SpaceshipFixing ship = targetShip;
        if (ship != null)
        {
            // Directly apply the effect
            ship.ApplyFix(); 
           // Debug.Log("Spanner used on ship!");
        }
        else
        {
            Debug.Log("No spaceship in range.");
        }
    }
}