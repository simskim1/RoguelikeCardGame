using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image frameImage;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image cardImage;

    [Header("프레임")]
    [SerializeField] private Sprite attackFrame;
    [SerializeField] private Sprite skillFrame;
    [SerializeField] private Sprite powerFrame;

    private CardData _cardData;
    private Button _button;
    // RewardUI에서 호출할 초기화 함수
    public void Setup(CardData data)
    {
        _cardData = data; // 이 슬롯이 들고 있을 카드 정보 저장
        if (data == null) return;

        cardNameText.text = data.cardName;
        descriptionText.text = data.description;
        costText.text = data.energyCost.ToString();
        switch (data.cardType)
        {
            case CardType.Attack: frameImage.sprite = attackFrame; break;
            case CardType.Skill: frameImage.sprite = skillFrame; break;
            case CardType.Power: frameImage.sprite = powerFrame; break;
        }
        // 버튼 컴포넌트를 가져와서 클릭 이벤트 초기화
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners(); // 이전 이벤트 제거 (중요!)
        _button.onClick.AddListener(OnClicked);
        UnityEngine.Debug.Log($"{data.cardName} 버튼 리스너 등록 완료");
        // 이미지가 설정되어 있을 때만 업데이트 (이미지 깨짐 방지)
        if (data.cardImage != null)
        {
            cardImage.sprite = data.cardImage;
        }

        Canvas canvas = GetComponent<Canvas>();

        canvas.overrideSorting = true;
        canvas.sortingOrder = 400;
    }

    private void OnClicked()
    {
        UnityEngine.Debug.Log("OnClicked 실행됨!");
        // 1. 덱 매니저에게 카드 추가 요청
        DeckManager.Instance.AddCardToMasterDeck(_cardData);

        // 2. 보상 UI 닫기 (부모인 RewardUI의 함수 호출)
        GetComponentInParent<RewardUI>().ClosePanel();

        UnityEngine.Debug.Log($"{_cardData.cardName} 선택됨!");
    }
}