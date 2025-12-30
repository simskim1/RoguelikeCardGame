using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro; // UI 반영용
using UnityEngine;
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
    [SerializeField] private GameObject enemyPrefab; // 소환할 적 프리팹 영역. 이영역을 추가, 분할해서 챕터구분으로 사용도 가능
    [SerializeField] private RectTransform enemySpawnPoint; // 적이 배치될 위치(Canvas 내부)

    private Enemy _currentEnemy; // 현재 소환된 적 참조 저장
    public List<Enemy> activeEnemies = new List<Enemy>();

    public BattleState currentState;//현재 턴의 상태를 저장

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }

    private void Start()
    {
        PlayerController.Instance.Initialize();
        SpawnEnemy();
        Enemy.Instance.DecideNextAction();
        ResetEnergy();
        currentState = BattleState.PlayerTurn;
    }
    //에너지관리-----------------------------------------------------------------------
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
    //-----------------------------------------------------------------------------
    //적소환-----------------------------------------------------------------------
    public void SpawnEnemy()
    {
        if (enemyPrefab == null || enemySpawnPoint == null) return;

        // 1. 프리팹 생성 (부모를 spawnPoint로 설정하여 UI 계층 유지)
        GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint);

        // 2. 생성된 오브젝트에서 Enemy 컴포넌트 참조
        _currentEnemy = enemyObj.GetComponent<Enemy>();

        activeEnemies.Add(_currentEnemy);

        // 3. (선택) 적 데이터 초기화 등 필요한 로직 수행
        Debug.Log($"{_currentEnemy.enemyData[Enemy.Instance.enemyWho].enemyName} 등장!");
    }
    public void CheckEnemyDeaths()
    {
        // 뒤에서부터 앞으로 루프를 돕니다.
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i]._currentHp <= 0)
            {
                activeEnemies.RemoveAt(i);
            }
        }

        //모든 적이 죽었는지 체크
        if (activeEnemies.Count == 0)
        {
            Debug.Log("전투 승리!");
            currentState = BattleState.Win;
        }
    }
    //---------------------------------------------------------------------------
    //턴 관리---------------------------------------------------------------------
    // 턴 종료 버튼에 연결할 함수

    public void OnEndTurnButton()
    {
        if (currentState == BattleState.PlayerTurn)
        {
            StartCoroutine(EndPlayerTurn());
        }
    }

    IEnumerator EndPlayerTurn()
    {
        Debug.Log("적의 턴!");
        // 1. 플레이어 조작 비활성화 (UI 클릭 방지 등)
        currentState = BattleState.EnemyTurn;

        // 2. 남은 카드 버리기 (DeckManager 연동)
        DeckManager.Instance.DiscardHand();

        yield return new WaitForSeconds(0.5f); // 연출을 위한 짧은 대기

        // 3. 적의 행동 실행
        yield return StartCoroutine(EnemyTurnProcess());
    }
    IEnumerator EnemyTurnProcess()
    {
        // 모든 적의 행동을 순차적으로 실행
        foreach (var enemy in activeEnemies)
        {
            yield return StartCoroutine(enemy.TakeAction());
        }

        // 4. 다시 플레이어 턴으로
        StartPlayerTurn();
    }
    void StartPlayerTurn()
    {
        Debug.Log("아군의 턴!");
        currentState = BattleState.PlayerTurn;

        // 에너지 리프레시
        ResetEnergy();

        // 카드 드로우
        DeckManager.Instance.DrawCard(5);

        // 다음 적 의도 결정
        Enemy.Instance.DecideNextAction();
    }
}