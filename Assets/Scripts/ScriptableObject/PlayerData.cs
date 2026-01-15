using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public int maxHP;
    public int baseEnergy; // 턴마다 회복되는 에너지
    public Sprite characterSprite;

    public MapNode playerNode;
    // 추후 '기본 덱' 리스트 등을 여기에 추가
}