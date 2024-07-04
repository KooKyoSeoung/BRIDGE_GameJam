using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] InGameUI inGameUI;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = inGameUI.gameObject.activeSelf;
            if (isActive)
                inGameUI.gameObject.SetActive(false);
            else
                inGameUI.gameObject.SetActive(true);
        }    
    }
}
