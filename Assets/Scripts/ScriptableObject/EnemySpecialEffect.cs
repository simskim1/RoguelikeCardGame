using UnityEngine;

public abstract class EnemySpecialEffect : ScriptableObject
{
    public abstract void Execute(Enemy enemy, PlayerController playerController); 
}
