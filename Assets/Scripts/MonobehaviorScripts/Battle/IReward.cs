public interface IReward
{
    string Description { get; }
    void Claim(); // 보상 획득 시 실행
}

// 이 클래스는 SO가 아니라 로직을 담는 일반 클래스야
public class CardReward : IReward
{
    private CardData data;
    public string Description => data.cardName;

    public CardReward(CardData cardData)
    {
        data = cardData;
    }

    public void Claim()
    {
        // DeckManager(싱글톤)의 마스터 덱에 추가
        DeckManager.Instance.AddCardToMasterDeck(data);
    }
}