using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
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

    private CardData currentData;

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
}