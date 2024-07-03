using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInputController : MonoBehaviour
{
    // Load Position : transform.position = SaveManager.Instance.LoadSavePoint();
    public TimeTravelItem ReachItem { get; set; } = null;
    void Update()
    {
        // ������ ȹ��
        if (Input.GetKeyDown(KeyCode.F) && ReachItem != null)
        {
            ReachItem.GetItem();
            ReachItem = null;
        }
        // �ð� ��ȭ
        if (Input.GetKeyDown(KeyCode.Space) && !DialogueManager.Instance.IsDialogue)
        {
            ChangeTimeZone();
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
}
