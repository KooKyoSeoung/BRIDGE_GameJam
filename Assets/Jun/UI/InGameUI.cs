using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Button[] btns;
    [SerializeField] string titleSceneName;

    public enum InGameUIBtnType
    {
        Resume = 0,
        ReturnTitle=1,
        Exit = 2,
    }

    void Start()
    {
        btns[(int)InGameUIBtnType.Resume].onClick.AddListener(() => this.gameObject.SetActive(false));
        btns[(int)InGameUIBtnType.ReturnTitle].onClick.AddListener(() =>SceneManager.LoadScene(titleSceneName));
        btns[(int)InGameUIBtnType.Exit].onClick.AddListener(() => Application.Quit());
    }

    public void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
