using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    void Start()
    {
        Managers.Sound.PlayBGM("TitleBgm");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Minjoon");
        }
    }
}
