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
    bool haveCollder=false;
    [SerializeField, Tooltip("과거 사진")] Sprite pastTimeZoneSprite;
    [SerializeField, Tooltip("현재 사진")] Sprite presentTimeZoneSprite;
   
    // Component
    SpriteRenderer spr;
    Collider2D[] itemColliders;
    Rigidbody2D rb;
    RigidbodyType2D defaultType;
    
    #region Unity Life Cycle
    private void Awake()
    {
        SetInteractableInformation();
        spr = GetComponent<SpriteRenderer>();
        if (haveCollder)
        {
            itemColliders = GetComponentsInChildren<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            gameObject.AddComponent<TimeTravelColliderChecker>();
            defaultType = rb.bodyType;
        }
    }

    public void SetInteractableInformation()
    {
        Interactable interactable = GetComponent<Interactable>();
        if (interactable == null)
            return;
        if (interactable.interactableType == InteractableType.HeavyMovable)
            haveCollder = true;
        else
            haveCollder = false;
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
            return true;
        }
        return false;
    }

    public void ApplyTimeZone(bool _isActive)
    {
        AudioChecker();
        if (haveCollder) // Have Collider : Heavy Movable
        {
            if (_isActive)
            {
                if (rb != null)
                    rb.bodyType = defaultType;
                itemColliders[0].enabled = true;
                spr.enabled = true;
            }
            else
            {
                if (rb != null)
                    rb.bodyType = RigidbodyType2D.Static;
                itemColliders[0].enabled = false;
                spr.enabled = false;
            }
        }
        else // Else Object
        {
            if (_isActive)
                spr.enabled = true;
            else
                spr.enabled = false;
        }
    }

    public bool CheckCollide()
    {
        if (!haveCollder)
            return true;
        ContactFilter2D contact = new ContactFilter2D();
        contact.useTriggers = true;
        contact.SetLayerMask(Physics2D.AllLayers);
        List<Collider2D> collList = new List<Collider2D>();
        itemColliders[1].OverlapCollider(contact, collList);
        int listCnt = collList.Count;
        for(int idx=1; idx<listCnt; idx++)
        {
            if (collList[idx].GetComponent<TimeTravelColliderChecker>() != null)
                return false;
            if (collList[idx].CompareTag("Ground"))
                return false;
        }
        return true;
    }

    public void AudioChecker()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio == null)
            return;
        if (spr.enabled==true)
            audio.enabled = true;
        else
            audio.enabled = false;
    }
}
