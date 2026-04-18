using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPC : MonoBehaviour, IInteractable
{
    public string characterName;
    public DialogueNode dialogueNode;
    public bool questGiven = false;
    public bool questCompleted = false;
    
    public List<ItemType> itemSet = new List<ItemType>();
    public ItemType selectedItem;
    
    [Inject] private DialogueDisplay dialogueDisplay;
    [Inject] private QuestManager questManager;
    public InteractionType GetInteractionType() => InteractionType.Instant;
    public InteractionName GetInteractionName() => InteractionName.Speak;

    public void OnInteractStart(PlayerInteract player)
    {
    }

    public void OnInteract(PlayerInteract player)
    {
        if (questCompleted)
            return;
        
        if (!questGiven)
        {
            selectedItem = itemSet[Random.Range(0, itemSet.Count)];
            dialogueDisplay.Open(selectedItem, characterName, dialogueNode);
            questGiven = true;
        }
        else
        {
            PickableItem inHandItem = player.InHandItem;
            
            if (!inHandItem || inHandItem.itemType != selectedItem)
                return;
            
            questManager.MarkCompleted();
            Destroy(inHandItem.gameObject);
            
            questCompleted = true;
        }
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
