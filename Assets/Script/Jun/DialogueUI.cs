using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("기획자 Part")]
    [SerializeField, Tooltip("다음 대화를 자동으로 넘기는지 결정하는 것")] bool isAutoDialouge;
    [SerializeField, Range(0.1f, 0.5f), Tooltip("타이핑 속도")] float typeTime;
    [SerializeField, Range(0.1f, 1f), Tooltip("대화가 자동으로 넘어가는 속도")] float autoNextDialogueTime;
    [SerializeField, Range(0f, 3.0f), Tooltip("다음 대화로 넘어갈 수 있게 해주는 속도")] float nextDialogueTime;

    [Header("개발자 Part")]
    [SerializeField] GameObject onoffUI;
    [SerializeField] GameObject skipTextObject;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI dialogueText;

    int storyID =1;
    public int StoryID { get { return storyID; } set { storyID = value; } }
    Dialogue dialogue = new Dialogue();
    int curIdx = 0; // Current Dialogue Index
    bool isStartDialogue = false; // Prevent Overlap Dialogue
    bool isDoneDialogue = false; // Show Next Dialouge Key
    
    public void LoadDiagloue()
    {
        curIdx = 0;
        dialogue = DialogueManager.Instance.LoadDialogue(storyID);
        if (dialogue == null)
            return;
        timeText.text = dialogue.storyTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isStartDialogue)
            StartDialogue();
        if(Input.anyKeyDown && isDoneDialogue)
        {
            CheckNextDialogue();
        }
    }

    public void StartDialogue()
    {
        if (dialogue == null)
            return;
        // Skip Btn
        isDoneDialogue = false;
        skipTextObject.SetActive(false);

        // Start Dialogue
        onoffUI.SetActive(true);
        isStartDialogue = true;
        LoadDiagloue();
        StartCoroutine(DialogueCor());       
    }

    public IEnumerator DialogueCor()
    { 
        dialogueText.text = "";
        dialogueText.text = dialogue.storyLines[curIdx];
        curIdx += 1;
        yield return new WaitForSeconds(nextDialogueTime);
        isDoneDialogue = true;
        skipTextObject.SetActive(true);
    }

    public IEnumerator DialogueTypeCor()
    {
        isDoneDialogue = false;
        int len = dialogue.storyLines[curIdx].Length;
        dialogueText.text = "";
        for (int i = 0; i < len; i++)
        {
            dialogueText.text += dialogue.storyLines[curIdx][i];
            yield return new WaitForSeconds(typeTime);
        }
        curIdx += 1;
        if (isAutoDialouge)
        {
            yield return new WaitForSeconds(autoNextDialogueTime);
            CheckNextDialogue();
        }
        else
        {
            isDoneDialogue = true;
            skipTextObject.SetActive(true);
        }
    }

    public void CheckNextDialogue()
    {
        int len = dialogue.storyLines.Count;
        skipTextObject.SetActive(false);
        if (curIdx >= len)
        {
            // 대화 종료
            isStartDialogue = false;
            isDoneDialogue = false;
            onoffUI.SetActive(false);
        }
        else
        {
            // 다음 대화
            StartCoroutine(DialogueCor());
        }
    }
}
