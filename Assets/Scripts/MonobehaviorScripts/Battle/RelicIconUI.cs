using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image iconImage;

    private BaseRelic myInstance; // 현재 이 아이콘이 들고 있는 데이터
    private bool isHovering = false;

    public void UpdateUI(BaseRelic instance)
    {
        myInstance = instance;
        iconImage.sprite = instance.icon;
    }

    // 마우스가 아이콘 영역에 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    // 마우스가 아이콘 영역에서 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        TooltipManager.Instance.HideTooltip(); // 마우스가 나가면 툴팁도 끔
    }

    // 클릭했을 때 (유니티 EventSystem이 좌/우클릭을 구분해줌)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovering && eventData.button == PointerEventData.InputButton.Right)
        {
            // 우클릭 시 툴팁 표시
            TooltipManager.Instance.ShowTooltip(
                myInstance.relicName,
                myInstance.description
            );
        }
    }
}
