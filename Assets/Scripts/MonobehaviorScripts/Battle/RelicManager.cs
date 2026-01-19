using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    public List<BaseRelic> ownedRelics = new List<BaseRelic>();

    private void Awake() => Instance = this;

    // 특정 시점에 모든 유물에게 알림을 보냄
    public void NotifyTurnStart()
    {
        foreach (var relic in ownedRelics)
        {
            relic.OnTurnStart();
        }
    }

    public void AddRelic(BaseRelic newRelic)
    {
        // 유물 획득 시 로직 (UI 업데이트 등)
        ownedRelics.Add(newRelic);
        // 필요하다면 인스턴스화하여 개별 데이터 유지
    }
}