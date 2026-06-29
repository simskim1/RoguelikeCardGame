using UnityEngine;
[CreateAssetMenu(fileName = "EnemyPowerEffect", menuName = "EnemyEffect/EnemyPowerEffect")]
public class EnemyPowerEffect : EnemySpecialEffect
{
    public override void Execute(Enemy enemy, PlayerController playerController)
    {
        StatusController statusController = enemy.GetComponent<StatusController>();
        if (statusController != null)
        {
            statusController.AddStatus(statusController.buff[0], enemy.GameObjectGetter(), 3, 0);
        }
    }
}
