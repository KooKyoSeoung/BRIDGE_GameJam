using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    bool once = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)&&once)
        {
            once = false;
            CameraManager.Instance.BlendCamera(virtualCamera);
        }    
    }
}
