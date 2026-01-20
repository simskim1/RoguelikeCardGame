using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance; // 접근 편의를 위한 싱글톤

    [Header("카드 데이터 설정")]
    public DeckData deck;
    public List<CardData> masterDeck = new List<CardData>(); 

    [Header("UI 연결")]
    public GameObject cardPrefab;  
    public Transform handParent;   // 카드가 배치될 부모 오브젝트

    // 실시간 카드 더미들
    public List<CardData> drawPile = new List<CardData>();
    private List<CardData> hand = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();

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
        masterDeck = deck.masterDeck;
        drawPile.AddRange(masterDeck);
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
            int hcount = hand.Count;
            for (int j = 0; j < hcount; j++)
            {
                Debug.Log($"{hand[j].cardName}, ");
            }

            // TODO: 여기서 실제로 화면(UI)에 카드를 생성하는 코드 호출 필요!
        }
    }

    //끌어서 사용한 카드를 DiscardPlle에 보내기(destroy는 이 함수를 사용하는 쪽이 받은 오브젝트에 destroy넣어주기)
    public void DiscardCardDragged(CardData card, PointerEventData eventData)
    {
        /*int count = hand.Count;
        int index = hand.IndexOf(card);
        discardPile.Add(card);
        hand.RemoveAt(index);
        //묘지 존재하는 카드 전부 올림
        for (int i = 0; i < discardPile.Count; i++) {
            Debug.Log($"{discardPile[i].cardName}가 묘지로 갔습니다");
        }*/
        // 1. 객체 자체를 리스트에서 지우기 (더 안전함)
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);

            // 묘지 존재하는 카드 전부 출력 (디버깅용)
            foreach (var discard in discardPile)
            {
                Debug.Log($"{discard.cardName}가 묘지로 갔습니다");
            }
        }
        else
        {
            Debug.LogWarning($"지우려는 카드 {card.cardName}가 패(hand)에 없습니다!");
        }
    }

    //내 패 전부를 DiscardPile에 보내기
    public void DiscardHand()
    {
        int count = hand.Count;
        for (int i = 0; i < count; i++)
        {
            discardPile.Add(hand[0]);
            hand.RemoveAt(0);
        }
        foreach (Transform child in handParent)
        {
            Destroy(child.gameObject);
        }
        
    }

    //끝날때 리셋
    public void DeckReset()
    {
        drawPile.AddRange(hand);
        drawPile.AddRange(discardPile);
        hand.Clear();
        discardPile.Clear();
    }
    //덱에 카드를 추가
    public void AddCardToMasterDeck(CardData data)
    {
        masterDeck.Add(data);
        Debug.Log($"{data.cardName}이(가) 영구적으로 덱에 추가되었습니다! 현재 덱 수: {masterDeck.Count}");
        deck.masterDeck = masterDeck;
    }
}