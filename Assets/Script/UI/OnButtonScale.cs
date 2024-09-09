using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OnButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Vector3 originalScale;
    protected float scaleIncreasment = 1.2f;
    [SerializeField] protected Transform textTransform;
    [SerializeField] protected Button btn;

    protected virtual void Awake()
    {
        originalScale = textTransform.localScale;

        if (textTransform == null)
            textTransform = GetComponent<Transform>();
        if (btn == null)
            btn = GetComponent<Button>(); 
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!btn.interactable)
            return;
        textTransform.localScale = originalScale * scaleIncreasment;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!btn.interactable)
            return;
        textTransform.localScale = originalScale;
    }

    public void ResizeButtonScale(bool _isIncrease = true)
    {
        if (_isIncrease)
        {
            textTransform.localScale = originalScale * scaleIncreasment;
        }
        else
        {
            textTransform.localScale = originalScale;
        }
    }
}
