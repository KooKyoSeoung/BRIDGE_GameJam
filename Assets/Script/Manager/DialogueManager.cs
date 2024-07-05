using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("개발자 Part")]    
    [SerializeField, Tooltip("스토리 Json")] TextAsset storyLine;
    Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

    private bool isDialogue = false;
    public bool IsDialogue { get { return isDialogue; } set { isDialogue = value; ControlPlayerMove(!isDialogue); } }

    #region Player Information
    private GameObject player;
    private PlayerControl playerControl;
    public GameObject Player { get { return player; } }
    #endregion

    public DialogueUI Dialogue_UI { get; set; } = null;
    public DialogueTrigger Dialogue_Trigger { get; set; } = null;
    public IndicatorTrigger Indicator_Trigger { get; set; } = null;
    #region Unity Life Cycle
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
   
    /// <summary>
    /// Call Once When you start game : Load Stroy Dialouge
    /// </summary>
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
    #endregion

    /// <summary>
    /// Approach Dialogue By Dialogue ID, Call By StoryTrigger
    /// </summary>
    /// <param name="_ID"></param>
    /// <returns></returns>
    public Dialogue LoadDialogue(int _ID)
    {
        if (dialogueDictionary.ContainsKey(_ID))
        {
            return dialogueDictionary[_ID];
        }
        return null;
    }

    public void ControlPlayerMove(bool _canMove)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerControl = player.GetComponent<PlayerControl>();
        }

        if (_canMove)
            playerControl.PlayerSubscribe();
        else
            playerControl.PlayerUnSubscribe();
    }
}
