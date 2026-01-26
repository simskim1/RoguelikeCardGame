using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SellingRelic : MonoBehaviour
{
    public static SellingRelic Instance; // 접근 편의를 위한 싱글톤

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI References")]
    [SerializeField] private GameObject relicPrefab; // 카드 UI 프리팹

    [SerializeField] private Transform contentArea; // Scroll View의 Content
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject listRootPanel;

    private List<BaseRelic> sellList = new List<BaseRelic>();
    private bool openCheck;
    private int price;

    private BaseRelic selectedRelic;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
        openCheck = false;
    }
    public void Open(string title, List<BaseRelic> relics)
    {
        listRootPanel.SetActive(true);
        titleText.text = title;

        // 1. 기존에 생성된 카드 UI 오브젝트 삭제 (Clean up)
        ClearList();

        // 2. 리스트를 순회하며 카드 생성
        foreach (BaseRelic data in relics)
        {
            GameObject relicObj = Instantiate(relicPrefab, contentArea);
            RelicTooltip tooltip = relicObj.GetComponent<RelicTooltip>();
            tooltip.RelicSetter(data);
            Image relicImage = relicObj.GetComponentInChildren<RelicImageTag>().GetComponent<Image>();
            relicImage.sprite = data.icon;
            TextMeshProUGUI relicNameText = relicObj.GetComponentInChildren<TextMeshProUGUI>();
            relicNameText.text = data.name;
            Image relicRarity = relicObj.GetComponentInChildren<RelicRarityTag>().GetComponent<Image>();
            switch (data.rarity)
            {
                case RelicRarity.Common: relicRarity.color = Color.white; break;
                case RelicRarity.Rare: relicRarity.color = Color.blue; break;
            }
        }
    }

    private void ClearList()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
    }
    public void SellRelic()
    {
        if (openCheck == false)
        {
            openCheck = true;
            sellList.Clear();

            // 3개의 고유한 렐릭을 뽑을 때까지 반복
            while (sellList.Count < 3)
            {
                ChooseWhatToSell();
            }
        }
        Open("", sellList);
    }

    public void ChooseWhatToSell()
    {
        float chance = Random.value;
        BaseRelic selectedVisual = null; // 뽑힌 카드를 임시 저장 (CardData는 실제 클래스명으로 변경하세요)

        // 1. 확률에 따라 카드 데이터베이스에서 하나를 선택, 렐릭 데이터베이스에서 고르기!
        
        if (chance <= 0.1f)
        {
            int index = Random.Range(0, RelicDatabaseShop.Instance.rare.Count);
            price = 50;
            selectedVisual = RelicDatabaseShop.Instance.rare[index];
        }
        else
        {
            int index = Random.Range(0, CardDatabaseShop.Instance.common.Count);
            price = 100;
            selectedVisual = RelicDatabaseShop.Instance.common[index];
        }
        
        // 2. 중복 체크 후 리스트에 추가
        // Contains를 사용하거나, 이름으로 비교하려면 !sellList.Any(x => x.name == selectedVisual.name) 사용
        if (selectedVisual != null && !sellList.Contains(selectedVisual))
        {
            sellList.Add(selectedVisual);
        }
    }

    public int PriceGetter()
    {
        return price;
    }

    public List<BaseRelic> SellListGetter()
    {
        return sellList;
    }

    public void SellListRemover(BaseRelic relic)
    {
        sellList.Remove(relic);
    }
}