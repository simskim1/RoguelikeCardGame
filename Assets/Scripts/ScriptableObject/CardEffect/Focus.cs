using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "Focus", menuName = "CardEffects/Focus")]
public class Focus : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        status.CurrentBlockAdder(cardData.value);
        status.BlockChangedInvoker();
        if (status != null)
        {
            if (statusEffect != null)
            {
                for (int i = 0; i < statusEffect.Length; i++)
                    status.StatusControllerGetter().AddStatus(statusEffect[i], player, 2, -1);
            }
        }
    }
}