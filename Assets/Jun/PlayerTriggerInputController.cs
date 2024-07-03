using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInputController : MonoBehaviour
{
    public TimeTravelItem ReachItem { get; set; } = null;

    #region Unity Life Cycle
    void Update()
    {
        // 아이템 획득
        if (Input.GetKeyDown(KeyCode.F) && ReachItem != null)
        {
            ReachItem.GetItem();
            ReachItem = null;
        }
        // 시간 변화
        if (Input.GetKeyDown(KeyCode.Space) && !DialogueManager.Instance.IsDialogue)
        {
            ChangeTimeZone();
        }

        // 리셋 기능
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = SaveManager.Instance.LoadSavePoint();
        }
    }

    public void ChangeTimeZone()
    {
        if (DialogueManager.Instance.Dialogue_Trigger != null)
        {
            DialogueManager.Instance.Dialogue_Trigger.Interaction();
            return;
        }
        if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Present;
        }
        else if(TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Past;
        }
    }
    #endregion
}
