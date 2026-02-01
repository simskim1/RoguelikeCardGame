using System.Collections;
using UnityEngine;


// 실제 유물 구현 예시
[CreateAssetMenu(fileName = "DrawFirstTurnRelic", menuName = "Relics/DrawFirstTurnRelic")]
public class DrawFirstTurnRelic : BaseRelic, ITurnStartRelic
{
    public override void OnTurnStart()
    {
        if(BattleManager.Instance.IsFirstTurnGetter())
        {
            // DeckManager에 코루틴으로 드로우를 요청하거나, 
            // 간단하게 Invoke를 사용해 테스트해보세요.
            DeckManager.Instance.StartCoroutine(DelayedDraw(0.1f));
            Debug.Log("액티베이티드 - 드로우 예약");
        }
        else
        {
            Debug.Log("페일드");
        }
    }

    private IEnumerator DelayedDraw(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeckManager.Instance.DrawCard(1);
    }
}