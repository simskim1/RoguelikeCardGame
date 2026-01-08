using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform slotParent; // Horizontal Layout Group이 있는 곳
    [SerializeField] private GameObject cardSlotPrefab;

    public void ShowRewardPanel()
    {
        panel.SetActive(true);
        GenerateCardRewards();
    }

    private void GenerateCardRewards()
    {
        // 이전에 생성된 슬롯 제거 (Clear)
        foreach (Transform child in slotParent) Destroy(child.gameObject);

        // 1. 랜덤 카드 3개 추출 (Fisher-Yates 알고리즘 적용된 메서드 호출)
        List<CardData> selectedCards = CardDatabase.Instance.GetRandomCards(3);

        // 2. 카드 슬롯 생성 및 데이터 바인딩
        foreach (var card in selectedCards)
        {
            GameObject slotGo = Instantiate(cardSlotPrefab, slotParent);
            CardReward reward = new CardReward(card);
            Canvas.ForceUpdateCanvases();
            /*
            // 슬롯의 버튼 클릭 시 보상 획득 로직 연결
            slotGo.GetComponent<Button>().onClick.AddListener(() => {
                reward.Claim();
                ClosePanel();
            });*/

            // UI 업데이트 (이름, 이미지 등)
            slotGo.GetComponent<CardSlotUI>().Setup(card);
            
            
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        // 다음 스테이지로 이동하는 로직 추가 가능
    }
}