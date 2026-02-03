using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "WithAllMyMight", menuName = "CardEffects/WithAllMyMight")]
public class WithAllMyMight : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        DeckManager.Instance.DrawCard(3);
        DeckManager.Instance.CanDrawSetter(false);
    }
}