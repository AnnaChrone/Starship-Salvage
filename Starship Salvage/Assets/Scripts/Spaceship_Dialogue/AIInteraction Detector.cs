using UnityEngine;

public class AIInteractionDetector : MonoBehaviour
{
    private AIInteractable interactableInRangeAI = null;

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.TryGetComponent(out AIInteractable interactable))
        {
            interactableInRangeAI = interactable; //Checks if object is interactable
            if (interactable.CanInteractAI())
        {
            interactable.InteractAI();
        }
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.TryGetComponent(out AIInteractable interactable) && interactable == interactableInRangeAI)
        {
            interactableInRangeAI = null; //Ensures that player does not interact with anything
        }
    } 

}
