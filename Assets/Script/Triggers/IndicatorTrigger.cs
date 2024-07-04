using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorTrigger : MonoBehaviour
{
    [SerializeField] float upValue;
    [SerializeField] GameObject indicatorTriggerObject;
    private void Start()
    {
        DialogueManager.Instance.Indicator_Trigger = this;
    }

    public void OnOffIndicator(bool _isActive, Vector2 _pos)
    {
        if (_isActive)
        {
            indicatorTriggerObject.transform.position = _pos;
            indicatorTriggerObject.transform.position += Vector3.up * upValue;
            indicatorTriggerObject.gameObject.SetActive(true);
        }
        else
        {
            indicatorTriggerObject.gameObject.SetActive(false);
        }
    }
}
