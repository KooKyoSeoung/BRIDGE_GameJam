using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatheringRock : MonoBehaviour
{
    [SerializeField] private Waterfall _waterfall;
    //[SerializeField] private Sprite _tallSprite;
    //[SerializeField] private Sprite _shortSprite;

    [SerializeField] private Vector2 _noColOffset;
    [SerializeField] private Vector2 _noColSize;
    [SerializeField] private Vector2 _shortColOffset;
    [SerializeField] private Vector2 _shortColSize;
    [SerializeField] private Vector2 _tallColOffset;
    [SerializeField] private Vector2 _tallColSize;

    [SerializeField] private float _pushColTime;

    private TimeTravelManager timeTravelManager;
    private SpriteRenderer spriteRenderer;
    private Coroutine pushColliiderRoutine;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnChangeTimeZone(TimeZoneType changedTimeZone)
    {
        if (changedTimeZone == TimeZoneType.Past)
        {
            //spriteRenderer.sprite = _tallSprite;
            spriteRenderer.enabled = true;
            if (pushColliiderRoutine != null) StopCoroutine(pushColliiderRoutine);
            pushColliiderRoutine = StartCoroutine(PushCollider(_tallColOffset,_tallColSize));
        }
        else if (changedTimeZone == TimeZoneType.Present)
        {
            if (_waterfall.isPositiveDirection)
            {
                //spriteRenderer.sprite = _shortSprite;
                spriteRenderer.enabled = true;
                if (pushColliiderRoutine != null) StopCoroutine(pushColliiderRoutine);
                pushColliiderRoutine = StartCoroutine(PushCollider(_shortColOffset, _shortColSize));
            }
            else
            {
                spriteRenderer.enabled = false;
                if (pushColliiderRoutine != null) StopCoroutine(pushColliiderRoutine);
                pushColliiderRoutine = StartCoroutine(PushCollider(_noColOffset, _noColSize));
                
                Managers.Sound.PlayBGM("Ending");
            }
        }
    }
    
    private IEnumerator PushCollider(Vector2 targetOffset, Vector2 targetSize)
    {
        var box = GetComponent<BoxCollider2D>();
        var counter = 0f;
        var offsetDelta = (targetOffset - box.offset) / _pushColTime * Time.deltaTime;
        var sizeDelta = (targetSize - box.size) / _pushColTime * Time.deltaTime;

        while (counter < _pushColTime && sizeDelta.y > 0)
        {
            yield return null;
            counter += Time.deltaTime;
            box.size += sizeDelta;
            box.offset += offsetDelta;
        }

        box.offset = targetOffset;
        box.size = targetSize;
    }
}
