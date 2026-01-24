using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 특정 시점에 모든 유물에게 알림을 보냄
    public void NotifyTurnStart()
    {
        foreach (BaseRelic relic in playerData.currentRelic)
        {
            relic.OnTurnStart();
        }
    }

    public void AddRelic(BaseRelic newRelic)
    {
        // 유물 획득 시 로직 (UI 업데이트 등)
        playerData.currentRelic.Add(newRelic);
        // 필요하다면 인스턴스화하여 개별 데이터 유지
    }
}