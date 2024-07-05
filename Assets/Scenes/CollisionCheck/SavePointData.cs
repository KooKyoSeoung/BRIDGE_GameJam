using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SavePointData",menuName ="SaveData/Point",order =(int.MaxValue))]
public class SavePointData : ScriptableObject
{
    public Vector2 savePoint;
    public TimeZoneType saveTime;
    public bool hasObtainedWatch;

    public void Init()
    {
        savePoint = Vector2.zero;
        saveTime = TimeZoneType.Present;
        hasObtainedWatch = false;
    }
}
