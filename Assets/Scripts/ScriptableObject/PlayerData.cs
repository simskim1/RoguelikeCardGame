using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public int currentHP;
    public int maxHP;
    public int baseEnergy; // 턴마다 회복되는 에너지
    public int money;
    public Sprite characterSprite;
    public int currentStage;

    public MapNode playerNode;
    // 추후 '기본 덱' 리스트 등을 여기에 추가
}