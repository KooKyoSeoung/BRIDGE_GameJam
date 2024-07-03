using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [Header("기획자 Part")]
    [SerializeField, Range(2f, 10f), Tooltip("다른 지점까지 도달하는 시간")] float reachTime;
    [SerializeField, Range(0f, 10f), Tooltip("멈춰있는 시간")] float stopTime;

    [Header("프로그래밍 Part")]
    int currentPoint = 0;
    int pointCnt = 0;
    [SerializeField] Vector2[] movePos;
    //WaitForSeconds delayTime;

    private void Awake()
    {
        transform.position = movePos[currentPoint];
        pointCnt = movePos.Length;
    }

    private void Start()
    {
        // 대기시간 픽스되면 사용
        //delayTime = new WaitForSeconds(stopTime);
        StartCoroutine(MovePlatformCor());
    }

    public IEnumerator MovePlatformCor()
    {
        float timer = 0f;
        Vector2 startPos = transform.position;
        currentPoint += 1;
        if (currentPoint >= pointCnt)
            currentPoint = 0;
        Vector2 destPos = movePos[currentPoint];

        while (timer<=reachTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, destPos, timer / reachTime);
            yield return null;
        }
        //yield return delayTime;
        yield return new WaitForSeconds(stopTime);
        StartCoroutine(MovePlatformCor());
    }

    #region ReStart or Stop
    public void StartMove()
    {
        StartCoroutine(MovePlatformCor());
    }

    public void StopMove()
    {
        StopCoroutine(MovePlatformCor());
    }
    #endregion
}
