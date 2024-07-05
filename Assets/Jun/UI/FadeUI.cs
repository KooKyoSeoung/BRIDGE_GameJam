using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeUI : MonoBehaviour
{
    [SerializeField] float fadeTime = 2f;
    [SerializeField] float fadeWaitTime = 0f;
    [SerializeField] Image fadeImage;
    Color defaultColor;
    bool isFadeIn = false;
    bool isFadeOut = false;
    bool isPressReset = false;
    bool isFadeOutIn = false;

    private void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponentInChildren<Image>();
        defaultColor = fadeImage.color;
    }

    public void FadeIn() // Dark ----> Light
    {
        if(isFadeIn)
            StopCoroutine(FadeInCor());
        StartCoroutine(FadeInCor());
    }
    public void FadeOut() // Light ----> Dark
    {
        if (isFadeOut)
            StopCoroutine(FadeOutCor());
        StartCoroutine(FadeOutCor());
    }

    #region FadeOutIn
    public void FadeOutIn()
    {
        if (isFadeOutIn)
            return;
        StartCoroutine(FadeOunInCor());
    }
    public IEnumerator FadeOunInCor()
    {
        isFadeOutIn = true;
        float timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            defaultColor.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = defaultColor;
            yield return null;
        }

        yield return new WaitForSeconds(fadeWaitTime);

        timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            defaultColor.a = Mathf.Lerp(1, 0, timer / fadeTime);
            fadeImage.color = defaultColor;
            yield return null;
        }
        isFadeOutIn = false;
    }
    #endregion

    #region FadeCor
    public IEnumerator FadeInCor()
    {
        isFadeIn = true;
        float timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            defaultColor.a = Mathf.Lerp(1, 0, timer / fadeTime);
            fadeImage.color = defaultColor;
            yield return null;
        }
        isFadeIn = false;
    }

    public IEnumerator FadeOutCor()
    {
        isFadeOut = true;
        float timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            defaultColor.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = defaultColor;
            yield return null;
        }
        isFadeOut = false;
    }
    #endregion

    #region Reset
    public void InputResetBtn()
    {
        if (isPressReset)
            return;
        StartCoroutine(ResetEffectCor());
    }
    public IEnumerator ResetEffectCor()
    {
        isPressReset = true;
        float timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            defaultColor.a = Mathf.Lerp(0, 1, timer / fadeTime);
            fadeImage.color = defaultColor;
            yield return null;
        }
        isPressReset = false;
        SceneManager.LoadScene("Delay");
    }
    #endregion
}
