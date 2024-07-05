using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("��ȹ Part")]
    [SerializeField, Tooltip("������ �ӵ�")] float blendSpeed;
    [SerializeField, Tooltip("�ٽ� �ǵ��ư��� �ð�")] float returnTime;
    [SerializeField, Tooltip("�� ��, �� �ƿ��Ǵ� �ӵ�")] float zoomSpeed;

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
    public CinemachineVirtualCamera TargetCam;
    public CinemachineVirtualCamera CurrentCam;
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

    #region Blend Cam
    public void BlendCamera(CinemachineVirtualCamera _virCam)
    {
        TargetCam = _virCam;
        StartCoroutine(BlendTimeCor());
    }

    public IEnumerator BlendTimeCor()
    {
        if (TargetCam == null)
            yield break;
        brain.m_DefaultBlend.m_Time = blendSpeed;
        if(CurrentCam==null)
        {
            CurrentCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            playerCam = CurrentCam;
        }
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
