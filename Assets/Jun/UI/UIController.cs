using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject[] uiObjects;

    #region UI
    public InGameUI InGame_UI { get; set; } = null;
    //public DialogueUI Dialogue_UI { get; set; } = null;
    //public TitleUI Title_UI { get; set; } = null;
    #endregion

    public enum UIType
    {
        InGameUI =0,
    }

    void Start()
    {
        if (InGame_UI == null)
            InGame_UI = GetComponentInChildren<InGameUI>();
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
