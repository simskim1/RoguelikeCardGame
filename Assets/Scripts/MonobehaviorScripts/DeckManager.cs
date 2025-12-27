using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance; // 접근 편의를 위한 싱글톤

    [Header("카드 데이터 설정")]
    public List<CardData> allCards; 

    [Header("UI 연결")]
    public GameObject cardPrefab;  
    public Transform handParent;   // 카드가 배치될 부모 오브젝트

    // 실시간 카드 더미들
    private List<CardData> drawPile = new List<CardData>();
    private List<CardData> hand = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();

    public int firstDrawCard = 5;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
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
            // 뽑을 카드가 없으면 버린 카드 더미를 다시 가져옴
            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0) return; // 더 이상 카드가 없음

                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            // 맨 위 카드 한 장 뽑기
            CardData card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);//핸드 리스트에 추가

            
            GameObject newCard = Instantiate(cardPrefab, handParent);//카드 프리팹으로 오브젝트 생성
            CardDisplay display = newCard.GetComponent<CardDisplay>();// 그 오브젝트의 CardDisplay의 내용들을 가져옴

            if (display != null)
            {
                display.Setup(card);//뽑은 카드에 대한 Setup을 실행
            }
            Debug.Log($"{card.cardName}을(를) 뽑았습니다.");

            // TODO: 여기서 실제로 화면(UI)에 카드를 생성하는 코드 호출 필요!
        }
    }

    //카드를 DiscardPlle에 보낸 후 자신을 destroy
    public void DiscardCard(CardData card)
    {
        discardPile.Add(card);
        for (int i = 0; i < discardPile.Count; i++) {
            Debug.Log($"{discardPile[i].cardName}가 묘지로 갔습니다");
        }
    }
}