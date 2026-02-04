using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "InfiniteGrowth", menuName = "CardEffects/InfiniteGrowth")]
public class InfiniteGrowth : CardEffect
{

    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        BattleManager.Instance.InfiniteGrowthHelper();
    }
}