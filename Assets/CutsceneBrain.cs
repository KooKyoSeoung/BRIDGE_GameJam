using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneBrain : MonoBehaviour
{
    public static CutsceneBrain Instance;
    public CutsceneType currentCutsceneType;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        currentCutsceneType = CutsceneType.Intro;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == "TitleScene") currentCutsceneType = CutsceneType.Intro;
        else if (scene.name == "CutScene")
        {
            Debug.Log("1");
            FindObjectOfType<CutsceneManager>().Initialize(currentCutsceneType);
        }
    }
}
