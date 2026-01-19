using UnityEngine;

public enum RelicRarity { Normal, Rare, Boss }

public abstract class BaseRelic : ScriptableObject
{
    [Header("Basic Info")]
    public string relicID;
    public string relicName;
    [TextArea] public string description;
    public Sprite icon;
    public RelicRarity rarity;

    // 실행 시점(Hooks) - 기본적으로는 아무 일도 하지 않음(Virtual)
    public virtual void OnBattleStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnCardPlayed(CardData card) { }
    public virtual void OnEnemyDeath() { }
    public virtual void OnTakeDamage(int amount) { }
}