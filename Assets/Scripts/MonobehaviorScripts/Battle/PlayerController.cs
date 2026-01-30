using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance; // 접근 편의를 위한 싱글톤

    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerUI playerUI;

    [Header("Runtime State")]
    [SerializeField] private int currentHP;
    [SerializeField] private int currentBlock;

    // UI 업데이트를 위한 이벤트를 쓰면 좋습니다 (옵저버 패턴의 기초)
    public event System.Action<int, int> OnHealthChanged; // <Current, Max>
    public event System.Action<int> OnBlockChanged;

    [SerializeField] private UnityEngine.UI.Slider hpSlider;
    private UnityEngine.UI.Image _playerImage;

    private StatusController status;

    [SerializeField] private MapData mapData;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
        OnHealthChanged += playerUI.UpdateHealthUI;
        OnBlockChanged += playerUI.UpdateBlockUI;
        status = GetComponent<StatusController>();
        BattleManager.Instance.RegisterEntity(status);
    }

    public void Initialize()
    {
        _playerImage = GetComponent<UnityEngine.UI.Image>();
        if (playerData != null)
        {
            // SO에 저장된 이미지로 변경
            if (_playerImage != null && playerData.characterSprite)
            {
                _playerImage.sprite = playerData.characterSprite;//이 오브젝트의 컴포넌트인 ememyimage의 스프라이트를 enemyData에 저장된 이미지로 변경
            }
        }
        if (playerData.currentHP == 0 || (mapData.currentFloor == 0 && playerData.currentStage == 1))
        {
            currentHP = playerData.maxHP;
        }
        else
        {
            currentHP = playerData.currentHP;
        }
            currentBlock = 0;
        UpdateUI();
    }

    public void OnTurnStart()
    {
        currentBlock = 0;
        OnBlockChanged?.Invoke(currentBlock);
    }

    private void UpdateUI()
    {
        OnHealthChanged?.Invoke(currentHP, playerData.maxHP);
        OnBlockChanged?.Invoke(currentBlock);
    }

    public void TakeDamage(int damageAmount)
    {
        int damageToTake = damageAmount;

        // 1. 방어력이 있다면 방어력부터 깎음
        if (currentBlock > 0)
        {
            if (currentBlock >= damageToTake)
            {
                currentBlock -= damageToTake;
                damageToTake = 0;
            }
            else
            {
                damageToTake -= currentBlock;
                currentBlock = 0;
            }
            OnBlockChanged?.Invoke(currentBlock); // UI 갱신
        }

        // 2. 남은 데미지가 있다면 체력을 깎음
        if (damageToTake > 0)
        {
            currentHP -= damageToTake;
            if (currentHP <= 0)
            {
                currentHP = 0;
                if (currentHP <= 0)
                {
                    Die();
                }
            }
            OnHealthChanged?.Invoke(currentHP, playerData.maxHP); // UI 갱신
        }

        // 피격 애니메이션 등 재생
    }

    private void Die()
    {
        DeckManager.Instance.DeckReset();
        mapData.nodes = null;
        BattleManager.Instance.CurrentStateSetter(BattleState.Lose);
        Destroy(gameObject);
    }
    
    public void SetHpAfterBattle()
    {
        playerData.currentHP = currentHP;
    }

    public void BlockChangedInvoker()
    {
        OnBlockChanged?.Invoke(currentBlock);
    }
    //getter/setter--------------------------
    public int CurrentBlockGetter()
    {
        return currentBlock;
    }

    public void CurrentBlockAdder(int amt)
    {
        currentBlock += amt;
    }
}