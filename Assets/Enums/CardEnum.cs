using UnityEngine;

public enum CardType
{
    Attack,   
    Skill,    
    Power,    
    Curse     
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

public enum BattleState
{
    Start,      
    PlayerTurn, 
    EnemyTurn, 
    Win,
    Lose
}