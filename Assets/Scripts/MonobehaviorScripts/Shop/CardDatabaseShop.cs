using System.Collections.Generic;
using UnityEngine;

public class CardDatabaseShop : MonoBehaviour
{
    public static CardDatabaseShop Instance; // 접근 편의를 위한 싱글톤

    [SerializeField] private List<CardData> Common = new();
    public IReadOnlyList<CardData> common => Common;
    [SerializeField] private List<CardData> Uncommon = new();
    public IReadOnlyList<CardData> uncommon => Uncommon;
    [SerializeField] private List<CardData> Rare = new();
    public IReadOnlyList<CardData> rare => Rare;
    [SerializeField] private List<CardData> Special = new();
    public IReadOnlyList<CardData> special => Special;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
}
