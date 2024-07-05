using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorTrigger : MonoBehaviour
{
    [SerializeField] float upValue;
    [SerializeField] GameObject indicatorTriggerObject;

    private Transform _targetTransform;
    private bool _isActive = false;

    private void Start()
    {
        DialogueManager.Instance.Indicator_Trigger = this;
    }

    public void OnOffIndicator(bool _isActive, Transform _target)
    {
        this._isActive = _isActive;
        _targetTransform = _target;
        if (_isActive && _target != null)
        {
            indicatorTriggerObject.transform.position = (Vector2)_targetTransform.position + Vector2.up * upValue;
        }
        indicatorTriggerObject.gameObject.SetActive(_isActive);

    }

    private void Update()
    {
        if (_isActive)
        {
            indicatorTriggerObject.transform.position = (Vector2)_targetTransform.position + Vector2.up * upValue;
        }
    }
}
