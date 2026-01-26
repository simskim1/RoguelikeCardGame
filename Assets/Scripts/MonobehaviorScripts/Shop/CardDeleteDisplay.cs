using DG.Tweening;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardDeleteDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image RarityImage;

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
    private bool isProcessed = false;
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

        }
    }

    public void Setup(CardData data)
    {
        cardData = data;

        cardNameText.text = data.cardName;
        descriptionText.text = data.description;
        costText.text = data.energyCost.ToString();
        cardImage.sprite = data.cardImage;
        switch (data.rarity)
        {
            case CardRarity.Common: RarityImage.color = Color.white; break;
            case CardRarity.Uncommon: RarityImage.color = Color.green; break;
            case CardRarity.Rare: RarityImage.color = Color.blue; break;
            case CardRarity.Special: RarityImage.color = Color.red; break;
        }
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
        // 2. 크기 키우기
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, animationDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 2. 원래 크기로 복구
        transform.DOKill();
        transform.DOScale(originalScale, animationDuration).SetEase(Ease.InQuad);
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isProcessed) return;
        Debug.Log("클릭 감지됨 (OnPointerDown)");
        if (ShopManager.Instance.isDeleting == true)
        {
            for (int i = 0; i < ShopManager.Instance.deckData.masterDeck.Count; i++)
            {
                if (ShopManager.Instance.deckData.masterDeck[i].cardName == cardData.cardName)
                {
                    ShopManager.Instance.deckData.masterDeck.RemoveAt(i);
                    ShopManager.Instance.DeletePanel.SetActive(false);
                    ShopManager.Instance.isDeleting = false;
                    break;
                }

            }
        }
        else
        {
            // 구매 모드 로직
            int price = GetPrice(); // 가격 계산 로직 분리 (아래 참고)

            if (ShopManager.Instance.playerData.money >= price)
            {
                // 돈 차감 (잊지 마세요!)
                ShopManager.Instance.playerData.money -= price;
                ShopManager.Instance.moneyText.text = ShopManager.Instance.playerData.money.ToString();
                // 내 덱에 추가
                ShopManager.Instance.deckData.masterDeck.Add(cardData);

                // 중요: 판매 리스트(데이터)에서 제거
                // 인덱스가 아닌 '객체' 자체를 찾아서 제거해야 안전합니다.
                if (SellingCard.Instance.sellList.Contains(this.cardData))
                {
                    SellingCard.Instance.sellList.Remove(this.cardData);
                }
                isProcessed = true;
                // 중요: 화면에서 프리팹 파괴
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("돈이 부족합니다.");
            }
        }
    }
    */
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isProcessed) return;

        // 복잡한 로직은 매니저에게 맡깁니다.
        bool success = ShopManager.Instance.TryProcessCardAction(this.cardData);

        if (success)
        {
            isProcessed = true;
            Destroy(gameObject);
        }
    }
}