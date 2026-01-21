using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance; // 접근 편의를 위한 싱글톤

    [Header("플레이어")]
    public PlayerData playerData;
    public DeckData deckData;

    [Header("초기 화면")]
    public Button healButton;
    public Button shopButton;
    public TextMeshProUGUI moneyText;

    [Header("선택 확인 패널")]
    public GameObject checkPanel;
    public TextMeshProUGUI panelText;
    public Button buttonAccept;
    public Button buttonRefuse;

    [Header("상점 UI관련")]
    public GameObject ShopUI;
    public GameObject DeletePanel;
    public Button cardDelete;
    public TextMeshProUGUI cardDeleteCost;
    public SellingCard sellingCard;
    public Transform sellingCardTransform;

    private int cardDeleteCount;
    public bool isDeleting;
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
        cardDeleteCost.text = $"카드\n삭제하기\n{80 * (cardDeleteCount+1)}원";
    }

    void DeleteCard()
    {
        if(playerData.money < 80 * (cardDeleteCount + 1))
        {
            return;
        }
        isDeleting = true;
        playerData.money -= (80 * (cardDeleteCount + 1));
        moneyText.text = playerData.money.ToString();
        cardDeleteCount++;
        cardDeleteCost.text = $"카드\n삭제하기\n{80 * (cardDeleteCount + 1)}원";
        CardListUI.Instance.Open("삭제할 카드를 고르시오", deckData.masterDeck);
    }
}
