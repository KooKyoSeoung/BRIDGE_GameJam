using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    [SerializeField, Tooltip("인게임/시간여행경고/페이드")] GameObject[] uiObjects;

    #region UI
    public InGameUI InGame_UI;
    public TimeTravelWarnUI TimeTravelWarn_UI;
    public FadeUI Fade_UI;
    //public DialogueUI Dialogue_UI { get; set; } = null;
    //public TitleUI Title_UI { get; set; } = null;

    public IndicatorTrigger indicatorTrigger;
    #endregion

    public enum UIType
    {
        InGameUI =0,
        TimeTravelWarnUI=1,
        FadeUI=2,
    }

    void Awake()
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
        if (Fade_UI == null)
            Fade_UI = uiObjects[(int)UIType.FadeUI].GetComponent<FadeUI>();
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

        #region Reset Scene
        if (Input.GetKeyDown(KeyCode.R) && !DialogueManager.Instance.IsDialogue)
        {
            Fade_UI.InputResetBtn();
        }
        #endregion
    }

    public void GameOver()
    {
        Fade_UI.InputResetBtn();
    }

    public void ResetGame()
    {
        TimeTravelManager.Instance.LoadTimeData();
    }
}
