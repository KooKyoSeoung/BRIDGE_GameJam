using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTriggerInputController : MonoBehaviour
{
    [Space(20), Header("플밍 Part")]
    // Load Position : transform.position = SaveManager.Instance.LoadSavePoint();
    [SerializeField] private float maxInteractDistance = 1f;
    //아웃라인이 하이라이트 되어있는 (하지만 아직 상호작용중이지는 않은) 물체.
    public Interactable currentFocusedInteractable;
    //실제로 상호작용 중인 물체.
    public Interactable currentInteractingObject;
    [SerializeField] private Color focusHighlightColor = Color.white;

    // 플레이어에 붙어있는 콜리더 : 바닥을 감지하는 것을 방지하기 위해 작은 콜리더를 하나 더 추가한 것
    [SerializeField] BoxCollider2D timeCheckColl; 
    // 스페이스바를 계속 누르는 상태를 방지하기 위한 Bool 변수
    bool isOverlapSpace = false;
    // 다른 시간대의 물체들과 겹치는지 확인하는 Bool 변수
    bool isOverlapMap = false;
    public bool IsOverlapMap { get { return isOverlapMap; } set { isOverlapMap = value; } }
    
    [Space(20), Header("기획 Part")]
    [SerializeField, Tooltip("시간여행을 하기 위해 걸리는 시간 : 스페이스바를 계속 누르는 시간")] float pressSpaceTime;
    float pressSpaceTimer = 0f;

    void Update()
    {
        SearchForInteractableToFocus();

        // 상호작용중인 물체가 없고 하이라이트된 물체가 있는 상태에서 F키
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentFocusedInteractable != null && currentInteractingObject == null) 
            {
                if (currentFocusedInteractable.interactableType != InteractableType.QuickInteraction) //QuickInteraction 타입은 단발적인 interaction이므로 상호작용'중'인 오브젝트로 추가하지 않는다.
                {
                    currentInteractingObject = currentFocusedInteractable;
                    SetFocusInteractable(null);
                }
                currentInteractingObject.StartInteraction();
            }
            else if (currentInteractingObject != null)
            {
                currentInteractingObject.EndInteraction();
                currentInteractingObject = null;
            }
        }

        if (currentInteractingObject != null && currentInteractingObject.IsHeavyItemDrop)
            currentInteractingObject = null;

        if (currentInteractingObject != null && currentInteractingObject.IsRopeJumped)
            currentInteractingObject = null;

        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 스토리 텍스트 
            if (DialogueManager.Instance.Dialogue_Trigger != null)
            {
                DialogueManager.Instance.Dialogue_Trigger.Interaction();
                return;
            }
            // 시간 여행 경고 : 겹치는 물체가 있으면 경고 메시지 출력 
            if (isOverlapMap /*|| isOverlapSpace*/ /*|| !TimeTravelManager.Instance.CheckTimeTravelCollide()*/)
            {
                if (!DialogueManager.Instance.IsDialogue)
                    UIController.Instance.TimeTravelWarn_UI.Warning();
            }
        }

        // 시간 여행 
        if (!isOverlapSpace && !DialogueManager.Instance.IsDialogue && Input.GetKey(KeyCode.Space))
        {
            pressSpaceTimer += Time.deltaTime;
            ChangeTimeZone();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isOverlapSpace = false;
            pressSpaceTimer = 0f;
        }
    }

    public void ChangeTimeZone()
    {
        if(isOverlapMap)
        {
            Warning();
            isOverlapSpace = true;
            return;
        }
        else
        {
            TimeTravelItem currentTravelItem = null;
            if (currentInteractingObject != null)
            {
                currentTravelItem = currentInteractingObject.GetComponent<TimeTravelItem>();
                if (!currentTravelItem.CheckCollide())
                {
                    isOverlapSpace = true;
                    Warning();
                    return;
                }
            }
        }
        // 시간여행 시작
        if (pressSpaceTimer >= pressSpaceTime)
        {
            isOverlapSpace = true;
            if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
            {
                TimeTravelItem currentTravelItem = null;
                if (currentInteractingObject != null)
                    currentTravelItem = currentInteractingObject.GetComponent<TimeTravelItem>();
                TimeTravelManager.Instance.ChangeTimeZone(TimeZoneType.Present, currentTravelItem);
            }
            else if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
            {
                TimeTravelItem currentTravelItem = null;
                if (currentInteractingObject != null)
                    currentTravelItem = currentInteractingObject.GetComponent<TimeTravelItem>();
                TimeTravelManager.Instance.ChangeTimeZone(TimeZoneType.Past, currentTravelItem);
            }
        }
    }

    private void SearchForInteractableToFocus()
    {
        //만약 상호작용 중인 물체가 이미 있으면 포커스를 찾지 않음.
        if (currentInteractingObject != null) return;
        
        var currentTimezone = TimeTravelManager.Instance.CurrentTimeZone;
        var playerCol = GetComponent<CapsuleCollider2D>();

        var allNearInteractables = Physics2D.OverlapCircleAll(transform.position, maxInteractDistance)
            .Where(x => x.GetComponent<Interactable>() != null)
            .Select(x => x.GetComponent<Interactable>()) //Interactable만 남김
            .Where(x => 
                x.GetComponent<TimeTravelItem>().ItemTimeZone == currentTimezone || 
                x.GetComponent<TimeTravelItem>().ItemTimeZone == TimeZoneType.AllTime) //현재 시간대와 같은 시간대의 물체만 남김
            .OrderBy(x => ((Vector2) transform.position -  (Vector2) x.transform.position).magnitude) //플레이어로부터 거리 오름차순으로 정렬
            .ToList();

        if (allNearInteractables.Count > 0)
            SetFocusInteractable(allNearInteractables[0]);
        else
            SetFocusInteractable(null);
    }

    private void SetFocusInteractable(Interactable interactable)
    {
        if (currentFocusedInteractable != null && currentFocusedInteractable != interactable)
        {
            //아웃라인 효과 제거
            UIController.Instance.indicatorTrigger.OnOffIndicator(false, this.transform);
            currentFocusedInteractable.GetComponent<SpriteRenderer>().material.SetFloat("_OutlinePixelWidth", 0);
        }

        currentFocusedInteractable = interactable;

        if (currentFocusedInteractable != null)
        {
            //아웃라인 효과 추가.
            UIController.Instance.indicatorTrigger.OnOffIndicator(true, this.transform);
            currentFocusedInteractable.GetComponent<SpriteRenderer>().material.SetFloat("_OutlinePixelWidth", 1);
        }
    }

    #region Trigger 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (timeCheckColl.IsTouching(collision))
            {
                // 맵과 플레이어가 겹치는 경우
                isOverlapMap = true;
            }
            else
            {
                if (currentInteractingObject == null)
                {
                    // 상호작용하는 물체가 없는 경우
                    isOverlapMap = false;
                }
                else if (currentInteractingObject.interactableType == InteractableType.HeavyMovable)
                {
                    // 상호작용하는 물체도 겹치는지 확인
                    BoxCollider2D[] boxColls = currentInteractingObject.GetComponentsInChildren<BoxCollider2D>();
                    int boxCnt = boxColls.Length;
                    if (boxCnt == 0) 
                    {
                        Debug.LogWarning("상호작용 중인 물체는 있지만 BoxCollider가 없음.");
                        // 예외처리
                    }
                    else
                    {
                        boxColls[boxCnt - 1].isTrigger = true;
                        // 상호작용 물체와 맵의 충돌 체크
                        if (boxColls[boxCnt - 1].IsTouching(collision))
                            isOverlapMap = true;
                        else
                            isOverlapMap = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isOverlapMap = false;
    }

    public void Warning()
    {
        if (isOverlapMap)
        {
            if (!DialogueManager.Instance.IsDialogue)
                UIController.Instance.TimeTravelWarn_UI.Warning();
        }
    }
    #endregion
}
