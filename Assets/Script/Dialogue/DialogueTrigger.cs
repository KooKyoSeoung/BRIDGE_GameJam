using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("프로그래밍 & 기획 Part")]
    [SerializeField, Tooltip("닿았을 때, 바로 대화를 불러오는지 결정")] bool isImmediatelyCall = false;
    [SerializeField, Tooltip("대화 시스템을 불러오는 스토리 ID")] int storyID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.Dialogue_Trigger = this;
            if (isImmediatelyCall)
            {
                Interaction();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.Dialogue_Trigger = null;
        }
    }

    public void Interaction()
    {
        DialogueManager.Instance.Dialogue_UI.StoryID = storyID;
        DialogueManager.Instance.Dialogue_UI.StartDialogue();
        DialogueManager.Instance.Dialogue_Trigger = null;
        Destroy(this.gameObject); // or gameObject.SetActive(false);
    }
}
