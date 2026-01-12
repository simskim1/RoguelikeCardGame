using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image intentIcon;
    [SerializeField] private TextMeshProUGUI intentValueText;
    [SerializeField] private Slider healthSlider;

    // 적의 의도를 업데이트하는 메서드 (Enemy 스크립트에서 호출)
    public void UpdateIntentUI(Sprite icon, int value)
    {
        intentIcon.sprite = icon;
        intentIcon.gameObject.SetActive(true);

        if (value > 0)
        {
            intentValueText.text = value.ToString();
            intentValueText.gameObject.SetActive(true);
        }
        else
        {
            intentValueText.gameObject.SetActive(false); // 버프/디버프 등 수치 없을 때
        }
    }

    // 턴이 끝나거나 의도가 실행된 후 숨기기
    public void HideIntentUI()
    {
        intentIcon.gameObject.SetActive(false);
        intentValueText.gameObject.SetActive(false);
    }
}