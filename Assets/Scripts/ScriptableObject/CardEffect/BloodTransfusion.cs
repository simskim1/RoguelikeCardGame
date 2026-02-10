using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "BloodTransfusion", menuName = "CardEffects/BloodTransfusion")]
public class BloodTransfusion : CardEffect
{
    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        status.CurrentHpAdder(-3);
        status.HealthChangedInvoker();
        BattleManager.Instance.currentEnergySetter(2);
        BattleManager.Instance.UpdateEnergyUI();
    }
}