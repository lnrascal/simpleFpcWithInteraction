using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public TMP_Text questText;
    public Image questCompletion;

    public void StartQuest(ItemType itemType)
    {
        questText.text = "Bring " + itemType.ToString();
    }

    public void MarkCompleted()
    {
        questCompletion.gameObject.SetActive(true);
    }
}
