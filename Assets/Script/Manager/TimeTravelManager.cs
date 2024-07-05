using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelManager : MonoBehaviour
{
    // Singleton
    public static TimeTravelManager Instance;

    [Header("기획 Part")]
    [SerializeField, Tooltip("시작하는 타임라인")] TimeZoneType currentTimeZone;
    public TimeZoneType CurrentTimeZone { get { return currentTimeZone; } set { currentTimeZone = value;} }

    [Header("프로그래밍 Part")]
    [SerializeField, Tooltip("0:과거, 1:현재")] TimeTravelMap[] timeTravelMaps;
    [SerializeField] GameObject interactionParent;

    List<TimeTravelItem> timeTravelItemList = new List<TimeTravelItem>();
    
    #region Unity Life Cycle
    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        // Link Travel Items
        TimeTravelItem[] travelItems = interactionParent.GetComponentsInChildren<TimeTravelItem>();
        int travelCnt = travelItems.Length;
        for(int idx=0; idx<travelCnt; idx++)
        {
            timeTravelItemList.Add(travelItems[idx]);
        }
    }

    private void Start()
    {
        // Set TimeZone Once
        if (currentTimeZone == TimeZoneType.None)
            return;
        ChangeTimeZone(currentTimeZone);
    }
    #endregion

    /// <summary>
    /// Use This When you Change TimeZone
    /// </summary>
    public void ChangeTimeZone(TimeZoneType _changeType , TimeTravelItem _excludeTimeTravelItem = null)
    {
        currentTimeZone = _changeType;
        ChangeTimeZoneItem(_excludeTimeTravelItem);
        ChangeTimeZoneMap();
    }

    #region Change TimeZone
    public void ChangeTimeZoneItem(TimeTravelItem _excludeTimeTravelItem = null)
    {
        int timeTravelItemCnt = timeTravelItemList.Count;
        if (_excludeTimeTravelItem == null) {
            for (int idx = 0; idx < timeTravelItemCnt; idx++)
            {
                if (!timeTravelItemList[idx].ApplyTimeZone(currentTimeZone))
                {
                    if (timeTravelItemList[idx].ItemTimeZone == currentTimeZone)
                        timeTravelItemList[idx].gameObject.SetActive(true);
                    else
                        timeTravelItemList[idx].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int idx = 0; idx < timeTravelItemCnt; idx++)
            {
                TimeTravelItem _travelItem = timeTravelItemList[idx];
                if (_travelItem == _excludeTimeTravelItem)
                    continue;
                if (!timeTravelItemList[idx].ApplyTimeZone(currentTimeZone))
                {
                    if (timeTravelItemList[idx].ItemTimeZone == currentTimeZone)
                        timeTravelItemList[idx].gameObject.SetActive(true);
                    else
                        timeTravelItemList[idx].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ChangeTimeZoneMap()
    {
        if (currentTimeZone == TimeZoneType.Past)
        {
            timeTravelMaps[0].ApplyTimeZone();
            timeTravelMaps[1].ApplyTimeZone();
        }
        else if (currentTimeZone == TimeZoneType.Present)
        {
            timeTravelMaps[1].ApplyTimeZone();
            timeTravelMaps[0].ApplyTimeZone();
        }
    }
    #endregion
}
