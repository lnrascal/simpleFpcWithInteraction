using TMPro;
using UnityEngine;
using Zenject;

public class DialogueDisplay : MonoBehaviour
{
    public TMP_Text characterName;
    public TMP_Text dialogueText;
    public Transform responseContainer;
    public DialogueResponse dialogueResponsePrefab;
    
    [Inject] private FPController _fpController;
    [Inject] private QuestManager _questManager;
    [Inject] private DiContainer _container;

    private ItemType _itemToBring;
    public void SetDialogue(DialogueNode dialogueNode)
    {
        if (dialogueNode == null)
        {
            Close();
            return;
        }
        
        _fpController.DisablePlayerControl();
        
        dialogueText.text = dialogueNode.text;

        if (dialogueNode.action == ActionType.StartQuest)
        {
            _questManager.StartQuest(_itemToBring);
        }
        
        ClearResponseContainer();
        foreach (DialogueNode responseNode in dialogueNode.responseNodes)
        {
            var response = _container.InstantiatePrefabForComponent<DialogueResponse>(dialogueResponsePrefab, responseContainer);
            response.SetDialogue(responseNode);
        }
    }

    public void ClearResponseContainer()
    {
        foreach (Transform child in responseContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void Open(ItemType itemType, string character, DialogueNode dialogueNode)
    {
        if (!dialogueNode)
            return;

        _itemToBring = itemType;
        
        this.gameObject.SetActive(true);
        characterName.text = character;
        ClearResponseContainer();
        
        SetDialogue(dialogueNode);
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
        _fpController.EnablePlayerControl();
    }
}
