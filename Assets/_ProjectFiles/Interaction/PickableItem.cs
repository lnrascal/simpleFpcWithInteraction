using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    public ItemType itemType;
    public Rigidbody rb;
    public InteractionType GetInteractionType() => InteractionType.Instant;
    public InteractionName GetInteractionName() => InteractionName.PickUp;
    
    public virtual void OnInspect() {}
    public void OnInteractStart(PlayerInteract player) {}

    public void OnInteract(PlayerInteract player)
    {
        player.PickUp(this);
    }

    public void OnInteractEnd(PlayerInteract player) {}

    public void OnHoverEnter() {}
    public void OnHoverExit() {}
}