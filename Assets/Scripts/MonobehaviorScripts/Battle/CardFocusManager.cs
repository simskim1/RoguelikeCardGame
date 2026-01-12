using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardFocusManager : MonoBehaviour
{
    [SerializeField] private GameObject focusPanel;
    private Canvas _focusCanvas;
    /*
    [Header("Card Basic Info")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image cardIllustration;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    */
    [Header("Status Effect Section")]
    [SerializeField] private Transform effectContainer; // 효과들이 생성될 부모 객체
    [SerializeField] private GameObject effectPrefab;   // 효과 아이템 프리팹

    [Header("Focus View 전용 UI 요소들")]
    [SerializeField] private Image frameImage;
    [SerializeField] private TextMeshProUGUI bigCardName;
    [SerializeField] private TextMeshProUGUI bigCardDesc;
    [SerializeField] private Image bigCardArt;
    [SerializeField] private TextMeshProUGUI bigCardCost;

    [Header("프레임")]
    [SerializeField] public Sprite attackFrame;
    [SerializeField] public Sprite skillFrame;
    [SerializeField] public Sprite powerFrame;

    private StatusEffect[] effectData;
    public static CardFocusManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        focusPanel.SetActive(false); // 시작할 때 꺼두기
        _focusCanvas = focusPanel.GetComponent<Canvas>();
    }

    public void ShowFocusView(CardData data)
    {
        focusPanel.SetActive(true);
        // 최상단으로 올리기 위해 Sorting Order 조정
        _focusCanvas.sortingOrder = 999;
        
        // 데이터 바인딩 로직 호출
        UpdateUI(data);
    }

    void Update()
    {
        // 신규 Input System 방식의 ESC 키 감지
        if (focusPanel.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HideFocusView();
        }
    }

    public void HideFocusView()
    {
        focusPanel.SetActive(false);
    }

    public void UpdateUI(CardData data)
    {
        // 2. 자식 UI 요소들에 데이터 바인딩 (수정해서 보여주기)
        bigCardName.text = data.cardName;
        bigCardDesc.text = ParseKeywords(data.description); // 아까 만든 파싱 함수 활용
        bigCardArt.sprite = data.cardImage;
        bigCardCost.text = data.energyCost.ToString();
        switch (data.cardType)
        {
            case CardType.Attack: frameImage.sprite = attackFrame; break;
            case CardType.Skill: frameImage.sprite = skillFrame; break;
            case CardType.Power: frameImage.sprite = powerFrame; break;
        }
        // 1. 기존에 생성된 툴팁들 제거 (Memory Leak 방지)
        foreach (Transform child in effectContainer)
        {
            Destroy(child.gameObject);
        }

        if (data.cardEffect != null && data.cardEffect.Length > 0)
        {
            foreach (var effectSO in data.cardEffect)//effectSO는 cardEffect
            {
                if (effectSO == null) continue;

                // 3. 각 효과 내부의 상태 이상 배열 순회 (StatusEffect[])
                // 여기서 이중 루프를 돌아야 모든 키워드가 생성됩니다.
                if (effectSO.statusEffect != null)
                {
                    foreach (var sEffect in effectSO.statusEffect)//sEffect는 statusEffect
                    {
                        if (sEffect == null) continue;

                        // 프리팹 생성 및 부모 설정
                        GameObject effectObj = Instantiate(effectPrefab, effectContainer);
                        var itemScript = effectObj.GetComponent<EffectItemUI>();

                        if (itemScript != null)
                        {
                            string coloredName = $"<color=#FFCC00>{sEffect.effectName}</color>";
                            itemScript.Setup(coloredName, sEffect.effectDescription);
                            UnityEngine.Debug.Log($"{sEffect.effectName} 툴팁 생성 완료!");
                        }
                    }
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(effectContainer as RectTransform);
        }
    }

    private string ParseKeywords(string originalText)
    {
        // 예: '출혈'이라는 단어를 빨간색으로 강조
        string parsedText = originalText.Replace("취약", "<color=#FF0000>취약</color>");
        parsedText = parsedText.Replace("방어도", "<color=#0000FF>방어도</color>");

        return parsedText;
    }
}