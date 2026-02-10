using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "AttackStance", menuName = "CardEffects/AttackStance")]
public class AttackStance : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        // MonoBehaviour가 아니므로 GetComponent를 쓸 때 'player'를 참조해야 합니다.
        // 예: PlayerStatus라는 스크립트에 방어도가 있다면
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        StatusController statusController = status.GetComponent<StatusController>();
        
        statusController.AddStatus(statusEffect[0], player, 2, 0);

        DeckManager.Instance.DrawCard(1);
    }
}