using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTriggerInputController : MonoBehaviour
{
    // Load Position : transform.position = SaveManager.Instance.LoadSavePoint();
    [SerializeField] private float maxInteractDistance = 1f;
    //아웃라인이 하이라이트 되어있는 (하지만 아직 상호작용중이지는 않은) 물체.
    public Interactable currentFocusedInteractable;
    //실제로 상호작용 중인 물체.
    public Interactable currentInteractingObject;

    [SerializeField] private Color focusHighlightColor = Color.white;


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

        // �ð� ��ȭ
        if (Input.GetKeyDown(KeyCode.Space) && !DialogueManager.Instance.IsDialogue)
        {
            ChangeTimeZone();
        }
    }

    public void ChangeTimeZone()
    {
        if (DialogueManager.Instance.Dialogue_Trigger != null)
        {
            DialogueManager.Instance.Dialogue_Trigger.Interaction();
            return;
        }
        if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Present;
        }
        else if(TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
        {
            TimeTravelManager.Instance.CurrentTimeZone = TimeZoneType.Past;
        }
    }

    private void SearchForInteractableToFocus()
    {
        //만약 상호작용 중인 물체가 이미 있으면 포커스를 찾지 않음.
        if (currentInteractingObject != null) return;

        var allNearInteractables = Physics2D.OverlapCircleAll(transform.position, maxInteractDistance)
            .Where(x => x.GetComponent<Interactable>() != null)
            .Select(x => x.GetComponent<Interactable>()) //Interactable만 남김
            .OrderBy(x => (Vector2) transform.position -  (Vector2) x.transform.position) //플레이어로부터 거리 오름차순으로 정렬
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
            currentFocusedInteractable.GetComponent<SpriteRenderer>().material.SetFloat("_OutlinePixelWidth", 0);
        }

        currentFocusedInteractable = interactable;

        if (currentFocusedInteractable != null)
        {
            //아웃라인 효과 추가.
            currentFocusedInteractable.GetComponent<SpriteRenderer>().material.SetFloat("_OutlinePixelWidth", 1);
        }
    }
}
