using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
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

    public CardData cardData;//디스플레이에 카드의 정보를 저장

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
}