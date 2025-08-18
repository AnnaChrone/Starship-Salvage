public interface IInteractable //Script for interactables, mainly NPCs
{
    void Interact();
    bool CanInteract();
    void Highlight(); //For later submissions, can change this from changing colour to just haloing
    void Unhighlight();
}
