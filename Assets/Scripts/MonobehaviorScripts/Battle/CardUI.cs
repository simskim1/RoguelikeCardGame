using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // 1. 인터페이스 사용을 위해 추가
using DG.Tweening; // 2. DOTween 사용을 위해 추가

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Image illustrationImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Resources")]
    [SerializeField] private Sprite frameAttack;
    [SerializeField] private Sprite frameSkill;
    [SerializeField] private Sprite framePower;

    [Header("Hover Settings")] // 3. 호버 설정 추가
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationDuration = 0.2f;

    private CardData currentData;
    private Vector3 originalScale;
    private int originalSiblingIndex;

    private void Awake()
    {
        // 처음 시작 시 원래 크기를 저장해둡니다.
        originalScale = transform.localScale;
    }

    // 외부(CardManager 등)에서 이 함수를 호출해 카드를 생성합니다.
    public void Setup(CardData data)
    {
        currentData = data;

        // 1. 텍스트 데이터 바인딩
        titleText.text = data.cardName;
        costText.text = data.energyCost.ToString();
        descriptionText.text = data.description;

        // 2. 이미지 데이터 바인딩
        illustrationImage.sprite = data.cardImage;

        // 3. 타입에 따른 외형 변경 (Visual 고도화)
        UpdateFrame(data.cardType);
    }

    private void UpdateFrame(CardType type)
    {
        switch (type)
        {
            case CardType.Attack:
                frameImage.sprite = frameAttack;
                //titleText.color = Color.white; // 폰트 색상 등 세부 조절 가능
                break;
            case CardType.Skill:
                frameImage.sprite = frameSkill;
                break;
            case CardType.Power:
                frameImage.sprite = framePower;
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1. 레이아웃 순서를 맨 위로 (카드가 겹쳐있을 때 위로 올라옴)
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        // 2. 크기 키우기 (Ease.OutBack을 쓰면 살짝 커졌다가 돌아오는 찰진 느낌이 납니다)
        transform.DOKill(); // 이전 애니메이션이 돌고 있다면 중단
        transform.DOScale(originalScale * hoverScale, animationDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 1. 원래 순서로 복구
        transform.SetSiblingIndex(originalSiblingIndex);

        // 2. 원래 크기로 복구
        transform.DOKill();
        transform.DOScale(originalScale, animationDuration).SetEase(Ease.InQuad);
    }
}