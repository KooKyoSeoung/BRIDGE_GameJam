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

[RequireComponent(typeof(SpriteOutline))]
public class TimeTravelItem : MonoBehaviour
{
    [Header("��ȹ Part")]
    [SerializeField, Tooltip("�������� �����ϴ� �ð���")] TimeZoneType itemTimeZone;
    public TimeZoneType ItemTimeZone { get { return itemTimeZone; } }

    [Header("���α׷��� Part")]
    [SerializeField, Tooltip("���� ����")] Sprite pastTimeZoneSprite;
    [SerializeField, Tooltip("���� ����")] Sprite presentTimeZoneSprite;

    bool canInteraction = true;
    public bool CanInteraction { get { return canInteraction; } }
    SpriteRenderer spr;
    SpriteOutline spriteOutline;

    #region Unity Life Cycle
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        spriteOutline = GetComponent<SpriteOutline>();
        spriteOutline.enabled = false;
    }

    private void OnEnable() // Prevene Exception
    {
        if (!canInteraction)
            gameObject.SetActive(false);
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

    /// <summary>
    /// Call By TimeTravelManager When you change TimeZone
    /// </summary>
    /// <param name="_currentTimeZone"></param>
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

    /// <summary>
    /// Delete Item & Get Item Data(To Do)
    /// </summary>
    public void GetItem()
    {
        canInteraction = false;
        gameObject.SetActive(false);
        // To Do : Send Item Data 
    }

    #region Collision Check (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TimeTravelManager.Instance.PlayerTrigger.ReachItem = this;
            DialogueManager.Instance.Indicator_Trigger.OnOffIndicator(true, transform.position);
            spriteOutline.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TimeTravelManager.Instance.PlayerTrigger.ReachItem = null;
            DialogueManager.Instance.Indicator_Trigger.OnOffIndicator(false, transform.position);
            spriteOutline.enabled = false;
        }
    }
    #endregion
}
