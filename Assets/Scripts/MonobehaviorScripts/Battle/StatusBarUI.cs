using System.Collections.Generic;
using UnityEngine;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab; // 1-1에서 만든 프리팹
    [SerializeField] private StatusController targetController;

    private List<StatusIconUI> spawnedIcons = new List<StatusIconUI>();

    private void Start()
    {
        if (targetController != null)
        {
            // 이벤트 구독: 상태가 변할 때마다 RefreshUI 실행
            targetController.OnStatusChanged += RefreshUI;
            RefreshUI(); // 초기화
        }
    }

    private void OnDestroy()
    {
        if (targetController != null)
            targetController.OnStatusChanged -= RefreshUI;
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
        foreach (var status in targetController.activeStatuses)
        {
            GameObject go = Instantiate(iconPrefab, transform);
            StatusIconUI iconScript = go.GetComponent<StatusIconUI>();
            iconScript.UpdateUI(status);
            spawnedIcons.Add(iconScript);
        }
    }
}