using System.Collections.Generic;
using UnityEngine;

public class RelicBarUI : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab; // 1-1에서 만든 프리팹
    [SerializeField] private RelicManager targetController;
    [SerializeField] private PlayerData playerData;

    private List<RelicIconUI> spawnedIcons = new List<RelicIconUI>();

    private void Start()
    {
        if (targetController != null)
        {
            RefreshUI(); // 초기화
        }
    }

    private void OnDestroy()
    {
        
    }

    // 상태 이상 목록이 바뀔 때마다 호출될 함수
    public void RefreshUI()
    {
        // 1. 기존 아이콘 모두 제거 (Object Pooling을 쓰면 더 좋지만, 우선 기본형으로 구현)
        foreach (var icon in spawnedIcons)
        {
            Destroy(icon.gameObject);
        }
        spawnedIcons.Clear();

        // 2. 현재 상태 이상 개수만큼 아이콘 생성
        foreach (var status in playerData.currentRelic)
        {
            GameObject go = Instantiate(iconPrefab, transform);
            RelicIconUI iconScript = go.GetComponent<RelicIconUI>();
            iconScript.UpdateUI(status);
            spawnedIcons.Add(iconScript);
        }
    }
}
