using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] int storyID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.Dialogue_Trigger = this;
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
        gameObject.SetActive(false);
    }
}
