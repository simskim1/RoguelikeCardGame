using System.Collections.Generic;
using UnityEngine;

public class CardDatabaseShop : MonoBehaviour
{
    public static CardDatabaseShop Instance; // 접근 편의를 위한 싱글톤

    public List<CardData> common = new List<CardData>();
    public List<CardData> uncommon = new List<CardData>();
    public List<CardData> rare = new List<CardData>();
    public List<CardData> special = new List<CardData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
}
