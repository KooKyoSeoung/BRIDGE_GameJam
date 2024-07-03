using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("������ Part")]    
    [SerializeField, Tooltip("���丮 Json")] TextAsset storyLine;
    Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

    public bool IsDialogue { get; set; } = false;
    public DialogueUI Dialogue_UI { get; set; } = null;
    public DialogueTrigger Dialogue_Trigger { get; set; } = null;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        LoadJsonFile();
    }

    public void LoadJsonFile()
    {
        if (storyLine != null)
        {
            DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(storyLine.text);
            foreach (var dialogue in dialogueList.dialogues)
            {
                if (!dialogueDictionary.ContainsKey(dialogue.storyID))
                {
                    dialogueDictionary.Add(dialogue.storyID, dialogue);
                }
            }
        }
    }

    public Dialogue LoadDialogue(int _ID)
    {
        if (dialogueDictionary.ContainsKey(_ID))
        {
            return dialogueDictionary[_ID];
        }
        return null;
    }
}
