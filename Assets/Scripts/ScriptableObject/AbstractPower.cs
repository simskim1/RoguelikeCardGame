using System;
using UnityEngine;
[System.Serializable]

public abstract class AbstractPower : ScriptableObject
{
    public string name; // 소문자 string 권장
    public int amount;

    public virtual void onPlayCard(CardData card) { }
    public virtual void atStartOfTurn() { }
    public virtual void onDamageAllEnemies() { }
    public virtual void onLoseHp() { }
}