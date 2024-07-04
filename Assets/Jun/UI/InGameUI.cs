using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Button[] btns;

    public enum InGameUIBtnType
    {
        Resume = 0,
        Exit = 1,
    }

    void Start()
    {
        btns[(int)InGameUIBtnType.Resume].onClick.AddListener(() => this.gameObject.SetActive(false));
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
