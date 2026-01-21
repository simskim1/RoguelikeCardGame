using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SellingCard : MonoBehaviour
{
    public static SellingCard Instance; // 접근 편의를 위한 싱글톤

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI References")]
    [SerializeField] private GameObject cardPrefab; // 카드 UI 프리팹
    [SerializeField] private Transform contentArea; // Scroll View의 Content
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject listRootPanel;

    public List<CardData> sellList = new List<CardData>();
    private bool openCheck;
    public int price;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
        openCheck = false;
    }
    public void Open(string title, List<CardData> cards)
    {
        listRootPanel.SetActive(true);
        titleText.text = title;

        // 1. 기존에 생성된 카드 UI 오브젝트 삭제 (Clean up)
        ClearList();

        // 2. 리스트를 순회하며 카드 생성
        foreach (CardData data in cards)
        {
            GameObject cardObj = Instantiate(cardPrefab, contentArea);
            cardObj.GetComponent<CardDeleteDisplay>().Setup(data);
            // 카드 UI의 Setup 함수를 호출하여 데이터 전달 (이미지, 텍스트 등)
        }
    }

    private void ClearList()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
    }
    public void SellCard()
    {
        if (openCheck == false)
        {
            openCheck = true;
            sellList.Clear();

            // 4개의 고유한 카드를 뽑을 때까지 반복
            while (sellList.Count < 4)
            {
                ChooseWhatToSell();
            }
        }
        Open("", sellList);
    }

    public void ChooseWhatToSell()
    {
        float chance = Random.value;
        CardData selectedVisual = null; // 뽑힌 카드를 임시 저장 (CardData는 실제 클래스명으로 변경하세요)

        // 1. 확률에 따라 카드 데이터베이스에서 하나를 선택
        if (chance <= 0.03f)
        {
            int index = Random.Range(0, CardDatabaseShop.Instance.special.Count);
            selectedVisual = CardDatabaseShop.Instance.special[index];
        }
        else if (chance <= 0.1f)
        {
            int index = Random.Range(0, CardDatabaseShop.Instance.rare.Count);
            selectedVisual = CardDatabaseShop.Instance.rare[index];
        }
        else if (chance <= 0.4f)
        {
            int index = Random.Range(0, CardDatabaseShop.Instance.uncommon.Count);
            selectedVisual = CardDatabaseShop.Instance.uncommon[index];
        }
        else
        {
            int index = Random.Range(0, CardDatabaseShop.Instance.common.Count);
            selectedVisual = CardDatabaseShop.Instance.common[index];
        }

        // 2. 중복 체크 후 리스트에 추가
        // Contains를 사용하거나, 이름으로 비교하려면 !sellList.Any(x => x.name == selectedVisual.name) 사용
        if (selectedVisual != null && !sellList.Contains(selectedVisual))
        {
            sellList.Add(selectedVisual);
        }
    }
}
