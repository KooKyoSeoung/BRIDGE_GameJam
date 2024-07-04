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

    [Header("플밍 Part")]
    [SerializeField, Tooltip("과거 사진")] Sprite pastTimeZoneSprite;
    [SerializeField, Tooltip("현재 사진")] Sprite presentTimeZoneSprite;
    // Component
    SpriteRenderer spr;
    
    #region Unity Life Cycle
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    #endregion

    public bool ApplyTimeZone(TimeZoneType _currentTimeZone)
    {
        // AllTime
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
            return true;
        }
        return false;
    }
}
