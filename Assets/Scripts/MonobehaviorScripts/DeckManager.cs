using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckManager : MonoBehaviour
{
    [Header("카드 데이터 설정")]
    public List<CardData> allCards; // 게임에서 사용할 전체 카드 리스트 (DB 역할)

    [Header("UI 연결")]
    public GameObject cardPrefab;  // 방금 만든 카드 프리팹
    public Transform handParent;   // 카드가 배치될 부모 오브젝트 (Hand)

    // 실시간 카드 더미들
    private List<CardData> drawPile = new List<CardData>();
    private List<CardData> hand = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();

    public int firstDrawCard = 5;

    void Start()
    {
        //시작할 때 모든 카드를 뽑을 더미에 넣고 섞은 후 첫 번째 턴 드로우 매수만큼 뽑는다.
        SetupDeck();
        DrawCard(firstDrawCard);
    }

    private void Update()
    {
        //스페이스 바를 누르면 드로우 실행(테스트용)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DrawCard(1);
        }
    }

    void SetupDeck()
    {
        drawPile.AddRange(allCards);
        Shuffle(drawPile);
    }

    //덱의 셔플을 구현(피셔 예이츠 셔플 알고리즘)
    void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CardData temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        Debug.Log("덱을 섞었습니다.");
    }

    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 1. 뽑을 카드가 없으면 버린 카드 더미를 다시 가져옴
            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0) return; // 더 이상 카드가 없음

                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            // 2. 맨 위 카드 한 장 뽑기
            CardData card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);

            // --- 추가된 코드: 실제 UI 생성 ---
            GameObject newCard = Instantiate(cardPrefab, handParent);
            CardDisplay display = newCard.GetComponent<CardDisplay>();

            if (display != null)
            {
                display.Setup(card);
            }
            Debug.Log($"{card.cardName}을(를) 뽑았습니다.");

            // TODO: 여기서 실제로 화면(UI)에 카드를 생성하는 코드 호출 필요!
        }
    }
}