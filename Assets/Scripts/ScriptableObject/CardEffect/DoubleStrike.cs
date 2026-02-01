using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "DoubleStrike", menuName = "CardEffects/DoubleStrike")]
public class DoubleStrike : CardEffect
{
    private float multiplier = 1.0f;

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        targetEnemy.TakeDamage(cardData.value);
        targetEnemy.TakeDamage(cardData.value);
    }
}