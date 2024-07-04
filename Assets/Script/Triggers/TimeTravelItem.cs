using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0,1 = timezone, 2(item only)
/// </summary>
public enum TimeZoneType
{
    Past = 0,
    Present = 1,
    AllTime = 2,
    None = 3
}

public class TimeTravelItem : MonoBehaviour
{
    [Header("기획 Part")]
    [SerializeField, Tooltip("아이템이 존재하는 시간대")] TimeZoneType itemTimeZone;
    public TimeZoneType ItemTimeZone { get { return itemTimeZone; } }
    [SerializeField, Tooltip("과거 사진")] Sprite pastTimeZoneSprite;
    [SerializeField, Tooltip("현재 사진")] Sprite presentTimeZoneSprite;

    SpriteRenderer spr;
    [SerializeField] bool canInteraction = true;
    public bool CanInteraction { get { return canInteraction; } }

    #region Unity Life Cycle
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InitInformation();
    }

    /// <summary>
    /// Call Once When you play game
    /// </summary>
    public void InitInformation()
    {
        TimeTravelManager.Instance.TimeTravelItemList.Add(this);
        TimeZoneType _currentTimeZone = TimeTravelManager.Instance.CurrentTimeZone;
        switch (itemTimeZone)
        {
            case TimeZoneType.Past:
                if (_currentTimeZone == TimeZoneType.Present)
                {
                    gameObject.SetActive(false);
                }
                break;
            case TimeZoneType.Present:
                if (_currentTimeZone == TimeZoneType.Past)
                {
                    gameObject.SetActive(false);
                }
                break;
            case TimeZoneType.AllTime:
                if (_currentTimeZone == TimeZoneType.Past)
                {
                    spr.sprite = pastTimeZoneSprite;
                }
                else if (_currentTimeZone == TimeZoneType.Present)
                {
                    spr.sprite = presentTimeZoneSprite;
                }
                break;
        }
    }

    #endregion

    public void ApplyTimeZone(TimeZoneType _currentTimeZone)
    {
        if(itemTimeZone==TimeZoneType.AllTime)
        {
            if(_currentTimeZone== TimeZoneType.Past)
            {
                spr.sprite = pastTimeZoneSprite;
            }
            else if (_currentTimeZone == TimeZoneType.Present)
            {
                spr.sprite = presentTimeZoneSprite;
            }
        }
    }

    public void GetItem()
    {
        canInteraction = false;
        gameObject.SetActive(false);
        // Send Item Data 
    }
}
