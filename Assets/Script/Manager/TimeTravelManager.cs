using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelManager : MonoBehaviour
{
    public static TimeTravelManager Instance;

    [SerializeField] PlayerTriggerInputController playerTrigger = null;
    public PlayerTriggerInputController PlayerTrigger 
    { 
        get 
        {
            if (playerTrigger == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                playerTrigger = player.GetComponent<PlayerTriggerInputController>();
            }        
            return playerTrigger; 
        } 
    } 

    [SerializeField] TimeZoneType currentTimeZone;
    [SerializeField, Tooltip("0:과거, 1:현재")] TimeTravelMap[] timeTravelMaps;
    public TimeZoneType CurrentTimeZone { get { return currentTimeZone; } set { currentTimeZone = value; ChangeTimeZone(); } }

    List<TimeTravelItem> timeTravelItemList = new List<TimeTravelItem>();
    public List<TimeTravelItem> TimeTravelItemList { get { return timeTravelItemList; } set { timeTravelItemList = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Call When you change TimeLine
    /// </summary>
    public void ChangeTimeZone()
    {
        ChangeTimeZoneItem();
        ChangeTimeZoneMap();
    }

    public void ChangeTimeZoneItem()
    {
        int timeTravelItemCnt = timeTravelItemList.Count;
        for (int idx = 0; idx < timeTravelItemCnt; idx++)
        {
            TimeTravelItem _travelItem = timeTravelItemList[idx];
            TimeZoneType _itemTimeZone = _travelItem.ItemTimeZone;
            switch (_itemTimeZone)
            {
                case TimeZoneType.Past:
                    if (currentTimeZone == TimeZoneType.Present)
                    {
                        _travelItem.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (_travelItem.CanInteraction)
                        {
                            _travelItem.gameObject.SetActive(true);
                            _travelItem.ApplyTimeZone(currentTimeZone);
                        }
                    }
                    break;
                case TimeZoneType.Present:
                    if (currentTimeZone == TimeZoneType.Past)
                    {
                        _travelItem.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (_travelItem.CanInteraction)
                        {
                            _travelItem.gameObject.SetActive(true);
                            _travelItem.ApplyTimeZone(currentTimeZone);
                        }
                    }
                    break;
                case TimeZoneType.AllTime:
                    _travelItem.ApplyTimeZone(currentTimeZone);
                    break;
            }
        }
    }

    public void ChangeTimeZoneMap()
    {
        if (currentTimeZone == TimeZoneType.Past)
        {
            timeTravelMaps[(int)TimeZoneType.Past].gameObject.SetActive(true);
            timeTravelMaps[(int)TimeZoneType.Present].ChangeOrderInLayer(0);
            timeTravelMaps[(int)TimeZoneType.Present].gameObject.SetActive(false);
            timeTravelMaps[(int)TimeZoneType.Past].ChangeOrderInLayer(1);
        }
        else if (currentTimeZone == TimeZoneType.Present)
        {
            timeTravelMaps[(int)TimeZoneType.Present].gameObject.SetActive(true);
            timeTravelMaps[(int)TimeZoneType.Past].gameObject.SetActive(false);
            timeTravelMaps[(int)TimeZoneType.Past].ChangeOrderInLayer(0);
            timeTravelMaps[(int)TimeZoneType.Present].ChangeOrderInLayer(1);
        }
    }
}
