using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class RelicTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject tooltip;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descText;
    private BaseRelic relic;
    private GameObject panel;
    private TextMeshProUGUI panelTitle;
    private SellingRelic sellingRelic;
    void Awake()
    {
        tooltip = GameObject.FindWithTag("Tooltip");
        Transform targetTransform = tooltip.transform.Find("RelicPanel");
        panel = targetTransform.gameObject;
        panel.SetActive(false);

        foreach (var t in panel.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (t.name == "TooltipTitle")
            {
                titleText = t;
            }
            else if (t.name =="TooltipExplanation")
            {
                descText = t; 
            }
        }

        GameObject target = GameObject.FindWithTag("SellingRelic");
        sellingRelic = target.GetComponent<SellingRelic>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        //panelTitle = panel.GetComponentInChildren<TextMeshProUGUI>();
        titleText.text = relic.relicName;
        descText.text = relic.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭 감지됨 (OnPointerDown)");
        int price = sellingRelic.PriceGetter();
        if (ShopManager.Instance.playerData.money >= price)
        {
            // 돈 차감 (잊지 마세요!)
            ShopManager.Instance.playerData.money -= price;
            ShopManager.Instance.moneyText.text = ShopManager.Instance.playerData.money.ToString();
            // 내 덱에 추가
            RelicManager.Instance.AddRelic(relic);

            // 중요: 판매 리스트(데이터)에서 제거
            // 인덱스가 아닌 '객체' 자체를 찾아서 제거해야 안전합니다.
            List<BaseRelic> tempSellList = sellingRelic.SellListGetter();
            if (tempSellList.Contains(relic))
            {
                sellingRelic.SellListRemover(relic);
            }
            panel.SetActive(false);
            // 중요: 화면에서 프리팹 파괴
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    public void RelicSetter(BaseRelic baseRelic)
    {
        relic = baseRelic;
    }
}
