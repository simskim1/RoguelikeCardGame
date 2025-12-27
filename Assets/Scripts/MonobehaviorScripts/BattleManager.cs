using UnityEngine;
using TMPro; // UI 반영용
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance; // 접근 편의를 위한 싱글톤

    [Header("Energy Settings")]
    public int maxEnergy = 3;
    public int currentEnergy;

    [Header("UI References")]
    public TextMeshProUGUI energyText;
    public Image energyIcon;          // 캐릭터에 따른 아이콘 변경을 나중에 구현하기 위해

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab; // 소환할 적 프리팹 영역, 이 영역을 추가, 분할함으로서 챕터에 따른 랜덤 적이 나오게 설정 가능
    [SerializeField] private RectTransform enemySpawnPoint; // 적이 배치될 위치(Canvas 내부)

    private Enemy _currentEnemy; // 현재 소환된 적 참조 저장

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }

    private void Start()
    {
        SpawnEnemy();
        ResetEnergy();
        Debug.Log($"{currentEnergy}가 현재 에너지");
    }

    // 에너지를 최대치로 초기화 (턴 시작 시 호출 예정)
    public void ResetEnergy()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
        Debug.Log($"{currentEnergy}가 현재 에너지");

    }

    // 에너지 사용 가능 여부 확인
    public bool CanUseCard(int cost)
    {
        return currentEnergy >= cost;
    }

    // 실제 에너지 차감
    public void UseEnergy(int amount)
    {
        if (CanUseCard(amount))
        {
            currentEnergy -= amount;
            UpdateEnergyUI();
            Debug.Log($"{currentEnergy}가 현재 에너지");
        }
    }

    private void UpdateEnergyUI()
    {
        energyText.text = $"{currentEnergy} / {maxEnergy}";
        energyText.color = (currentEnergy <= 0) ? Color.red : Color.black;
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || enemySpawnPoint == null) return;

        // 1. 프리팹 생성 (부모를 spawnPoint로 설정하여 UI 계층 유지)
        GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint);

        // 2. 생성된 오브젝트에서 Enemy 컴포넌트 참조
        _currentEnemy = enemyObj.GetComponent<Enemy>();

        // 3. (선택) 적 데이터 초기화 등 필요한 로직 수행
        Debug.Log($"{_currentEnemy.enemyData[Enemy.Instance.enemyWho].enemyName} 등장!");
    }
}