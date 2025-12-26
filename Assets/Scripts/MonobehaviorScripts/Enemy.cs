using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Enemy : MonoBehaviour, IDropHandler
{
    public EnemyData enemyData; 
    private int _currentHp;

    [SerializeField] private Slider hpSlider; // UI 슬라이더 연결
    private Image _enemyImage;

    void Start()
    {
        // 자신의 Image 컴포넌트 가져오기
        _enemyImage = GetComponent<Image>();

        if (enemyData != null)
        {
            // SO에 저장된 이미지로 변경
            if (_enemyImage != null && enemyData.enemyImage != null)
            {
                _enemyImage.sprite = enemyData.enemyImage;//이 오브젝트의 컴포넌트인 ememyimage의 스프라이트를 enemyData에 저장된 이미지로 변경
            }

            _currentHp = enemyData.maxHp;//현재체력도 변경.
            UpdateHpUI();
        }
    }

    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        _currentHp = Mathf.Clamp(_currentHp, 0, enemyData.maxHp); // 체력 하한/상한 고정

        Debug.Log($"{enemyData.enemyName}이(가) {amount}의 데미지를 입음! 남은 체력: {_currentHp}");

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
            hpSlider.value = (float)_currentHp / enemyData.maxHp;//이 오브젝트의 슬라이더의 값 조정
        }
    }

    void Die()
    {
        Debug.Log("적이 처치되었습니다!");
        // 여기서 애니메이션이나 파괴 로직 실행
    }

    public void OnDrop(PointerEventData eventData)//드롭시, 즉 마우스를 땠을때 그 마우스를 땐곳 밑에있는 오브젝트들의 OnDrop을 실행한다
    {
        GameObject draggedObject = eventData.pointerDrag;//내 위에서 감지된 놈 확인

        // 드래그한 오브젝트에서 카드 정보 가져오기
        CardDisplay card = draggedObject.GetComponent<CardDisplay>();// 디스플레이를 경유해 CardData에 접속하는 이유는 CardData에 직접적으로 접근할 수 없기 때문. SO는 변수이기에, CardDisplay에 저장된 CardData라는 변수의 정보를 가져온다. 라고 접근해야함.
        Enemy enemy = GetComponent<Enemy>();

        if (card != null && enemy != null)
        {
            // 공격 카드일 때만 데미지 주기
            if (card.cardData.cardType == CardType.Attack)
            {
                enemy.TakeDamage(card.cardData.value);
            }
        }
    }
}