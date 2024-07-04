using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // SingleTon
    public static CameraManager Instance;

    [Header("기획 Part")]
    [SerializeField, Tooltip("오프셋 속도, 카메라 섞는 속도")] float blendSpeed;
    [SerializeField, Tooltip("다시 되돌아가는 시간")] float returnTime;
    [SerializeField, Tooltip("줌 인, 줌 아웃되는 속도")] float zoomSpeed;

    // Call Awake
    #region MainCamera 
    float defaultZoomSize;
    Camera mainCam;
    CinemachineBrain brain;
    #endregion

    // Call Start
    #region TargetCamera
    float targetZoomSize;
    CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera TargetCam { get; set; } = null;
    public CinemachineVirtualCamera CurrentCam { get; set; } = null;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        #region SingleTon
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        mainCam = Camera.main;
        brain = mainCam.GetComponent<CinemachineBrain>();
        defaultZoomSize = mainCam.orthographicSize;
    }

    private void Start()
    {
        CurrentCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        playerCam = CurrentCam;
    }
    #endregion

    #region Blend Camera
    public void BlendCamera(CinemachineVirtualCamera _virCam)
    {
        if (_virCam == null)
            return;
        TargetCam = _virCam;
        StartCoroutine(BlendTimeCor());
    }

    public IEnumerator BlendTimeCor()
    {
        if (TargetCam == null)
            yield break;
        brain.m_DefaultBlend.m_Time = blendSpeed;
        TargetCam.Priority = CurrentCam.Priority + 1;
        CurrentCam = TargetCam;
        yield return new WaitForSeconds(blendSpeed + returnTime);
        playerCam.Priority = 10;
        TargetCam.Priority = 0;
        TargetCam = null;
    }
    #endregion

    //public void SelectZoomState(bool _isZoomIn) 
    //{
    //    if (_isZoomIn)
    //    {
    //        StartCoroutine(ZoomInCor());
    //    }
    //    else
    //    {
    //        StartCoroutine(ZoomOutCor());
    //    }
    //}

    //public IEnumerator ZoomInCor()
    //{
    //    float timer = 0f;
    //    while (timer <= zoomSpeed)
    //    {
    //        timer += Time.deltaTime;
    //        CurrentCam.m_Lens.OrthographicSize = Mathf.Lerp(defaultZoomSize, targetZoomSize, timer / zoomSpeed);
    //        yield return null;
    //    }
    //}

    //public IEnumerator ZoomOutCor()
    //{
    //    yield return null;
    //}


}
