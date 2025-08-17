using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact(); //Calls Interaction from new input system
        }
    }
    private void OnTriggerEnter(Collider collide)
    {
        if ( collide.TryGetComponent(out IInteractable interactable))
        {
            interactableInRange = interactable; //Checks if object is interactable
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null; //Ensures that player does not interact with anything
        }
    } 


}

/*Title: Add an Interaction System to your Game- Top Down Unity #16
Author: Game Code Library
Date: 12/08/2025
Availability: https://youtu.be/MPP9GLp44Pc?si=I9y-UMrxzywSl3G6
*/