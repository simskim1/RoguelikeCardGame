using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "StrikeDown", menuName = "CardEffects/StrikeDown")]
public class StrikeDown : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        StatusController statusController = status.GetComponent<StatusController>();
        var existing = statusController.activeStatuses.Find(s => s.effectData == statusEffect[0]);
        if (existing != null) {
            targetEnemy.TakeDamage(cardData.value + existing.stacks);
        }
        else
        {
            targetEnemy.TakeDamage(cardData.value);
        }
    }
}