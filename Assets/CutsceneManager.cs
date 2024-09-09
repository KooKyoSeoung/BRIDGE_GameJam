using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI mainText;

    [TextArea] public string[] introTexts;
    [TextArea] public string[] trueEndingTexts;
    [TextArea] public string[] normalEndingTexts;
    public Image introImage;
    public Image trueEndingImage;
    public Image normalEndingImage;
    
    string[] currentTexts;
    int currentTextIndex = 0;
    private string nextSceneName;

    public void Initialize(CutsceneType cutsceneType)
    {
        Debug.Log(cutsceneType);
        switch (cutsceneType)
        {
            case CutsceneType.Intro:
                currentTexts = introTexts;
                introImage.gameObject.SetActive(true);
                trueEndingImage.gameObject.SetActive(false);
                normalEndingImage.gameObject.SetActive(false);
                nextSceneName = "Minjoon";
                break;
            case CutsceneType.TrueEnding:
                currentTexts = trueEndingTexts;
                introImage.gameObject.SetActive(false);
                trueEndingImage.gameObject.SetActive(true);
                normalEndingImage.gameObject.SetActive(false);
                nextSceneName = "TitleScene";
                break;
            case CutsceneType.NormalEnding:
                currentTexts = normalEndingTexts;
                introImage.gameObject.SetActive(false);
                trueEndingImage.gameObject.SetActive(false);
                normalEndingImage.gameObject.SetActive(true);
                nextSceneName = "TitleScene";
                break;
        }

        currentTextIndex = 0;
        mainText.text = currentTexts[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTextIndex++;
            if (currentTextIndex >= currentTexts.Length)
            {
                //Fade Out and Load Next Scene
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                mainText.text = currentTexts[currentTextIndex];
            }
        }
    }
}

public enum CutsceneType
{
    Intro,
    TrueEnding,
    NormalEnding
}