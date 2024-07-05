using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorTrigger : MonoBehaviour
{
    [SerializeField] float upValue;
    [SerializeField] GameObject indicatorTriggerObject;
    [SerializeField] TextMeshProUGUI tmp;
    private Transform _targetTransform;
    private bool _isActive = false;

    public void OnOffIndicator(bool _isActive, Transform _target, string _text="")
    {
        this._isActive = _isActive;
        _targetTransform = _target;
        SetText(_text);
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

    public void SetText(string _text)
    {
        tmp.text = _text;
    }
}
