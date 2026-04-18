using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogueResponse : MonoBehaviour
{
    public TMP_Text responseText;
    public Button selectButton;
    [Inject] private DialogueDisplay _dialogueDisplay;
    private DialogueNode _dialogueNode;

    public void SetDialogue(DialogueNode node)
    {
        _dialogueNode = node;
        responseText.text = _dialogueNode.text;
        selectButton.onClick.AddListener(Select);
    }

    private void Select()
    {
        DialogueNode next = null;

        if (_dialogueNode.responseNodes.Count > 0)
            next = _dialogueNode.responseNodes[^1];
        
        _dialogueDisplay.SetDialogue(next);
    }
    
}
