public interface IInteractable //Script for interactables, mainly NPCs
{
    void Interact();
    bool CanInteract();
    void Highlight();
    void Unhighlight();
}
