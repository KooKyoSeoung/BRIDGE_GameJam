using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("��ȹ�� Part")]
    //[SerializeField, Tooltip("���� ��ȭ�� �ڵ����� �ѱ���� �����ϴ� ��")] bool isAutoDialouge;
    [SerializeField, Range(0f, 3.0f), Tooltip("���� ��ȭ�� �Ѿ �� �ְ� ���ִ� �ӵ�")] float nextDialogueTime;

    [Header("������ Part")]
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

    #region Unity Life Cycle
    private void Start()
    {
        DialogueManager.Instance.Dialogue_UI = this;
    }
    private void Update()
    {
        if (Input.anyKeyDown && isDoneDialogue)
            CheckNextDialogue();
    }
    #endregion

    #region Start Dialouge 
    public void StartDialogue()
    {
        if (isStartDialogue)
            return;
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

    public void LoadDiagloue()
    {
        curIdx = 0;
        dialogue = DialogueManager.Instance.LoadDialogue(storyID);
        if (dialogue == null)
            return;
        timeText.text = dialogue.storyTime;
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
    #endregion

    /// <summary>
    /// Keep Dialogue
    /// </summary>
    public void CheckNextDialogue()
    {
        int len = dialogue.storyLines.Count;
        skipTextObject.SetActive(false);
        if (curIdx >= len)
        {
            // ��ȭ ����
            isStartDialogue = false;
            isDoneDialogue = false;
            onoffUI.SetActive(false);
        }
        else
        {
            // ���� ��ȭ
            StartCoroutine(DialogueCor());
        }
    }
}
