using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "BloodThirst", menuName = "CardEffects/BloodThirst")]
public class BloodThirst : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        // MonoBehaviour가 아니므로 GetComponent를 쓸 때 'player'를 참조해야 합니다.
        // 예: PlayerStatus라는 스크립트에 방어도가 있다면
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        status.CurrentHpAdder(-5);
        status.HealthChangedInvoker();
        targetEnemy.TakeDamage(cardData.value);
    }
}