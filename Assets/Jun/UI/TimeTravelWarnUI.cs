using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTravelWarnUI : MonoBehaviour
{
    [SerializeField] float fadeTime;
    [SerializeField] TextMeshProUGUI warnText;
    Color textColor;

    bool isFade = false;
    float transparentTimer = 0f;
    float opaqueTimer = 0f;

    private void Start()
    {
        textColor= warnText.color;
    }

    public void Warning()
    {
        if (isFade)
            StopAllCoroutines();
        StartCoroutine(OpaqueCor());
    }

    public IEnumerator OpaqueCor()
    {
        isFade = true;
        opaqueTimer = 0f;
        while (opaqueTimer <= fadeTime)
        {
            opaqueTimer += Time.deltaTime;
            textColor.a = Mathf.Lerp(0, 1, opaqueTimer / fadeTime);
            warnText.color = textColor;
            yield return null;
        }
        StartCoroutine(TransparentCor());
    }

    public IEnumerator TransparentCor()
    {
        transparentTimer = 0f;
        while (transparentTimer <= fadeTime)
        {
            transparentTimer += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, transparentTimer / fadeTime);
            warnText.color = textColor;
            yield return null;
        }
        isFade = false;
    }
}
