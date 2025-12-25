using UnityEngine;

public enum CardType
{
    Attack,   // 공격
    Skill,    // 스킬
    Power,    // 파워
    Curse     // 저주
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Special
}

public enum TargetType
{
    Self,           
    Enemy,          
    AllEnemies,     
    None            
}