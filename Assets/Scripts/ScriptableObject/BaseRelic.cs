using UnityEngine;
using UnityEngine.UI;

public enum RelicRarity { Common, Rare, Boss }

public abstract class BaseRelic : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string RelicID;
    [SerializeField] private string RelicName;
    [SerializeField, TextArea] private string Description;
    [SerializeField] private Sprite Icon;
    [SerializeField] private RelicRarity Rarity;

    // 외부(UI, Shop 등)에서는 이 프로퍼티를 통해 읽기만 가능
    public string relicID => RelicID;
    public string relicName => RelicName;
    public string description => Description;
    public Sprite icon => Icon;
    public RelicRarity rarity => Rarity;

    // 실행 시점(Hooks) - 기본적으로는 아무 일도 하지 않음(Virtual)
    public virtual void OnBattleStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnCardPlayed(CardData card) { }
    public virtual void OnEnemyDeath() { }
    public virtual void OnTakeDamage(int amount) { }
}