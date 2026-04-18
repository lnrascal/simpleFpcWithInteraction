using UnityEngine;

public class ItemNest : MonoBehaviour, IInteractable
{
    public PickableItem item;
    private ItemType _itemType;
    
    private Vector3 _itemPosition;
    private Quaternion _itemRotation;

    void Start()
    {
        _itemPosition = item.transform.localPosition;
        _itemRotation = item.transform.localRotation;
        _itemType = item.itemType;
    }
    public InteractionType GetInteractionType() => InteractionType.Hold;

    public InteractionName GetInteractionName() => InteractionName.Put;

    public void OnInteractStart(PlayerInteract player)
    {
    }

    public void OnInteract(PlayerInteract player)
    {
        PickableItem inHandItem = player.InHandItem;
            
        if (!inHandItem || inHandItem.itemType != _itemType)
            return;
        
        inHandItem.transform.SetParent(transform);
        inHandItem.transform.SetLocalPositionAndRotation(_itemPosition, _itemRotation);
        
        inHandItem.rb.isKinematic = true;
    }

    public void OnInteractEnd(PlayerInteract player)
    {
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}
