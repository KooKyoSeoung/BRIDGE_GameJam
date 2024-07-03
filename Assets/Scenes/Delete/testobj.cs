using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testobj : MonoBehaviour
{
    public TimeTravelItem ReachItem { get; set; } = null;
    void Update()
    {
        // 조작
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
        }

        // 로드

        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.position = SaveManager.Instance.LoadSavePoint();
        }

        // 아이템 획득
        if (Input.GetKeyDown(KeyCode.F) && ReachItem!=null)
        {
            ReachItem.GetItem();
            ReachItem = null;
        }

        // 시간 변화
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeTimeZone();
        }
    }

    public void ChangeTimeZone()
    {
        if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Present;
        }
        else if(TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Past;
        }
    }
}
