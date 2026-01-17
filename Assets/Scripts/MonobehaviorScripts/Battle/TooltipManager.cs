using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance; // 어디서든 접근 가능하게 싱글톤 설정

    [SerializeField] private GameObject tooltipWindow;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HideTooltip(); // 시작할 때는 숨김
    }

    private void Update()
    {
        if (tooltipWindow.activeSelf)
        {
            // 2. Legacy(Input.mousePosition) 대신 New System 문법 사용
            // Mouse.current는 현재 연결된 마우스 장치를 참조합니다.
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            transform.position = mousePosition;
        }
    }

    public void ShowTooltip(string title, string content)
    {
        tooltipWindow.SetActive(true);
        titleText.text = title;
        descriptionText.text = content;
    }

    public void HideTooltip()
    {
        tooltipWindow.SetActive(false);
    }
}