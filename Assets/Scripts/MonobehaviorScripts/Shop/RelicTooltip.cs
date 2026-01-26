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
        bool isSold = ShopManager.Instance.TryBuyRelic(relic);
        if (isSold)
        {
            panel.SetActive(false);
        }
    }

    public void RelicSetter(BaseRelic baseRelic)
    {
        relic = baseRelic;
    }
}
