using System.Linq;
using UnityEngine;

// MonoBehaviour 대신 아까 만든 CardEffect(ScriptableObject)를 상속받습니다.
[CreateAssetMenu(fileName = "BloodRage_Effect", menuName = "CardEffects/BloodRage_Effect")]
public class BloodRage_Effect : CardEffect
{
    public override void Execute(GameObject player, GameObject enemy, CardData cardData)
    {
        if (player == null)
        {
            Debug.LogError("[비상] Execute에 전달된 player가 null입니다!");
            return;
        }
        // MonoBehaviour가 아니므로 GetComponent를 쓸 때 'player'를 참조해야 합니다.
        // 예: PlayerStatus라는 스크립트에 방어도가 있다면
        var status = player.GetComponent<PlayerController>();
        Enemy targetEnemy = enemy.GetComponent<Enemy>();
        // 1. PlayerController 확인
        if (status == null)
        {
            Debug.LogError($"[에러] {player.name} 객체에 PlayerController가 없습니다!");
            return;
        }

        // 2. power 배열 존재 확인
        if (power == null || power.Length == 0)
        {
            Debug.LogError($"[에러] {this.name}의 power 배열이 비어있습니다. 인스펙터에서 Size를 1로 늘려주세요.");
            return;
        }

        // 3. power[0] 할당 확인
        if (power[0] == null)
        {
            Debug.LogError($"[에러] {this.name}의 power[0] (Element 0)에 에셋이 할당되지 않았습니다.");
            return;
        }
        status.PowerAdder(power[0]);
    }
}