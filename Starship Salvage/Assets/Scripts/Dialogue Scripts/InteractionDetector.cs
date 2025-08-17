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
