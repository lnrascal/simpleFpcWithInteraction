using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "DialogueNode")]
public class DialogueNode : ScriptableObject
{
    public string text;
    public List<DialogueNode> responseNodes;
    public ActionType action;
}

public enum ActionType
{
    None,
    StartQuest
}