using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCam;

    bool isOnce = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOnce)
        {
            isOnce = false;
            CameraManager.Instance.BlendCamera(virtualCam);
        }
    }
}
