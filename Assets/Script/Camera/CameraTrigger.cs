using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCam;

    bool isOnce = true;

    private void Update()
    {
        if (isOnce && Input.GetKeyDown(KeyCode.Alpha1))
        {
            isOnce = false;
            CameraManager.Instance.BlendCamera(virtualCam);
        }
    }
}
