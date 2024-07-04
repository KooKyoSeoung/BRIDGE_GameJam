using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInputController : MonoBehaviour
{
    [Header("기획 Part")]
    [SerializeField, Tooltip("시간여행 키 누르는 시간")] float travelPressTime;
    
    public TimeTravelItem ReachItem { get; set; } = null;

    bool isTimeTravel = false; 
    float travelPressTimer = 0;

    #region Unity Life Cycle
    void Update()
    {
        #region GetItem
        if (Input.GetKeyDown(KeyCode.F) && ReachItem != null)
        {
            ReachItem.GetItem();
            ReachItem = null;
        }
        #endregion

        #region Time Travel
        if (!isTimeTravel && Input.GetKey(KeyCode.Space) && !DialogueManager.Instance.IsDialogue)
        {
            travelPressTimer += Time.deltaTime;
            ChangeTimeZone();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            travelPressTimer = 0;
            isTimeTravel = false;
        }
        #endregion

        #region Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = SaveManager.Instance.LoadSavePoint();
        }
        #endregion
    }

    public void ChangeTimeZone()
    {
        if (DialogueManager.Instance.Dialogue_Trigger != null)
        {
            DialogueManager.Instance.Dialogue_Trigger.Interaction();
            return;
        }
        if (travelPressTimer >= travelPressTime)
        {
            isTimeTravel = true;
            if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
            {
                TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Present;
            }
            else if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
            {
                TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Past;
            }
        }
    }
    #endregion
}
