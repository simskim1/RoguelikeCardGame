using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.GPUSort;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance; // 접근 편의를 위한 싱글톤

    [Header("플레이어")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private DeckData deckData;

    [Header("초기 화면")]
    [SerializeField] private UnityEngine.UI.Button healButton;
    [SerializeField] private UnityEngine.UI.Button shopButton;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("선택 확인 패널")]
    [SerializeField] private GameObject checkPanel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private UnityEngine.UI.Button buttonAccept;
    [SerializeField] private UnityEngine.UI.Button buttonRefuse;

    [Header("상점 UI관련")]
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject DeletePanel;
    [SerializeField] private UnityEngine.UI.Button cardDelete;
    [SerializeField] private TextMeshProUGUI cardDeleteCost;
    [SerializeField] private SellingCard sellingCard;
    [SerializeField] private SellingRelic sellingRelic;
    [SerializeField] private Transform sellingCardTransform;

    private int cardDeleteCount;
    private bool isDeleting;
    private CardData selectedCardData;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
    void Start()
    {
        DeletePanel.SetActive(false);
        ShopUI.SetActive(false);
        checkPanel.SetActive(false);
        healButton.onClick.AddListener(HealCheck);
        shopButton.onClick.AddListener(ShopCheck);
        buttonAccept.onClick.AddListener(Rest);
        buttonRefuse.onClick.AddListener(Return);
        cardDelete.onClick.AddListener(DeleteCard);
        moneyText.text = playerData.money.ToString();
        isDeleting = false;
        cardDeleteCount = 0;
    }

    private void Return()
    {
        checkPanel.SetActive(false);
    }
    //휴식 버튼에 대한 코드들--------------------------------------
    void HealCheck()
    {
        checkPanel.SetActive(true);
        panelText.text = "회복하시겠습니까?\n최대채력의 15%만큼을 회복하고,\n상점을 떠나게 됩니다.";
    }

    void Rest()
    {
        checkPanel.SetActive(false);
        playerData.currentHP += Mathf.RoundToInt((float)(playerData.maxHP * 0.15));
        if (playerData.currentHP > playerData.maxHP)
        {
            playerData.currentHP = playerData.maxHP;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }
    //상점에 대한 코드들---------------------------------------
    void ShopCheck()
    {
        ShopUI.SetActive(true);
        sellingCard.SellCard();
        sellingRelic.SellRelic();
        cardDeleteCost.text = $"카드\n삭제하기\n{80 * (cardDeleteCount+1)}원";
    }

    void DeleteCard()
    {
        if(playerData.money < 80 * (cardDeleteCount + 1))
        {
            return;
        }
        DeletePanel.SetActive(true);
        isDeleting = true;
        playerData.money -= (80 * (cardDeleteCount + 1));
        moneyText.text = playerData.money.ToString();
        cardDeleteCount++;
        cardDeleteCost.text = $"카드\n삭제하기\n{80 * (cardDeleteCount + 1)}원";
        CardListUI.Instance.Open("삭제할 카드를 고르시오", deckData.masterDeck);
    }

    public bool TryProcessCardAction(CardData card)
    {
        selectedCardData = card;
        if (isDeleting)
        {
            return TryDeleteCard(card);
        }
        else
        {
            return TryBuyCard(card);
        }
    }

    private bool TryDeleteCard(CardData card)
    {
        for (int i = 0; i < deckData.masterDeck.Count; i++)
        {
            if (deckData.masterDeck[i].cardName == card.cardName)
            {
                deckData.masterDeck.RemoveAt(i);
                DeletePanel.SetActive(false);
                isDeleting = false;
                break;
            }
        }
        return true;
    }
    private bool TryBuyCard(CardData card)
    {
        int price = GetPrice(card);
        if (playerData.money < price) return false;

        playerData.money -= price;
        deckData.masterDeck.Add(card);
        List<CardData> sellList = SellingCard.Instance.SellListgetter();
        if (sellList.Contains(card))
        {
            sellList.Remove(card);
        }
        moneyText.text = playerData.money.ToString();
        return true;
    }

    public int GetPrice(CardData card)
    {
        return card.rarity switch
        {
            CardRarity.Common => 50,
            CardRarity.Uncommon => 70,
            CardRarity.Rare => 100,
            CardRarity.Special => 130,
            _ => 9999
        };
    }
    //-------------------------------------
    public bool TryBuyRelic(BaseRelic relic)
    {
        Debug.Log("클릭 감지됨 (OnPointerDown)");
        int price = sellingRelic.PriceGetter();
        if (playerData.money >= price)
        {
            // 돈 차감 (잊지 마세요!)
            playerData.money -= price;
            moneyText.text = playerData.money.ToString();
            // 내 덱에 추가
            RelicManager.Instance.AddRelic(relic);

            // 중요: 판매 리스트(데이터)에서 제거
            // 인덱스가 아닌 '객체' 자체를 찾아서 제거해야 안전합니다.
            List<BaseRelic> tempSellList = sellingRelic.SellListGetter();
            if (tempSellList.Contains(relic))
            {
                sellingRelic.SellListRemover(relic);
            }
            
            // 중요: 화면에서 프리팹 파괴
            Destroy(this.gameObject);
            return true;
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }
    }
}
