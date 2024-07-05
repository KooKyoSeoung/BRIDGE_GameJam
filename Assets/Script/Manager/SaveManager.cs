using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] SavePointData savePointData;
    public SavePointData LoadData { get { return savePointData; } }

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
    
    public void SaveData(Vector2 _pos, TimeZoneType _timeZoneType)
    {
        if(savePointData.savePoint.x > _pos.x)
            return;
        savePointData.savePoint = _pos;
        savePointData.saveTime = _timeZoneType;
    }
}
