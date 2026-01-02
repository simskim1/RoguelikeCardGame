using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                           IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI References")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image cardImage;
    public Image frameImage;

    [Header("Frame Sprites")]
    public Sprite attackFrame;
    public Sprite skillFrame;
    public Sprite powerFrame;

    [Header("Hover Settings")] // 3. 호버 설정 추가
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationDuration = 0.2f;

    public CardData cardData;//디스플레이에 카드의 정보를 저장
    private Vector3 originalScale;
    private int originalSiblingIndex;
    private Canvas cardCanvas;

    public TargetingArrow arrow;
    private Vector3 dragStartPosition;
    private Vector3 dragStartWorldPos;
    private Vector3 dragStartPos; // 카드 시작 위치 (로컬)

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
    }
    private void Start()
    {
        arrow = FindFirstObjectByType<TargetingArrow>();
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

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        // 카드의 부모(Canvas 등) 내에서의 로컬 좌표를 저장
        dragStartPos = transform.localPosition;

        if (arrow != null)
        {
            arrow.lineRenderer.enabled = true;
            arrow.gameObject.SetActive(true);
        }
    }*/

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1. 카드의 현재 '월드' 위치를 기준으로 캔버스 내 로컬 좌표를 새로 계산
        RectTransform canvasRect = arrow.GetComponentInParent<Canvas>().transform as RectTransform;
        Camera uiCamera = eventData.pressEventCamera;

        // 카드의 현재 위치(transform.position)를 캔버스 로컬 좌표로 정확히 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            // 카드가 렌더링되는 카메라를 통해 스크린 좌표를 먼저 구함
            RectTransformUtility.WorldToScreenPoint(uiCamera, transform.position),
            uiCamera,
            out Vector2 correctedStartPos
        );

        dragStartPos = correctedStartPos; // 보정된 시작점 저장

        if (arrow != null)
        {
            arrow.lineRenderer.enabled = true;
            arrow.gameObject.SetActive(true);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // 1. 카드 이동 (마우스 위치를 UI 로컬 좌표로 변환)
        RectTransform canvasRect = arrow.GetComponentInParent<Canvas>().transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localMousePos))
        {
            // 카드는 마우스 위치로 (필요 시)
            // transform.localPosition = localMousePos; 

            // 2. 화살표 그리기 (시작점: 저장해둔 위치, 끝점: 현재 마우스 위치)
            // 두 좌표 모두 동일한 canvasRect 기준이므로 오차가 발생할 수 없음
            arrow.DrawCurve(dragStartPos, localMousePos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)//포물선의 캐릭터 위에 떨어졌을 때의 삭제는 여기가 아닌 Enemy와 Player의 onDrop에서 구현중임.
    {
        if (arrow != null)
        {
            arrow.gameObject.SetActive(false);
            arrow.lineRenderer.enabled = false;
        }
    }
}