using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance;

    // 프로젝트의 모든 카드 SO 리스트 (인스펙터에서 할당)
    [SerializeField] private List<CardData> allCards = new List<CardData>();

    void Awake()
    {
        Instance = this;
    }

    public List<CardData> GetRandomCards(int count)
    {
        // 1. 원본 리스트를 복사 (원본 데이터 보호)
        List<CardData> shuffleList = new List<CardData>(allCards);
        List<CardData> result = new List<CardData>();

        // 2. Fisher-Yates Shuffle 알고리즘
        // 뒤에서부터 앞으로 오면서 무작위 요소와 스왑
        for (int i = shuffleList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            CardData temp = shuffleList[i];
            shuffleList[i] = shuffleList[randomIndex];
            shuffleList[randomIndex] = temp;
        }

        // 3. 섞인 리스트에서 앞에서부터 count만큼 추출
        for (int i = 0; i < count && i < shuffleList.Count; i++)
        {
            result.Add(shuffleList[i]);
        }

        return result;
    }
}