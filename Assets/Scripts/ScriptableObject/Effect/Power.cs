using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Power", menuName = "Status/Power")]
public class Power : StatusEffect
{

    public override float OnProcessDamage(float damage, StatusInstance instance)
    {
        float resDamage = damage += instance.stacks;

        bool isPlayer = instance.owner.GetComponent<PlayerController>() != null;
        bool isPlayerTurn = BattleManager.Instance.CurrentStateGetter() == BattleState.PlayerTurn;

        bool isEnemy = instance.owner.GetComponent<Enemy>() != null;
        bool isEnemyTurn = BattleManager.Instance.CurrentStateGetter() == BattleState.EnemyTurn;

        if (isPlayer || isPlayerTurn)
        {
            return resDamage;
        }
        else if (isEnemy || isEnemyTurn)
        {
            return resDamage;
        }
        else
        {
            return damage;
        }
    }
}
