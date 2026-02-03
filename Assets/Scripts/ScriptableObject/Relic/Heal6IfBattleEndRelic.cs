using UnityEngine;

// 실제 유물 구현 예시
[CreateAssetMenu(fileName = "Heal6IfBattleEndRelic", menuName = "Relics/Heal6IfBattleEndRelic")]
public class Heal6IfBattleEndRelic : BaseRelic
{
    public override void OnEnemyDeath()
    {
        if (BattleManager.Instance.ActiveEnemiesGetter().Count == 0)
        {
            PlayerController.Instance.CurrentHpAdder(6);
            if(PlayerController.Instance.CurrentHpGetter() > PlayerController.Instance.PlayerDataGetter().maxHP)
            {
                int dif = PlayerController.Instance.CurrentHpGetter() - PlayerController.Instance.PlayerDataGetter().maxHP;
                PlayerController.Instance.CurrentHpAdder(-dif);
            }
        }
    }
}