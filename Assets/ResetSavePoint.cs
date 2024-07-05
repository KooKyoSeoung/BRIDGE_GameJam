using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSavePoint : MonoBehaviour
{
    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private TimeZoneType startingTimeZone;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SaveManager.Instance.SaveData(startingPosition, startingTimeZone, true);
        }
    }
}
