using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(fileName = "CardEffect", menuName = "Scriptable Objects/CardEffect")]
public abstract class CardEffect : ScriptableObject
{
    public StatusEffect[] statusEffect;
    public abstract void Execute(GameObject player, GameObject enemy, CardData cardData);
}
