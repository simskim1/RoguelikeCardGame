using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance; // 접근 편의를 위한 싱글톤

    [Header("플레이어")]
    public PlayerData playerData;

    [Header("상점화면 버튼들")]
    public Button healButton;
    public Button shopButton;

    [Header("선택 확인 패널")]
    public GameObject checkPanel;
    public TextMeshProUGUI panelText;
    public Button buttonAccept;
    public Button buttonRefuse;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }
    void Start()
    {
        checkPanel.SetActive(false);
        healButton.onClick.AddListener(HealCheck);
    }

    private void Return()
    {
        checkPanel.SetActive(false);
    }
    //휴식 버튼에 대한 코드들--------------------------------------
    void HealCheck()
    {
        checkPanel.SetActive(true);
        panelText.text = "회복하시겠습니까?\n최대채력의 15%만큼을 회복하고,\n상점을 떠나게 됩니다.";
        buttonAccept.onClick.AddListener(Rest);
        buttonRefuse.onClick.AddListener(Return);
    }

    void Rest()
    {
        checkPanel.SetActive(false);
        playerData.currentHP += Mathf.RoundToInt((float)(playerData.maxHP * 0.15));
        if (playerData.currentHP > playerData.maxHP)
        {
            playerData.currentHP = playerData.maxHP;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }
}
