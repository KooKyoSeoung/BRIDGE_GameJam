using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [Header("°³¹ß Part")]
    [SerializeField] Button[] btns;
    [SerializeField] GameObject onoffUIObject;
    [SerializeField] string titleSceneName;

    public enum InGameUIBtnType
    {
        Resume = 0,
        ReturnTitle=1,
        Exit = 2,
    }

    void Start()
    {
        btns[(int)InGameUIBtnType.Resume].onClick.AddListener(ResumeGame);
        btns[(int)InGameUIBtnType.ReturnTitle].onClick.AddListener(ReturnTitleScene);
        btns[(int)InGameUIBtnType.Exit].onClick.AddListener(()=> Application.Quit());
    }

    #region Btn Method
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        onoffUIObject.SetActive(false);
    }

    public void ReturnTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }
    #endregion

    public void OnOffUI()
    {
        if (onoffUIObject.activeSelf)
        {
            onoffUIObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            onoffUIObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
