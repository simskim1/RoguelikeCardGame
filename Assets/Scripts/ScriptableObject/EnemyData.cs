using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHp;
    public int attackPower;

    public Sprite enemyImage; 

}