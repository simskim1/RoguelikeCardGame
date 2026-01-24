using System.Collections.Generic;
using UnityEngine;

public class RelicDatabaseShop : MonoBehaviour
{
    public static RelicDatabaseShop Instance; // 접근 편의를 위한 싱글톤

    [SerializeField] private List<BaseRelic> Common = new();
    public IReadOnlyList<BaseRelic> common => Common;

    [SerializeField] private List<BaseRelic> Rare = new();
    public IReadOnlyList<BaseRelic> rare => Rare;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
}
