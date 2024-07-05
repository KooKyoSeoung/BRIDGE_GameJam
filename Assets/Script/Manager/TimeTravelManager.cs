using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTravelManager : MonoBehaviour
{
    // Singleton
    public static TimeTravelManager Instance;

    [Header("��ȹ Part")]
    [SerializeField, Tooltip("�����ϴ� Ÿ�Ӷ���")] TimeZoneType currentTimeZone;
    public TimeZoneType CurrentTimeZone { get { return currentTimeZone; } }

    [Header("���α׷��� Part")]
    [SerializeField, Tooltip("0:����, 1:����")] TimeTravelMap[] timeTravelMaps;
    [SerializeField] GameObject interactionParent;

    List<TimeTravelItem> timeTravelItemList = new List<TimeTravelItem>();
    public PlayerTriggerInputController PlayerTrigger { get; set; } = null; 
    private WeatheringRock weatheringRock;
    private TimeTravelItem[] backgroundTimes;
    
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
        // Link Player
        GameObject player = GameObject.FindWithTag("Player");
        PlayerTrigger = player.GetComponent<PlayerTriggerInputController>();
    }

    private void Start()
    {
        // Set TimeZone Once
        if (currentTimeZone == TimeZoneType.None)
            return;
        
        //풍화 바위 찾기
        weatheringRock = FindObjectOfType<WeatheringRock>();
        if (weatheringRock == null) Debug.LogWarning("WeatheringRock is Null. 메인 레벨이 있는 씬이 아니라면 무시해도 무방합니다.");
        
        //백그라운드 찾기
        backgroundTimes = FindObjectsOfType<ParallaxBackground>().Select(x => x.GetComponent<TimeTravelItem>())
            .ToArray();
        if (backgroundTimes.Length < 2) Debug.LogWarning("ParallaxBackground가 두개 미만입니다. 메인 씬이 아니라면 무시해도 무방합니다.");
        
        // Load Save Data
        TimeZoneType _curTimeZone = SaveManager.Instance.LoadData.saveTime;
        PlayerTrigger.gameObject.transform.position = SaveManager.Instance.LoadData.savePoint;
        ChangeTimeZone(_curTimeZone);
    }
    #endregion

    public void LoadTimeData()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Use This When you Change TimeZone
    /// </summary>
    public void ChangeTimeZone(TimeZoneType _changeType , TimeTravelItem _excludeTimeTravelItem = null)
    {
        currentTimeZone = _changeType;
        ChangeTimeZoneItem(_excludeTimeTravelItem);
        ChangeTimeZoneMap();

        if (weatheringRock != null)
        {
            weatheringRock.OnChangeTimeZone(_changeType);
        }

        if (backgroundTimes.Length == 2)
        {
            foreach (var backgroundTime in backgroundTimes)
            {
                backgroundTime.gameObject.SetActive(currentTimeZone == backgroundTime.ItemTimeZone);
            }
        }
    }

    #region Change TimeZone
    public void ChangeTimeZoneItem(TimeTravelItem _excludeTimeTravelItem = null)
    {
        int timeTravelItemCnt = timeTravelItemList.Count;
        
        if (_excludeTimeTravelItem == null)  // ������ ��ǰ�� ���� ���
        {
            for (int idx = 0; idx < timeTravelItemCnt; idx++)
            {
                if (!timeTravelItemList[idx].ApplyAllTimeZone(currentTimeZone))
                {
                    if (timeTravelItemList[idx].ItemTimeZone == currentTimeZone)
                        timeTravelItemList[idx].ApplyTimeZone(true);
                    else
                        timeTravelItemList[idx].ApplyTimeZone(false);
                }
            }
        }
        else // ������ ��ǰ�� �ִ� ���
        {
            for (int idx = 0; idx < timeTravelItemCnt; idx++)
            {
                TimeTravelItem _travelItem = timeTravelItemList[idx];
                if (_travelItem == _excludeTimeTravelItem)
                {
                    _travelItem.ItemTimeZone = currentTimeZone;
                    continue;
                }
                if (!timeTravelItemList[idx].ApplyAllTimeZone(currentTimeZone))
                {
                    if (timeTravelItemList[idx].ItemTimeZone == currentTimeZone)
                        timeTravelItemList[idx].ApplyTimeZone(true);
                    else
                        timeTravelItemList[idx].ApplyTimeZone(false);
                }
            }
        }
    }

    public void ChangeTimeZoneMap()
    {
        if (currentTimeZone == TimeZoneType.Past)
        {
            timeTravelMaps[0].ApplyAllTimeZone();
            timeTravelMaps[1].ApplyAllTimeZone();
        }
        else if (currentTimeZone == TimeZoneType.Present)
        {
            timeTravelMaps[1].ApplyAllTimeZone();
            timeTravelMaps[0].ApplyAllTimeZone();
        }
    }

    public void RemoveTravelItem(TimeTravelItem _timeTravelItem)
    {
        int itemCnt = timeTravelItemList.Count;
        for(int idx = 0; idx<itemCnt; idx++)
        {
            if(_timeTravelItem == timeTravelItemList[idx])
            {
                Destroy(timeTravelItemList[idx].gameObject);
                timeTravelItemList.RemoveAt(idx);
                return;
            }
        }
    }
    #endregion
}
