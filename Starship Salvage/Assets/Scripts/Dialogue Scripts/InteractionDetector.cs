using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    void Start()
    {
        
    }
    private IInteractable interactableInRange = null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
            print("I work");
        }
    }
    private void OnTriggerEnter(Collider collide)
    {
        if ( collide.TryGetComponent(out IInteractable interactable))
        {
            interactableInRange = interactable;
            Debug.Log("State interactable");
        }
        Debug.Log("Player triggered");
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
        }
    } 


}
