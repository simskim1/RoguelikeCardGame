using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IEndDragHandler//, IDragHandler
{
    private Vector3 returnParentPos; // 드래그 취소 시 돌아갈 위치
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        returnParentPos = this.transform.position;
        // 드래그 중에는 마우스 클릭이 카드를 통과해서 바닥(적)을 감지할 수 있게 함
        canvasGroup.blocksRaycasts = false;
    }

    /*public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치를 따라 카드 이동
        this.transform.position = eventData.position;
    }*/

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝나면 다시 마우스 클릭을 감지하도록 설정
        canvasGroup.blocksRaycasts = true;

        // 적 위에 놓지 않았다면 원래 위치로 복귀 (나중에 로직 추가)
        this.transform.position = returnParentPos;
    }
}