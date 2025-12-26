using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Enemy : MonoBehaviour, IDropHandler
{
    public EnemyData enemyData; // 설계도 연결
    private int _currentHp;

    [SerializeField] private Slider hpSlider; // UI 슬라이더 연결
    private Image _enemyImage;

    void Start()
    {
        // 1. 자신의 Image 컴포넌트 가져오기
        _enemyImage = GetComponent<Image>();

        if (enemyData != null)
        {
            // 2. SO에 저장된 이미지로 변경
            if (_enemyImage != null && enemyData.enemyImage != null)
            {
                _enemyImage.sprite = enemyData.enemyImage;
            }

            _currentHp = enemyData.maxHp;
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
            hpSlider.value = (float)_currentHp / enemyData.maxHp;
        }
    }

    void Die()
    {
        Debug.Log("적이 처치되었습니다!");
        // 여기서 애니메이션이나 파괴 로직 실행
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;

        // 드래그한 오브젝트에서 카드 정보 가져오기
        CardDisplay card = draggedObject.GetComponent<CardDisplay>();
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