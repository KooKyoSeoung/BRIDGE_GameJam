using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("���α׷��� & ��ȹ Part")]
    [SerializeField, Tooltip("����� ��, �ٷ� ��ȭ�� �ҷ������� ����")] bool isImmediatelyCall = false;
    [SerializeField, Tooltip("��ȭ �ý����� �ҷ����� ���丮 ID")] int storyID;

    public void Interaction()
    {
        DialogueManager.Instance.Dialogue_UI.StoryID = storyID;
        DialogueManager.Instance.Dialogue_UI.StartDialogue();
        DialogueManager.Instance.Dialogue_Trigger = null;
        gameObject.SetActive(false);
    }
}
