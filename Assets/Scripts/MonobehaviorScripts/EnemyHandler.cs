using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;

        // 드래그한 오브젝트에서 카드 정보 가져오기
        CardDisplay card = draggedObject.GetComponent<CardDisplay>();
        Enemy enemy = GetComponent<Enemy>();

        if (card != null && enemy != null)
        {
            // 공격 카드일 때만 데미지 주기
            if (card.cardData.cardType == CardType.Attack)
            {
                enemy.TakeDamage(card.cardData.value);
            }
        }
    }
}