using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHp;
    public List<EnemyAction> actions;//적의 Action설정

    public Sprite enemyImage;

    [System.Serializable]
    public class EnemyAction
    {
        public IntentType type; // Attack, Defend, Buff, Debuff 등
        public int value;
        public Sprite intentIcon;
    }

    public enum IntentType { Attack, Defend, Strategic }

}