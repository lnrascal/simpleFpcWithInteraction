using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Transform lid;
    public Vector3 rotationAmount;
    public float duration = 0.8f;
    public bool locked = true;
    public bool isOpen = false;
    public bool isRotating = false;
    public InteractionType GetInteractionType() => InteractionType.Instant;
    public InteractionName GetInteractionName() => InteractionName.Press;
    public void OnInteractStart(PlayerInteract player)
    {
        
    }

    public void OnInteract(PlayerInteract player)
    {
        if (isRotating)
            return;

        if (locked)
        {
            PickableItem inHandItem = player.InHandItem;
            
            if (!inHandItem || inHandItem.itemType != ItemType.Key)
                return;

            locked = false;
            Destroy(inHandItem.gameObject);
        }
        
        
        isRotating = true;
        
        if (isOpen)
        {
            lid.DOLocalRotate(Vector3.zero, duration, RotateMode.Fast).OnComplete(() => isRotating = false);
        }
        else
        {
            lid.DOLocalRotate(rotationAmount, duration, RotateMode.Fast).OnComplete(() => isRotating = false);
        }
        
        isOpen = !isOpen;
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
