using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class TitleUI : MonoBehaviour
{
    [SerializeField] Button[] btns;
    [SerializeField] string nextSceneName;
    public enum LobbyBtnType
    {
        Start = 0,
        Exit = 1,
    }

    void Start()
    {
        btns[(int)LobbyBtnType.Start].onClick.AddListener(() => SceneManager.LoadScene(nextSceneName));
        btns[(int)LobbyBtnType.Exit].onClick.AddListener(() => Application.Quit());
    }
}
