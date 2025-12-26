using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image cardImage;

    public CardData cardData;//단순 저장용

    // 이 함수가 호출되면 SO의 데이터로 UI를 채움
    public void Setup(CardData data)
    {
        cardData = data;

        cardNameText.text = data.cardName;
        descriptionText.text = data.description;
        costText.text = data.energyCost.ToString();
        cardImage.sprite = data.cardImage;
    }
}