using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro; // UI 반영용
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance; // 접근 편의를 위한 싱글톤
    [Header("Player")]
    [SerializeField] private GameObject player;
    [Header("Energy Settings")]
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int currentEnergy;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image energyIcon;          // 캐릭터에 따른 아이콘 변경을 나중에 구현하기 위해
    [SerializeField] private GameObject winButton;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab; // 소환할 적 프리팹 영역. 이영역을 추가, 분할해서 챕터구분으로 사용도 가능
    [SerializeField] private RectTransform enemySpawnPoint; // 적이 배치될 위치(Canvas 내부)

    [SerializeField] private RewardUI rewardUI; // RewardCanvas와 연결된 스크립트

    [Header("Statuses")]
    [SerializeField] private StatusEffect vulnerable;
    [SerializeField] private StatusEffect power;
    private Enemy _currentEnemy; // 현재 소환된 적 참조 저장
    private List<Enemy> activeEnemies = new List<Enemy>();

    private BattleState currentState;//현재 턴의 상태를 저장

    private List<StatusController> _allControllers = new List<StatusController>();
    StatusController playerStatus;
    public void RegisterEntity(StatusController controller) => _allControllers.Add(controller);

    private bool isFirstTurn;

    private bool IsInfiniteGrowth = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }

    private void Start()
    {
        isFirstTurn = true;
        winButton.SetActive(false);
        PlayerController.Instance.Initialize();
        SpawnEnemy();
        foreach (var enemy in activeEnemies)
        {
            enemy.DecideNextAction();
        }
        ResetEnergy();
        RelicManager.Instance.NotifyTurnStart();
        currentState = BattleState.PlayerTurn;
        playerStatus = PlayerController.Instance.StatusControllerGetter();
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

    public void UpdateEnergyUI()
    {
        energyText.text = $"{currentEnergy} / {maxEnergy}";
        energyText.color = (currentEnergy <= 0) ? Color.red : Color.black;
    }
    //-----------------------------------------------------------------------------
    //적소환-----------------------------------------------------------------------
    public void SpawnEnemy()
    {
        if (enemyPrefab == null || enemySpawnPoint == null) return;

        GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint, false);

        // 2. 생성된 오브젝트에서 Enemy 컴포넌트 참조
        _currentEnemy = enemyObj.GetComponent<Enemy>();

        activeEnemies.Add(_currentEnemy);

        // 3. (선택) 적 데이터 초기화 등 필요한 로직 수행
        //Debug.Log($"{_currentEnemy.enemyData[Enemy.Instance.enemyWho].enemyName} 등장!");
    }
    public void CheckEnemyDeaths()
    {
        // 뒤에서부터 앞으로 루프를 돕니다.
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i].CurrentHpGetter() <= 0)
            {
                activeEnemies.RemoveAt(i);
                RelicManager.Instance.NotifyEnemyDeath();
            }
        }

        //모든 적이 죽었는지 체크
        if (activeEnemies.Count == 0)
        {
            currentState = BattleState.Win;
            WinBattle();
        }
    }

    public void WinBattle()
    {
        IsInfiniteGrowth = false;
        activeEnemies.Clear(); // 중복 실행 방지
        Debug.Log("전투 승리!");
        PlayerController.Instance.SetHpAfterBattle();
        DeckManager.Instance.DeckReset();
        // 보상 시스템 호출
        rewardUI.ShowRewardPanel();
        Button win = winButton.GetComponent<Button>();
        winButton.SetActive(true);
        // 버튼 클릭 리스너 등록
        win.onClick.AddListener(WinClick);
    }

    public void WinClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
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
        if(IsInfiniteGrowth == true)
        {
            playerStatus.AddStatus(power, player, 1, -1);
        }
        Debug.Log("적의 턴!");
        // 1. 플레이어 조작 비활성화 (UI 클릭 방지 등)
        currentState = BattleState.EnemyTurn;
        DeckManager.Instance.CanDrawSetter(true);
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
            yield return new WaitForSeconds(0.1f);
        }

        // 4. 패배가 아니면 다시 플레이어 턴으로
        if (currentState != BattleState.Lose)
        {
            StartPlayerTurn();

        }
    }
    void StartPlayerTurn()
    {
        isFirstTurn = false;
        for (int i = _allControllers.Count-1; i >= 0; i--)
        {
            _allControllers[i].TickTurn();
        }
        Debug.Log(_allControllers[0].activeStatuses.Count);
        Debug.Log(_allControllers[1].activeStatuses.Count);
        Debug.Log("아군의 턴!");
        currentState = BattleState.PlayerTurn;

        // 에너지 리프레시
        ResetEnergy();
        //플레이어 턴시작시
        PlayerController.Instance.OnTurnStart();
        // 카드 드로우
        DeckManager.Instance.DrawCard(5);

        // 다음 적 의도 결정
        foreach (var enemy in activeEnemies)
        {
            enemy.DecideNextAction();
        }
        //턴 시작시 유물 실행
        RelicManager.Instance.NotifyTurnStart();
    }
    //--------------------getter/setter
    public void currentEnergySetter(int count)
    {
        currentEnergy += count;
    }

    public void CurrentStateSetter(BattleState changed) {
        currentState = changed;
    }

    public bool IsFirstTurnGetter()
    {
        return isFirstTurn;
    }

    public BattleState CurrentStateGetter()
    {
        return currentState;
    }

    public List<Enemy> ActiveEnemiesGetter()
    {
        return activeEnemies;
    }
    //-------------------------------------
    public void InfiniteGrowthHelper()
    {
        IsInfiniteGrowth = true;
    }
}