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
    public TimeZoneType ItemTimeZone { get { return itemTimeZone; } set { itemTimeZone = value; } }

    [Header("플밍 Part")]
    [SerializeField, Tooltip("과거 사진")] Sprite pastTimeZoneSprite;
    [SerializeField, Tooltip("현재 사진")] Sprite presentTimeZoneSprite;
    // Component
    SpriteRenderer spr;
    Collider2D[] itemColliders;
    Rigidbody2D rb;
    // Value
    RigidbodyType2D defaultType;
    bool isItemActive = false;


    #region Unity Life Cycle
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        itemColliders = GetComponentsInChildren<Collider2D>();
        if (rb != null)
            defaultType = rb.bodyType;
    }
    #endregion

    public bool ApplyAllTimeZone(TimeZoneType _currentTimeZone)
    {
        // AllTime
        if (itemTimeZone == TimeZoneType.AllTime)
        {
            if (_currentTimeZone == TimeZoneType.Past)
            {
                spr.sprite = pastTimeZoneSprite;
            }
            else if (_currentTimeZone == TimeZoneType.Present)
            {
                spr.sprite = presentTimeZoneSprite;
            }
            isItemActive = true;
            return true;
        }
        return false;
    }

    public void ApplyTimeZone(bool _isActive)
    {
        if (_isActive)
        {
            if(rb!=null)
                rb.bodyType = defaultType;
            itemColliders[0].enabled = true;
            spr.enabled = true;
            isItemActive = true;
        }
        else
        {
            if(rb!=null)
                rb.bodyType = RigidbodyType2D.Static;
            itemColliders[0].enabled = false;
            spr.enabled = false;
            isItemActive = false;
        }
    }

    public bool CheckCollide()
    {
        ContactFilter2D contact = new ContactFilter2D();
        contact.useTriggers = true;
        contact.SetLayerMask(Physics2D.AllLayers);
        List<Collider2D> collList = new List<Collider2D>();
        itemColliders[1].OverlapCollider(contact, collList);
        int listCnt = collList.Count;
        for(int idx=1; idx<listCnt; idx++)
        {
            if (collList[idx].CompareTag("Ground"))
            {
                Debug.Log(collList[idx].name);
                return false;
            }
        }
        return true;
    }
}
