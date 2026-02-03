using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "Repair", menuName = "CardEffects/Repair")]
public class Repair : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        DeckManager.Instance.DrawCard(2);
        status.CurrentBlockAdder(cardData.value);
        status.BlockChangedInvoker();
    }
}