using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField, Tooltip("인게임/시간여행경고/")] GameObject[] uiObjects;

    #region UI
    public InGameUI InGame_UI { get; set; } = null;
    public TimeTravelWarnUI TimeTravelWarn_UI { get; set; } = null;
    //public DialogueUI Dialogue_UI { get; set; } = null;
    //public TitleUI Title_UI { get; set; } = null;
    #endregion

    public enum UIType
    {
        InGameUI =0,
        TimeTravelWarnUI=1,

    }

    void Start()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        // UI
        if (InGame_UI == null)
            InGame_UI = uiObjects[(int)UIType.InGameUI].GetComponent<InGameUI>();
        if(TimeTravelWarn_UI==null)
            TimeTravelWarn_UI = uiObjects[(int)UIType.TimeTravelWarnUI].GetComponent<TimeTravelWarnUI>();
        //if (Dialogue_UI == null)
        //    Dialogue_UI = GetComponentInChildren<DialogueUI>();
    }

    void Update()
    {
        #region InGameUI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGame_UI == null)
                    return;
            InGame_UI.OnOffUI();
        }
        #endregion
    }
}
