public interface IInteractable
{
    InteractionType GetInteractionType();
    InteractionName GetInteractionName();

    void OnInteractStart(PlayerInteract player);
    void OnInteract(PlayerInteract player);
    void OnInteractEnd(PlayerInteract player);

    void OnHoverEnter();
    void OnHoverExit();
}