using UnityEngine;

public interface ITurnStartRelic { void OnTurnStart(); }

// 실제 유물 구현 예시
[CreateAssetMenu(fileName = "EnergyRelic", menuName = "Relics/Energy Relic")]
public class EnergyRelic : BaseRelic, ITurnStartRelic
{
    public override void OnTurnStart()
    {
        Debug.Log("턴 시작 시 에너지를 1 추가합니다.");
        BattleManager.Instance.currentEnergy += 1;
    }
}