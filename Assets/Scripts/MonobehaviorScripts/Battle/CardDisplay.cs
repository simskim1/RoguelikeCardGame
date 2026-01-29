using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                           IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image frameImage;

    [Header("Frame Sprites")]
    [SerializeField] private Sprite attackFrame;
    [SerializeField] private Sprite skillFrame;
    [SerializeField] private Sprite powerFrame;

    [Header("Hover Settings")] // 3. 호버 설정 추가
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationDuration = 0.2f;

    private CardData cardData;//디스플레이에 카드의 정보를 저장
    private Vector3 originalScale;
    private int originalSiblingIndex;
    private Canvas cardCanvas;

    private TargetingArrow arrow;
    private Vector3 dragStartPosition;
    private Vector3 dragStartWorldPos;
    private Vector3 dragStartPos; // 카드 시작 위치 (로컬)
    private Vector2 pointerOffset; // 마우스 클릭 지점과 카드 중심의 차이

    private void Awake()
    {
        originalScale = transform.localScale;

        // 만약 에디터에서 실수로 scale을 0으로 해뒀을 경우를 대비한 방어 코드
        if (originalScale == Vector3.zero) originalScale = Vector3.one;
        cardCanvas = GetComponent<Canvas>();

        if (cardCanvas != null)
        {
            // 시작하자마자 켜줍니다. 
            // 이렇게 하면 모든 카드가 동일한 'Canvas 렌더링 규칙'을 따르게 됩니다.
            cardCanvas.overrideSorting = true;

            // 기본 sortingOrder를 0으로 고정합니다.
            cardCanvas.sortingOrder = 0;
        }
        arrow = TargetingArrow.Instance;
    }
    
    public void Setup(CardData data)
    {
        cardData = data;

        cardNameText.text = data.cardName;
        descriptionText.text = data.description;
        costText.text = data.energyCost.ToString();
        cardImage.sprite = data.cardImage;

        // [추가] 타입별 프레임 변경 로직
        UpdateFrame(data.cardType);
    }

    private void UpdateFrame(CardType type)
    {
        if (frameImage == null) return;

        switch (type)
        {
            case CardType.Attack: frameImage.sprite = attackFrame; break;
            case CardType.Skill: frameImage.sprite = skillFrame; break;
            case CardType.Power: frameImage.sprite = powerFrame; break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1. SiblingIndex 대신 Sorting Order를 높여서 맨 앞으로 가져오기
        if (cardCanvas != null)
        {
            cardCanvas.sortingOrder = 100; // 보통 0인 다른 카드들보다 높게 설정
        }

        // 2. 크기 키우기
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, animationDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 1. 원래 정렬 순서로 복구
        if (cardCanvas != null)
        {
            cardCanvas.sortingOrder = 0;
        }

        // 2. 원래 크기로 복구
        transform.DOKill();
        transform.DOScale(originalScale, animationDuration).SetEase(Ease.InQuad);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransform canvasRect = arrow.GetComponentInParent<Canvas>().transform as RectTransform;
        RectTransform parentRect = transform.parent as RectTransform; // Hand 패널
        Camera uiCamera = eventData.pressEventCamera;

        Vector2 tempPos;
        // --- 1. 화살표 시작점 계산 (Canvas 기준) ---
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(uiCamera, transform.position),
            uiCamera,
            out tempPos
        );

        dragStartPos = tempPos;
        // --- 2. 카드 드래그 오프셋 계산 (Hand 기준) ---
        // 마우스 위치를 카드의 부모인 'Hand' 기준 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            uiCamera,
            out Vector2 mouseInParentPos))
        {
            // 현재 카드의 localPosition과 마우스 위치의 차이를 저장 (튀는 현상 방지)
            pointerOffset = (Vector2)transform.localPosition - mouseInParentPos;
        }

        if (arrow != null)
        {
            arrow.lineRenderer.enabled = true;
            arrow.gameObject.SetActive(true);
        }

        cardCanvas.sortingOrder = 100;
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardCanvas.sortingOrder = 100;
        RectTransform canvasRect = arrow.GetComponentInParent<Canvas>().transform as RectTransform;
        RectTransform parentRect = transform.parent as RectTransform; // Hand 패널
        Camera uiCamera = eventData.pressEventCamera;

        // --- 1. 카드 이동 (Hand 기준 좌표 사용) ---
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            uiCamera,
            out Vector2 mouseInParentPos))
        {
            transform.localPosition = mouseInParentPos + pointerOffset;
        }

        // --- 2. 화살표 그리기 (Canvas 기준 좌표 사용) ---
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            uiCamera,
            out Vector2 mouseInCanvasPos))
        {
            arrow.DrawCurve(dragStartPos, mouseInCanvasPos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)//포물선의 캐릭터 위에 떨어졌을 때의 삭제는 여기가 아닌 Enemy와 Player의 onDrop에서 구현중임.
    {
        if (arrow != null)
        {
            arrow.gameObject.SetActive(false);
            arrow.lineRenderer.enabled = false;
        }
        cardCanvas.sortingOrder = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("우클릭 감지됨 (OnPointerDown)");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("우클릭 감지됨 (OnPointerDown)");
            CardFocusManager.Instance.ShowFocusView(cardData);
        }
    }
    //getter/Setter------------
    public TargetingArrow TargetingArrowGetter()
    {
        return arrow;
    }
    public CardData CardDataGetter() { 
        return cardData;
    }
}