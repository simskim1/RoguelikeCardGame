using UnityEngine;
using UnityEngine.UI; // Button 컴포넌트 접근을 위해 필요

public class GraveyardButton : MonoBehaviour
{
    private Button myButton;

    void Awake()
    {
        // 1. 현재 오브젝트에 붙은 Button 컴포넌트를 가져옵니다.
        myButton = GetComponent<Button>();

        // 2. 클릭 이벤트에 리스너(함수)를 등록합니다.
        // 델리게이트를 활용한 이벤트 구독 방식입니다.
        myButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        // 하나씩 체크해봅니다.
        if (CardListUI.Instance == null)
        {
            Debug.LogError("CardListUI 인스턴스가 없습니다! 씬에 스크립트가 붙은 오브젝트가 있나요?");
            return;
        }

        if (DeckManager.Instance == null)
        {
            Debug.LogError("DeckManager 인스턴스가 없습니다!");
            return;
        }

        if (DeckManager.Instance.drawPile == null)
        {
            Debug.LogError("drawPile 리스트 자체가 초기화되지 않았습니다!");
            return;
        }
        // 싱글톤 패턴을 잘 활용하고 계시네요!
        CardListUI.Instance.Open("버린 카드 리스트", DeckManager.Instance.discardPile);
    }
}