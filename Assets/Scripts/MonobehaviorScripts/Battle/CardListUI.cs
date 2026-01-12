using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardListUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cardPrefab; // 카드 UI 프리팹
    [SerializeField] private Transform contentArea; // Scroll View의 Content
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject listRootPanel;

    public static CardListUI Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 생성 방지
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
            // 카드 UI의 Setup 함수를 호출하여 데이터 전달 (이미지, 텍스트 등)
            cardObj.GetComponent<CardDisplay>().Setup(data);
        }
    }

    private void Update()
    {
        if (listRootPanel.activeInHierarchy == true)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Close();
            }
        }
    }
    public void Close()
    {
        ClearList();
        listRootPanel.SetActive(false);
    }

    private void ClearList()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
    }
}