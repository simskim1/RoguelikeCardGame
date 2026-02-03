using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EnemyData;


public class Enemy : MonoBehaviour, IDropHandler
{

    [SerializeField] private EnemyData[] enemyData;//일단은 배열이 하나이지만, 나중에 래퍼 클래스를 이용하여 랜덤으로 결정된 어느 챕터의 일반, 엘리트, 또는 보스의 어느 몬스터가 나올지를 결정하게 할 수 있다.
    private int enemyWho;
    private int _currentHp;

    [SerializeField] private Slider hpSlider; // UI 슬라이더 연결
    private Image _enemyImage;

    private EnemyAction nextAction;
    [SerializeField] private EnemyUI enemyUI; // 인스펙터에서 연결하거나 GetComponentInChildren

    [SerializeField] private StatusController enemyStatus;
    private StatusController playerStatus;

    private void Awake()
    {
        init();
    }

    void init()
    {
        enemyWho = Random.Range(0, 2);
        // 자신의 Image 컴포넌트 가져오기
        _enemyImage = GetComponent<Image>();

        if (enemyData != null)
        {
            // SO에 저장된 이미지로 변경
            if (_enemyImage != null && enemyData[enemyWho].enemyImage != null)
            {
                _enemyImage.sprite = enemyData[enemyWho].enemyImage;//이 오브젝트의 컴포넌트인 ememyimage의 스프라이트를 enemyData에 저장된 이미지로 변경
            }

            _currentHp = enemyData[enemyWho].maxHp;//현재체력도 변경.
            UpdateHpUI();
        }
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localScale = Vector3.one; // Local Scale을 (1,1,1)로 초기화
            rect.sizeDelta = new Vector2(100, 100); // 원하는 Width, Height 강제 지정
            rect.anchoredPosition = Vector2.zero; // 부모(SpawnPoint)의 정중앙에 위치
        }
        enemyStatus = GetComponent<StatusController>();
        BattleManager.Instance.RegisterEntity(enemyStatus);
        playerStatus = PlayerController.Instance.GetComponent<StatusController>();
    }

    public void TakeDamage(int amount)
    {
        int checkingDamage = playerStatus.DamageCheck(amount);
        int finalDamage =  enemyStatus.DamageCheck(checkingDamage);
        _currentHp -= finalDamage;
        _currentHp = Mathf.Clamp(_currentHp, 0, enemyData[enemyWho].maxHp); // 체력 하한/상한 고정

        Debug.Log($"{enemyData[enemyWho].enemyName}이(가) {finalDamage}의 데미지를 입음! 남은 체력: {_currentHp}");

        UpdateHpUI();

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    void UpdateHpUI()
    {
        if (hpSlider != null)
        {
            // 슬라이더의 Value는 0~1 사이이므로 비율로 계산
            hpSlider.value = (float)_currentHp / enemyData[enemyWho].maxHp;//이 오브젝트의 슬라이더의 값 조정
        }
    }

    void Die()
    {
        BattleManager.Instance.CheckEnemyDeaths();
        // 여기서 애니메이션이나 파괴 로직 실행
    }

    public void OnDrop(PointerEventData eventData)//드롭시, 즉 마우스를 땠을때 그 마우스를 땐곳 밑에있는 오브젝트들의 OnDrop을 실행한다
    {
        CardDisplay card1 = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (card1 != null)
        {
            TargetingArrow arr = card1.TargetingArrowGetter();
            // 카드가 가지고 있는 화살표를 직접 끔
            if (arr != null) card1.TargetingArrowGetter().gameObject.SetActive(false);

            
        }

        GameObject draggedObject = eventData.pointerDrag;//내 위에서 감지된 놈 확인

        // 드래그한 오브젝트에서 카드 정보 가져오기
        CardDisplay card = draggedObject.GetComponent<CardDisplay>();// 디스플레이를 경유해 CardData에 접속하는 이유는 CardData에 직접적으로 접근할 수 없기 때문. SO는 변수이기에, CardDisplay에 저장된 CardData라는 변수의 정보를 가져온다. 라고 접근해야함.
        
        if (card != null)
        {
            // 공격 카드일 때만 데미지 주기
            if(card.CardDataGetter().hasCardEffect ==true && card.CardDataGetter().cardType == CardType.Attack && BattleManager.Instance.CanUseCard(card.CardDataGetter().energyCost))
            {
                foreach (CardEffect effect in card.CardDataGetter().cardEffect)
                {
                    // 부모 틀에 정의된 Execute를 호출하면, 
                    // 실제 데이터(DamageEffect 등)에 따라 다르게 작동합니다. (다형성)
                    effect.Execute(PlayerController.Instance.gameObject, this.gameObject, card.CardDataGetter());
                }
                BattleManager.Instance.UseEnergy(card.CardDataGetter().energyCost);
                DeckManager.Instance.DiscardCardDragged(card.CardDataGetter(), eventData);
                Destroy(draggedObject);
            }
            else if (card.CardDataGetter().cardType == CardType.Attack && BattleManager.Instance.CanUseCard(card.CardDataGetter().energyCost))
            {
                TakeDamage(card.CardDataGetter().value);
                BattleManager.Instance.UseEnergy(card.CardDataGetter().energyCost);
                DeckManager.Instance.DiscardCardDragged(card.CardDataGetter(), eventData);
                Destroy(draggedObject);
            }
        }
    }

    public void DecideNextAction()
    {
        // 예: 리스트에서 랜덤하게 하나 선택하거나 순차적으로 선택
        nextAction = enemyData[enemyWho].actions[Random.Range(0, enemyData[enemyWho].actions.Count)];
        if (nextAction.type == EnemyData.IntentType.Attack)
        {
            enemyUI.UpdateIntentUI(nextAction.intentIcon, nextAction.value);
        }
    }

    public IEnumerator TakeAction()
    {
        // 의도에 따른 실제 로직 실행
        
        if (nextAction.type == IntentType.Attack)
        {
            PlayerController.Instance.TakeDamage(nextAction.value);
        }
        
        // ... 애니메이션 대기 등
        yield return new WaitForSeconds(1.0f);

        DecideNextAction(); // 행동 후 다음 턴 의도 미리 결정
    }

    //getter/setter--------------------------

    public StatusController StatusGetter()
    {
        return enemyStatus;
    }

    public int CurrentHpGetter()
    {
        return _currentHp;
    }
}