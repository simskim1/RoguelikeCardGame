using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "Training", menuName = "CardEffects/Training")]
public class Training : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        StatusController statusController = status.GetComponent<StatusController>();
        var existing = statusController.activeStatuses.Find(s => s.effectData == statusEffect[0]);
        if (existing != null) {
            statusController.AddStatus(statusEffect[0], player, existing.stacks, 0);
        }
        else
        {
            Debug.Log("힘이 없습니다.");
        }
    }
}