using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "ShieldAttack", menuName = "CardEffects/ShieldAttack")]
public class ShieldAttack : CardEffect
{
    public float multiplier = 1.0f;
    public StatusEffect vulnerableEffect;

    public override void Execute(GameObject player, GameObject enemy)
    {
        // MonoBehaviour가 아니므로 GetComponent를 쓸 때 'player'를 참조해야 합니다.
        // 예: PlayerStatus라는 스크립트에 방어도가 있다면
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        if (status != null)
        {
            int damage = Mathf.RoundToInt(status.currentBlock * multiplier);
            targetEnemy.TakeDamage(damage);
            if (vulnerableEffect != null)
            {
                targetEnemy.status.AddStatus(vulnerableEffect, 1, 1);
            }
            Debug.Log($"방어도 {status.currentBlock}만큼 공격!");
        }
    }
}