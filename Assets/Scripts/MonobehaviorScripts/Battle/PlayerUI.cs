using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private GameObject blockIcon; // 방어도가 있을 때만 표시할 아이콘

    // 이 메서드가 나중에 PlayerController의 이벤트에 등록될 함수입니다.
    public void UpdateHealthUI(int currentHP, int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        hpText.text = $"{currentHP} / {maxHP}";
    }

    public void UpdateBlockUI(int block)
    {
        blockText.text = block.ToString();
        // 방어도가 0보다 크면 아이콘 활성화
        blockIcon.SetActive(block > 0);
    }
}