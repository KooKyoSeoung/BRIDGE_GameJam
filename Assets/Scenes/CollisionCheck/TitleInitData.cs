using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInitData : MonoBehaviour
{
    [SerializeField] SavePointData savePointData;

    private void Awake()
    {
        if (savePointData != null)
        {
            savePointData.Init();
        }
    }
}
