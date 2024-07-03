using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    List<float> savePointList = new List<float>();
    Dictionary<float, SavePoint> savePointDic = new Dictionary<float, SavePoint>();

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

    public Vector2 LoadSavePoint()
    {
        int savePointCnt = savePointList.Count;
        if (savePointCnt == 0) // Do not Have Save Point => 0,0 
            return Vector2.zero;
        if (savePointDic.ContainsKey(savePointList[savePointCnt - 1]))
            return savePointDic[savePointList[savePointCnt - 1]].SaveVec;
        return Vector2.zero;
    }

    public void AddSavePoint(SavePoint _savePoint)
    {
        savePointDic.Add(_savePoint.SaveVec.x,_savePoint);
        savePointList.Add(_savePoint.SaveVec.x);
        savePointList.Sort();
    }
}
