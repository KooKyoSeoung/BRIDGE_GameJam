using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    [SerializeField] private Transform controlRock;
    [SerializeField] private Transform leftWaterfall;
    [SerializeField] private Transform rightWaterfall;

    [SerializeField] private float waterfallCenterOffset;
    [SerializeField] private float waterfallWidth;
    
    private float WaterfallCenterX => transform.position.x + waterfallCenterOffset;

    [HideInInspector] public bool isPositiveDirection = true;

    private void Start()
    {
        SetDirection(true);
    }

    void Update()
    {
        if (isPositiveDirection && controlRock.position.x > WaterfallCenterX + waterfallWidth/2f)
        {
            SetDirection(false);
        }
        else if (!isPositiveDirection && controlRock.position.x < WaterfallCenterX - waterfallWidth/2f)
        {
            SetDirection(true);
        }
    }
    
    private void SetDirection(bool isPositive)
    {
        rightWaterfall.gameObject.SetActive(isPositive);
        leftWaterfall.gameObject.SetActive(!isPositive);
        
        isPositiveDirection = isPositive;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.right * waterfallCenterOffset, 0.1f);
    }
}
